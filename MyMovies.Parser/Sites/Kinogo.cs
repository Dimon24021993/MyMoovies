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
using System.Text;
using System.Text.RegularExpressions;

namespace MyMovies.Parser.Sites
{
    public static class Kinogo
    {
        public static int Movies = 0;
        public static void ParseKinogo()
        {
            var client = new HttpClient(new HttpClientHandler() { CookieContainer = new CookieContainer() })
            { BaseAddress = new Uri("http://kinogo.cc/") };

            var pages = Convert.ToInt32(client.GetDocument("").QuerySelector(".bot-navigation a:nth-last-child(2)").TextContent);

            for (var i = 1; i < pages; i++)
            {
                ParseKinogoPage(ref client, $"{client.BaseAddress}page/{i}/");
            }

            Console.WriteLine($"Added {Movies} movies");
            Console.WriteLine("Done Parse Kinogo!");
        }

        private static void ParseKinogoPage(ref HttpClient client, string href)
        {
            List<string> items = client.GetDocument(href)
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
                    if (node.GetType().FullName == "AngleSharp.Html.Dom.HtmlBoldElement")
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
                var date = DateTime.TryParseExact(cred.FirstOrDefault(x => x.Key.TextContent?.IndexOf("Премьера", StringComparison.Ordinal) > -1).Value, "dd MMMM yyyy", CultureInfo.GetCultureInfo("RU-ru"), DateTimeStyles.AssumeLocal, out var ini);

                var movie = new Movie()
                {
                    Id = Guid.NewGuid(),
                    OriginalName = movieName,
                    Country = cred.FirstOrDefault(x => x.Key.TextContent == "Страна:").Value?.Trim() ?? "",
                    Date = date ? ini : DateTime.Now,
                    Duration = TimeSpan.Parse(cred.FirstOrDefault(x => x.Key.TextContent == "Продолжительность:").Value?.Trim().Trim('~').Trim() ?? "0"),
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

                var rates = new List<Rate>()
                {
                    new Rate()
                    {
                        Value = Convert.ToDecimal(new Regex("[a-zA-Z]+").Replace(rate, ""),
                                    CultureInfo.InvariantCulture) / 20.0M,
                        Id = Guid.NewGuid(),
                        MovieId = movie.Id,
                        UserId = user.Id,
                        RateType = RateType.Kinogo
                    }
                };

                DbTasks.AddRates(rates);

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

                #region Pictures
                var items = new List<Item>();
                string videoHref;
                var pictureHrefs = document.QuerySelectorAll(".screens a").Select(x => x.GetAttribute("href"));
                var pictures = pictureHrefs.Select((pictureHref, index) => new Picture()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Name = pictureHref.Split("/").Last(),
                    Href = pictureHref,
                    Sort = index,
                    SourceType = SourceType.Kinogo,
                    Type = PictureType.Preview
                }).ToList();


                var bannerHref = document.QuerySelector($"#news-id-{href.Split("-").First().Split("/").Last()} a").GetAttribute("href");
                var banner = new Picture()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Name = bannerHref.Split("/").Last(),
                    Href = bannerHref,
                    Sort = 1,
                    SourceType = SourceType.Kinogo,
                    Type = PictureType.Banner
                };

                pictures.Add(banner);

                var boxes = document.QuerySelectorAll(".box");
                var script = boxes.First()?.Children?.First()?.InnerHtml ?? "";
                if (!string.IsNullOrWhiteSpace(script))
                {
                    if (script.Length < 30)
                    {
                        //sdafsdf
                    }
                    var text = script.Substring(30, script.Length - 4 - 30);
                    var encoded = Encoding.UTF8.GetString(Convert.FromBase64String(text));

                    var poster = encoded.InnerString("poster=", "\"");
                    if (!string.IsNullOrWhiteSpace(poster))
                        pictures.Add(new Picture()
                        {
                            Id = Guid.NewGuid(),
                            MovieId = movie.Id,
                            Name = poster.Split("/").Last(),
                            Href = poster,
                            Sort = 1,
                            SourceType = SourceType.Kinogo,
                            Type = PictureType.Poster
                        });

                    videoHref = encoded.InnerString("file=", "&amp;");
                    if (string.IsNullOrWhiteSpace(videoHref))
                        videoHref = encoded.InnerString("pl=", "&amp;");
                    if (!string.IsNullOrWhiteSpace(videoHref))
                        items.Add(new Item()
                        {
                            Id = Guid.NewGuid(),
                            Value = videoHref,
                            ItemType = ItemType.Link,
                            MovieId = movie.Id,
                            SourceType = SourceType.Kinogo
                        });

                }
                else
                {
                    var encoded = boxes.First()?.Children.Last().TextContent;

                    var poster = encoded.InnerString("\"poster\":\"", "\"");
                    if (!string.IsNullOrWhiteSpace(poster))
                        pictures.Add(new Picture()
                        {
                            Id = Guid.NewGuid(),
                            MovieId = movie.Id,
                            Name = poster.Split("/").Last(),
                            Href = poster,
                            Sort = 1,
                            SourceType = SourceType.Kinogo,
                            Type = PictureType.Poster
                        });

                    videoHref = encoded.InnerString("\"file\":\"", "\"");
                    if (!string.IsNullOrWhiteSpace(videoHref))
                        items.Add(new Item()
                        {
                            Id = Guid.NewGuid(),
                            Value = videoHref,
                            ItemType = ItemType.Link,
                            MovieId = movie.Id,
                            SourceType = SourceType.Kinogo
                        });

                    videoHref = encoded.InnerString("file:\"", "\"");
                    items.Add(new Item()
                    {
                        Id = Guid.NewGuid(),
                        Value = videoHref,
                        ItemType = ItemType.Link,
                        MovieId = movie.Id,
                        SourceType = SourceType.Kinogo
                    });

                }

                var content = boxes.Last().Children.Last().TextContent;
                videoHref = content.InnerString("file:\"", "\"");
                if (!string.IsNullOrWhiteSpace(videoHref))
                    items.Add(new Item()
                    {
                        Id = Guid.NewGuid(),
                        Value = videoHref,
                        ItemType = ItemType.Trailer,
                        MovieId = movie.Id,
                        SourceType = SourceType.Kinogo
                    });


                DbTasks.AddPictures(pictures);
                DbTasks.AddItems(items);

                #endregion
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}