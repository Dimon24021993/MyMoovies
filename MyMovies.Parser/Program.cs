using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMovies.DAL;
using MyMovies.Parser.DataBase;
using MyMovies.Parser.Sites;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

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

            DbTasks.Context = new DataBaseContext(new DbContextOptionsBuilder<DataBaseContext>().UseSqlServer(configuration.GetSection("Connections")["DataBase"]).Options);
            //Multiplex.ParseMultiplex();
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
            var document = Parser.ParseDocumentAsync(res).Result;
            return document;
        }
    }
}
