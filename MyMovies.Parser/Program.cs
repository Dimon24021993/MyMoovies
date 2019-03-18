using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMovies.DAL;
using MyMovies.Parser.DataBase;
using MyMovies.Parser.Sites;
using System;
using System.IO;

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

            DbTasks.Context = new DataBaseContext(new DbContextOptionsBuilder<DataBaseContext>().UseSqlServer(configuration.GetConnectionString("DataBase")).Options);
            Multiplex.ParseMultiplex();
            Kinogo.ParseKinogo();

            Console.ReadKey();
        }
    }
}
