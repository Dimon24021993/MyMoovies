using MyMovies.Domain.Entities;
using MyMovies.Domain.Enums;
using MyMovies.Parser.DataBase;
using MyMovies.Parser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace MyMovies.Parser.Sites
{
    public static class Multiplex
    {
        public static int Movies = 0;
        public static void ParseMultiplex()
        {
            var client = new HttpClient(new HttpClientHandler() { CookieContainer = new CookieContainer() })
            { BaseAddress = new Uri("https://multiplex.ua") };

            var items = client.GetDocument("/soon")
                .QuerySelectorAll("a[data-genretype='Кино']")
                .Select(x => x.GetAttribute("href")).ToList();

            if (items.Any())
            {

                foreach (var item in items)
                {
                    ParseMultiplexMovie(ref client, item);
                }
            }
            Console.WriteLine($"Added {Movies} movies");
            Console.WriteLine("Done Parse Multiplex!");
        }

        private static void ParseMultiplexMovie(ref HttpClient client, string href)
        {
            try
            {
                var document = client.GetDocument(href);
                var movieName = document.QuerySelector("h1").TextContent;
                var results = document
                    .QuerySelectorAll("ul.movie_credentials li")
                    .ToDictionary(elem => elem.FirstElementChild.TextContent);

                #region User

                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Language = Language.Ru,
                    UserName = "MyMovieParser1",
                    Login = "MyMovieParser1",
                    Password = "MyMovieParser1"
                };
                DbTasks.AddUsersIntoDb(ref user);


                #endregion

                #region Movie&Description

                var movie = new Movie()
                {
                    Id = Guid.NewGuid(),
                    OriginalName =
                        results.FirstOrDefault(cred => cred.Key == "Оригинальное название:").Value
                            ?.LastElementChild?.TextContent.Trim() ?? movieName,
                    Country = results.FirstOrDefault(cred => cred.Key == "Производство:").Value
                                  ?.LastElementChild?.TextContent.Trim() ?? "",
                    Date = DateTime.Parse(
                        results.FirstOrDefault(cred => cred.Key == "Дата выхода:").Value?.LastElementChild
                            ?.TextContent.Trim() ?? DateTime.Now.ToString("d")),
                    Duration = TimeSpan.Parse(results.FirstOrDefault(cred => cred.Key == "Длительность:").Value
                                                  ?.LastElementChild?.TextContent.Trim() ?? "0"),

                };
                movie.Rates = results.Where(y => y.Key == "Рейтинг Imdb:" || y.Key == "Рейтинг Kinopoisk:").Select(cr =>
                    new Rate()
                    {
                        Id = Guid.NewGuid(),
                        MovieId = movie.Id,
                        Value = Convert.ToDecimal(cr.Value),
                        UserId = user.Id,
                        RateType = cr.Key.Contains("Kinopoisk") ? RateType.Kinopoisk : cr.Key.Contains("Imdb") ? RateType.Imdb : RateType.Multiplex
                    }
                ).ToList();

                DbTasks.AddMovieAndDescriptionIntoDb(ref movie, ref Movies, new Description()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Language = Language.Ru,
                    MovieName = movieName,
                    UserId = user.Id,
                    DescriptionText = document.QuerySelector(".movie_description").TextContent
                });

                #endregion

                #region Actors

                DbTasks.AddPersonsIntoDb(
                    results.FirstOrDefault(cred => cred.Key == "В главных ролях:").Value?.LastElementChild?.TextContent
                        .Split(','), movie.Id, JobType.Actor);

                #endregion

                #region Scenarists

                DbTasks.AddPersonsIntoDb(
                    results.FirstOrDefault(cred => cred.Key == "Сценарий:").Value?.LastElementChild?.TextContent
                        .Split(','), movie.Id, JobType.Scenarist);

                #endregion

                #region Directors

                DbTasks.AddPersonsIntoDb(
                    results.FirstOrDefault(cred => cred.Key == "Режиссер:").Value?.LastElementChild?.TextContent
                        .Split(','), movie.Id, JobType.Director);

                #endregion

                #region Tags

                DbTasks.AddTagsIntoDb(
                    results.FirstOrDefault(cred => cred.Key == "Жанр:").Value?.LastElementChild.Children
                        .Select(item => item.GetAttribute("href").Split('/').Last()), Language.En, movie.Id);

                DbTasks.AddTagsIntoDb(
                    results.FirstOrDefault(cred => cred.Key == "Жанр:").Value?.LastElementChild.Children
                        .Select(item => item.TextContent), Language.Ru, movie.Id);

                #endregion

                #region Pictures

                var pictureHref = document.QuerySelector(".poster")?.GetAttribute("src") ?? "";
                if (!string.IsNullOrWhiteSpace(pictureHref)) pictureHref = client.BaseAddress.AbsoluteUri + pictureHref;

                var picture = new Picture()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Name = pictureHref.Split("/").Last(),
                    Href = pictureHref,
                    Sort = 1,
                    SourceType = SourceType.Multiplex,
                    Type = PictureType.Poster
                };
                var pictureHref2 = document.QuerySelector("#mvi_poster")?.GetAttribute("value") ?? "";
                if (!string.IsNullOrWhiteSpace(pictureHref2)) pictureHref2 = client.BaseAddress.AbsoluteUri + pictureHref2;

                var picture2 = new Picture()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Name = pictureHref2.Split("/").Last(),
                    Href = pictureHref2,
                    Sort = 1,
                    SourceType = SourceType.Multiplex,
                    Type = PictureType.Poster
                };
                var pictureHref3 = document.QuerySelector(".movie_mask")?.GetStyle("background")?.Replace("url('", "").Replace("')", "").Trim() ?? "";
                if (!string.IsNullOrWhiteSpace(pictureHref3)) pictureHref3 = client.BaseAddress.AbsoluteUri + pictureHref3;

                var picture3 = new Picture()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Name = pictureHref3.Split("/").Last(),
                    Href = pictureHref3,
                    Sort = 1,
                    SourceType = SourceType.Multiplex,
                    Type = PictureType.BackGroung
                };
                DbTasks.AddPictures(new List<Picture> { picture, picture2, picture3 });

                var trailer = document.QuerySelector("#desktop_trailer")?.GetAttribute("data-fullyturl") ?? "";
                var review = document.QuerySelector("#desktop_review")?.GetAttribute("data-fullyturl") ?? "";

                var items = new List<Item>();
                if (!string.IsNullOrWhiteSpace(trailer))
                    items.Add(
                        new Item()
                        {
                            Id = Guid.NewGuid(),
                            Value = trailer,
                            ItemType = ItemType.Trailer,
                            MovieId = movie.Id,
                            SourceType = SourceType.Multiplex,

                        }
                    );
                if (!string.IsNullOrWhiteSpace(review))
                    items.Add(
                        new Item()
                        {
                            Id = Guid.NewGuid(),
                            Value = review,
                            ItemType = ItemType.Review,
                            MovieId = movie.Id,
                            SourceType = SourceType.Multiplex,
                        }
                    );
                items.Add(new Item()
                {
                    Id = Guid.NewGuid(),
                    Value = href,
                    ItemType = ItemType.Link,
                    MovieId = movie.Id,
                    SourceType = SourceType.Multiplex,
                });
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