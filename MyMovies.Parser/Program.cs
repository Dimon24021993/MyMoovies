using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Microsoft.Extensions.Configuration;
using MyMovies.DAL;
using MyMovies.Domain.Entities;
using MyMovies.Domain.Enums;
using Newtonsoft.Json;

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


            //Parse();


            Console.ReadKey();
        }

        //private static void Parse()
        //{
        //    var consumerQueue = new AsyncProducerConsumerQueue<string>(ParseTask, "parse", int.Parse(Configuration.GetSection("ParserConfig")["treads"]));

        //    foreach (var site in Configuration.GetSection("ParsedSites").GetChildren())
        //    {
        //        consumerQueue.Produce(site.Key);
        //    }
        //}

        //public static void ParseTask(string site)
        //{
        //    List<Movie> Movies = new List<Movie>();

        //    var siteConfig = Configuration.GetSection("ParsedSites").GetSection(site);
        //    var client = new HttpClient(new HttpClientHandler() { CookieContainer = new CookieContainer() })
        //    { BaseAddress = new Uri(siteConfig["baseAddress"]) };
        //    /*
        //    var startHref = siteConfig["startHref"];
        //    var hrefSelect = siteConfig["hrefSelect"];
        //    var hrefSelectTag = siteConfig["hrefSelectTag"];

        //    var document = Client.GetDocument(startHref);
        //    var allMovies = document.QuerySelectorAll(hrefSelect);
        //    var items = allMovies.Select(x=>x.GetAttribute(hrefSelectTag));
        //    */
        //    var items = client.GetDocument(siteConfig["startHref"])
        //        .QuerySelectorAll(siteConfig["hrefSelect"])
        //        .Select(x => x.GetAttribute(siteConfig["hrefSelectTag"]));

        //    if (items.Any())
        //    {
        //        var movieParser = new AsyncProducerConsumerQueue<string>(x =>
        //        {
        //            var moviePageConfig = siteConfig.GetSection("moviePage");

        //            var document = client.GetDocument(x);
        //            var movieName = document.QuerySelector("h1").TextContent;
        //            var results = document
        //                .QuerySelectorAll("ul.movie_credentials li")
        //                .ToDictionary(elem => elem.FirstElementChild.TextContent);

        //            var MovieId = Guid.NewGuid();

        //            #region Actors

        //            var actorNames = results.FirstOrDefault(cred => cred.Key == "В главных ролях:").Value?.LastElementChild
        //                ?.TextContent.Split(',');
        //            var roles = new List<Person>();
        //            if (actorNames != null)
        //                foreach (var name in actorNames)
        //                    roles.Add(new Person()
        //                    {
        //                        PersonId = Guid.NewGuid(),
        //                        Name = name.Trim(),
        //                        Actor = true
        //                    });

        //            #endregion

        //            #region Tags

        //            var tagNamesEng = results.FirstOrDefault(cred => cred.Key == "Жанр:").Value?.LastElementChild.Children.Select(item => item.GetAttribute("href").Split('/').Last());
        //            var tags = new List<Tag>();
        //            if (tagNamesEng != null)
        //                foreach (var name in tagNamesEng)
        //                    tags.Add(new Tag()
        //                    {
        //                        TagId = Guid.NewGuid(),
        //                        TagText = name.Trim(),
        //                        Language = Language.En,
        //                        MovieId = MovieId
        //                    });
        //            var tagNamesRus = results.FirstOrDefault(cred => cred.Key == "Жанр:").Value?.LastElementChild.Children.Select(item => item.TextContent);
        //            if (tagNamesRus != null)
        //                foreach (var name in tagNamesRus)
        //                    tags.Add(new Tag()
        //                    {
        //                        TagId = Guid.NewGuid(),
        //                        TagText = name.Trim(),
        //                        Language = Language.Ru,
        //                        MovieId = MovieId
        //                    });

        //            #endregion

        //            #region Scenarists

        //            var scenaristsNames = results.FirstOrDefault(cred => cred.Key == "Сценарий:").Value?.LastElementChild?.TextContent.Split(',');
        //            var scenarists = new List<Person>();
        //            if (scenaristsNames != null)
        //                foreach (var name in scenaristsNames)
        //                    scenarists.Add(new Person()
        //                    {
        //                        PersonId = Guid.NewGuid(),
        //                        Name = name.Trim(),
        //                        Scenarist = true
        //                    });

        //            #endregion

        //            #region Directors

        //            var directorsNames = results.FirstOrDefault(cred => cred.Key == "Режиссер:").Value?.LastElementChild?.TextContent.Split(',');
        //            var directors = new List<Person>();
        //            if (directorsNames != null)
        //                foreach (var name in directorsNames)
        //                    directors.Add(
        //                        new Person()
        //                        {
        //                            PersonId = Guid.NewGuid(),
        //                            Name = name.Trim(),
        //                            Director = true
        //                        });


        //            #endregion

        //            var movie = new Movie()
        //            {
        //                MovieId = MovieId,
        //                OriginalName = results.FirstOrDefault(cred => cred.Key == "Оригинальное название:").Value?.LastElementChild?.TextContent.Trim() ?? movieName,
        //                Country = results.FirstOrDefault(cred => cred.Key == "Производство:").Value?.LastElementChild?.TextContent.Trim() ?? "",
        //                Date = DateTime.Parse(results.FirstOrDefault(cred => cred.Key == "Дата выхода:").Value?.LastElementChild?.TextContent.Trim() ?? DateTime.Now.ToString("d")),
        //                Director = directors ?? null,
        //                Duration = TimeSpan.Parse(results.FirstOrDefault(cred => cred.Key == "Длительность:").Value?.LastElementChild?.TextContent.Trim() ?? "0"),
        //                Scenarist = scenarists ?? null,
        //                MainRoles = roles,
        //                Tags = tags
        //            };

        //            Movies.Add(movie);
        //        }, site, Convert.ToInt32(siteConfig["threads"]));

        //        foreach (var item in items)
        //        {
        //            movieParser.Produce(item);
        //        }

        //        while (items.Count() > Movies.Count)
        //        {
        //            Task.Delay(1000);
        //        }

        //        File.AppendAllText(Directory.GetCurrentDirectory() + "/Movies.json", JsonConvert.SerializeObject(Movies));
        //        //var dsf = Context.Movies.Select(x => x.MainRoles);
        //        Context.Movies.AddRange(Movies);
        //        Context.SaveChanges();

        //        Console.WriteLine("Done!");
        //    }

        //}

        //private static IHtmlDocument GetDocument(this HttpClient Client, string href)
        //{
        //    var res = Client.GetAsync(href).Result.Content.ReadAsStringAsync().Result;
        //    var document = Parser.ParseAsync(res).Result;
        //    return document;
        //}
    }
}
