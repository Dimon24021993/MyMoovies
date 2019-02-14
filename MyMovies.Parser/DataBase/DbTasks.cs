using Microsoft.EntityFrameworkCore;
using MyMovies.DAL;
using MyMovies.Domain.Entities;
using MyMovies.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MyMovies.Parser.DataBase
{
    public static class DbTasks
    {
        public static DataBaseContext Context { get; set; }

        public static void AddMovieAndDescriptionIntoDb(ref Movie movie, ref int moviesCount, Description description)
        {
            var originalName = movie.OriginalName;
            var originalYear = movie.Date.Year;
            lock (Context)
            {
                var movieBase = Context.Movies.Include(x => x.Rates).FirstOrDefault(x => x.OriginalName == originalName && x.Date.Year == originalYear);
                if (movieBase == null)
                {
                    var descriptionBase = Context.Descriptions.FirstOrDefault(x => x.Language == description.Language && x.MovieName == description.MovieName);
                    if (descriptionBase != null)
                    {
                        movie = Context.Movies.FirstOrDefault(m => m.Id == descriptionBase.MovieId);
                        return;
                    }
                    Context.Movies.Add(movie);
                    Context.Descriptions.Add(description);
                    Context.Rates.AddRange(movie.Rates);
                    Context.SaveChanges();
                    Interlocked.Increment(ref moviesCount);
                }
                else
                {
                    #region Rate

                    var newRates = movieBase.Rates.Join(movie.Rates, x => new { x.UserId, x.RateType }, y => new { y.UserId, y.RateType },
                        (x, y) =>
                        {
                            if (y.Value > 0.0M) x.Value = y.Value;
                            return x;
                        });

                    if (newRates.Any())
                    {
                        foreach (var rate in newRates)
                        {
                            Context.Rates.Update(rate);
                            Context.SaveChanges();
                        }
                    }
                    else
                    {
                        foreach (var rate in movie.Rates)
                        {
                            rate.MovieId = movieBase.Id;
                        }
                        Context.Rates.AddRange(movie.Rates);
                    }


                    #endregion

                    var descriptionBase = Context.Descriptions.FirstOrDefault(x => x.Language == description.Language && x.MovieName == description.MovieName);
                    if (descriptionBase == null)
                    {
                        description.MovieId = movieBase.Id;
                        Context.Descriptions.Add(description);
                        Context.SaveChanges();
                    }
                    else if (string.IsNullOrWhiteSpace(descriptionBase.DescriptionText) && !string.IsNullOrWhiteSpace(description.DescriptionText))
                    {
                        descriptionBase.DescriptionText = description.DescriptionText;
                        Context.Descriptions.Update(descriptionBase);
                        Context.SaveChanges();
                    }
                    else if (descriptionBase.DescriptionText == description.DescriptionText)
                    {
                        //do nothin
                    }
                    else
                    {
                        description.MovieId = movieBase.Id;
                        Context.Descriptions.Add(description);
                        Context.SaveChanges();
                    }

                    movie = movieBase;
                }
            }
        }

        public static void AddUsersIntoDb(ref User user)
        {
            var login = user.Login;
            lock (Context)
            {
                var userBase = Context.Users.FirstOrDefault(u => u.Login == login);
                if (userBase == null)
                {

                    Context.Users.Add(user);
                    Context.SaveChanges();

                }
                else
                {
                    user = userBase;
                }
            }
        }

        public static void AddPersonsIntoDb(string[] personNames, Guid movieId, JobType jobType)
        {
            if (personNames == null)
            {
                return;
            }

            Person person;
            lock (Context)
            {
                foreach (var name in personNames)
                {

                    person = Context.Persons.FirstOrDefault(p => p.Name == name.Trim());
                    if (person == null)
                    {
                        person = new Person()
                        {
                            Id = Guid.NewGuid(),
                            Name = name.Trim()
                        };
                        Context.Persons.Add(person);
                        Context.SaveChanges();
                    }
                    Context.Jobs.Add(new Job()
                    {
                        MovieId = movieId,
                        PersonId = person.Id,
                        JobType = jobType
                    });
                    Context.SaveChanges();
                }
            }
        }

        public static void AddTagsIntoDb(IEnumerable<string> tagNames, Language language, Guid movieId)
        {
            if (tagNames == null)
            {
                return;
            }

            Tag tag;
            lock (Context)
            {
                foreach (var name in tagNames)
                {

                    tag = Context.Tags.FirstOrDefault(p => p.TagText == name.Trim().ToLower() && p.MovieId == movieId && p.Language == language);
                    if (tag == null)
                    {
                        tag = new Tag()
                        {
                            Id = Guid.NewGuid(),
                            TagText = name.Trim().ToLower(),
                            Language = language,
                            MovieId = movieId,
                            AccessLevel = AccessLevel.Public
                        };
                        Context.Tags.Add(tag);
                        Context.SaveChanges();
                    }
                }
            }
        }
    }
}