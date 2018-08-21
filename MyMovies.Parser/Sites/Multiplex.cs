using MyMovies.Domain.Entities;
using MyMovies.Domain.Enums;
using MyMovies.Parser.DataBase;
using System;
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
            Console.ReadKey();

        }

        private static void ParseMultiplexMovie(ref HttpClient client, string x)
        {
            try
            {
                var document = client.GetDocument(x);
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

                DbTasks.AddMovieAndDescriptionIntoDb(ref movie, ref Movies, new Description()
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    Language = Language.Ru,
                    MovieName = movieName,
                    UserId = user.Id
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
            }
            catch (Exception e)
            {
                // ignored
            }
        }


    }
}