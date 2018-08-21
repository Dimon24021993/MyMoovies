using AngleSharp.Dom;
using MyMovies.Domain.Entities;
using MyMovies.Domain.Enums;
using MyMovies.Parser.DataBase;
using MyMovies.Parser.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace MyMovies.Parser.Sites
{
    public static class Kinogo
    {
        public static int Movies = 0;
        public static void ParseKinogo()
        {
            var client = new HttpClient(new HttpClientHandler() { CookieContainer = new CookieContainer() })
            { BaseAddress = new Uri("http://kinogo.cc") };

            var pages = Convert.ToInt32(client.GetDocument("").QuerySelector(".bot-navigation a:nth-last-child(2)").TextContent);

            for (var i = 1; i < pages; i++)
            {
                ParseKinogoPage(ref client, $"page/{i}/");
                Console.WriteLine($"Added {Movies} movies");
            }

            Console.WriteLine("Done Parse Kinogo!");
        }

        private static void ParseKinogoPage(ref HttpClient client, string href)
        {
            var items = client.GetDocument(href)
                           .QuerySelectorAll("h2.zagolovki a")
                           .Select(x => x.GetAttribute("href")).ToList();
            if (items.Any())
            {
                foreach (var item in items)
                {
                    ParseKinogoMovie(ref client, item);
                }
            }
        }

        private static void ParseKinogoMovie(ref HttpClient client, string href)
        {
            try
            {
                var document = client.GetDocument(href, "Windows-1251");
                var movieName = document.QuerySelector("h1").TextContent.Split('(')[0].Trim();
                var nodes = document.QuerySelector(".fullimg div[id]").ChildNodes;
                var cred = new Dictionary<INode, string>();

                foreach (var node in nodes)
                {
                    if (node.GetType().FullName == "AngleSharp.Dom.Comment")
                        continue;
                    else
                    if (node.GetType().FullName == "AngleSharp.Dom.Html.HtmlBoldElement")
                    {
                        cred.Add(node, "");
                    }
                    else if (!string.IsNullOrWhiteSpace(node.TextContent.Trim()))
                    {
                        if (cred.Count == 0)
                        {
                            cred.Add(node, node.TextContent.Trim());
                        }
                        else
                        {
                            cred[cred.Last().Key] += node.TextContent.Trim();
                        }
                    }
                }

                #region User

                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Language = Language.Ru,
                    UserName = "MyMovieParser2",
                    Login = "MyMovieParser2",
                    Password = "MyMovieParser2"
                };
                DbTasks.AddUsersIntoDb(ref user);

                #endregion

                #region Movie&Description

                var rate = document.QuerySelector("ul[itemprop='rating'] li.current-rating").GetStyle("width");

                var movie = new Movie()
                {
                    Id = Guid.NewGuid(),
                    OriginalName = movieName,
                    Country = cred.FirstOrDefault(x => x.Key.TextContent == "Страна:").Value.Trim() ?? "",
                    Date = DateTime.Parse(cred.FirstOrDefault(x => x.Key.TextContent?.IndexOf("Премьера", StringComparison.Ordinal) > -1).Value ?? DateTime.Now.ToString("G")),
                    Duration = TimeSpan.Parse(cred.FirstOrDefault(x => x.Key.TextContent == "Продолжительность:").Value?.Trim() ?? "0"),
                    Rate = Convert.ToDecimal(new Regex("[a-zA-Z]+").Replace(rate, ""), CultureInfo.InvariantCulture) / 20.0M
                };
                DbTasks.AddMovieAndDescriptionIntoDb(ref movie, ref Movies, new Description()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Language = Language.Ru,
                    MovieName = movieName,
                    UserId = user.Id,
                    DescriptionText = cred.First().Value.Trim()
                });

                #endregion

                #region Actors

                DbTasks.AddPersonsIntoDb(cred.FirstOrDefault(x => x.Key.TextContent == "В ролях:").Value?.Split(','),
                    movie.Id, JobType.Actor);

                #endregion

                #region Scenarists

                //DbTasks.AddPersonsIntoDb(
                //    cred.FirstOrDefault(x => x.Key.TextContent == "Сценарий:").Value?.Split(','), movie.Id, JobType.Scenarist);

                #endregion

                #region Directors

                DbTasks.AddPersonsIntoDb(cred.FirstOrDefault(x => x.Key.TextContent == "Режиссер:").Value?.Split(','),
                    movie.Id, JobType.Director);

                #endregion

                #region Tags

                DbTasks.AddTagsIntoDb(cred.FirstOrDefault(x => x.Key.TextContent == "Жанр:").Value?.Split(','), Language.Ru, movie.Id);

                #endregion
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}