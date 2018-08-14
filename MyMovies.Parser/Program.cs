using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Microsoft.Extensions.Configuration;
using MyMovies.DAL;

namespace MyMovies.Parser
{
    static class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static string ConnStr { get; set; }
        public static DataBaseContext Context { get; set; }
        public static HtmlParser Parser { get; set; } = new HtmlParser();

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            Context = new DataBaseContext(Configuration.GetSection("Connections"));

            var domain = AppDomain.CreateDomain("multiplex");


            Parse();



            Console.ReadKey();
        }

        private static void Parse()
        {
            var consumerQueue = new AsyncProducerConsumerQueue<string>(ParseTask, "parse", int.Parse(Configuration.GetSection("ParserConfig")["treads"]));

            foreach (var site in Configuration.GetSection("ParsedSites").GetChildren())
            {
                consumerQueue.Produce(site.Key);
            }
        }

        public static void ParseTask(string site)
        {
            var siteConfig = Configuration.GetSection("ParsedSites").GetSection(site);
            var Client = new HttpClient(new HttpClientHandler() { CookieContainer = new CookieContainer() })
            { BaseAddress = new Uri(siteConfig["baseAddress"]) };
            /*
            var startHref = siteConfig["startHref"];
            var hrefSelect = siteConfig["hrefSelect"];
            var hrefSelectTag = siteConfig["hrefSelectTag"];

            var document = Client.GetDocument(startHref);
            var allMovies = document.QuerySelectorAll(hrefSelect);
            var items = allMovies.Select(x=>x.GetAttribute(hrefSelectTag));
            */
            var items = Client.GetDocument(siteConfig["startHref"])
                .QuerySelectorAll(siteConfig["hrefSelect"])
                .Select(x => x.GetAttribute(siteConfig["hrefSelectTag"]));

            if (items.Any())
            {
                var movieParser = new AsyncProducerConsumerQueue<string>(x =>
                {
                    var moviePageConfig = siteConfig.GetSection("moviePage");




                }, site, Convert.ToInt32(siteConfig["threads"]));

                foreach (var item in items)
                {
                    movieParser.Produce(item);
                }
            }

        }

        private static IHtmlDocument GetDocument(this HttpClient Client, string href)
        {
            var res = Client.GetAsync(href).Result.Content.ReadAsStringAsync().Result;
            var document = Parser.ParseAsync(res).Result;
            return document;
        }
    }
}
