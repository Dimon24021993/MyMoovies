using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Microsoft.Extensions.Configuration;
using MyMovies.DAL;
using MyMovies.Parser.DataBase;
using MyMovies.Parser.Sites;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace MyMovies.Parser
{
    internal static class Program
    {
        public static HtmlParser Parser { get; set; } = new HtmlParser();

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            DbTasks.Context = new DataBaseContext(configuration.GetSection("Connections"));

            Multiplex.ParseMultiplex();
            Kinogo.ParseKinogo();

            Console.ReadKey();
        }

        public static IHtmlDocument GetDocument(this HttpClient client, string href, string charset = null)
        {
            var res = "";
            if (charset != null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                res = Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding(charset), Encoding.UTF8, client.GetAsync(href).Result.Content.ReadAsByteArrayAsync().Result));
            }
            else
            {
                res = client.GetAsync(href).Result.Content.ReadAsStringAsync().Result;
            }
            var document = Parser.ParseAsync(res).Result;
            return document;
        }
    }
}
