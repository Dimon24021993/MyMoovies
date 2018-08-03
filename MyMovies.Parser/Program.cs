using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using Microsoft.Extensions.Configuration;
using MyMovies.DAL;

namespace MyMovies.Parser
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static string ConnStr { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            ConnStr = Configuration.GetSection("Connections")["DataBase"];

            DataBaseContext context = new DataBaseContext(Configuration.GetSection("Connections"));

            

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

        }
    }
}
