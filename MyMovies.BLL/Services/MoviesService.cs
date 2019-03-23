using Microsoft.EntityFrameworkCore;
using MyMovies.BLL.BllModels;
using MyMovies.BLL.Interfaces;
using MyMovies.DAL;
using MyMovies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMovies.BLL.Services
{
    public sealed class MoviesService : EntitiesService, IMoviesService
    {
        public MoviesService(DataBaseContext context) : base(context)
        {

        }

        public async Task<Movie> GetMovieAsync(Guid movieId, bool getFullInfo = false)
        {
            var Movie = await GetByIdAsync(
                movieId,
                new List<Expression<Func<Movie, object>>>
                {
                    x => x.Descriptions,
                    x => x.Tags,
                    x=>x.Items,
                    x=>x.Pictures,
                    x=>x.Jobs,
                    x=>x.Rates
                });

            if (getFullInfo && Movie != null)
            {
                Movie.Jobs = (await GetListAsync(
                    new List<Expression<Func<Job, bool>>>
                    {
                        x => x.MovieId == Movie.Id
                    }, new List<Expression<Func<Job, object>>>
                    {
                        x=>x.Person
                    })).ToList();
            }

            return Movie;
        }

        public async Task<ICollection<Movie>> GetMoviesAsync(IEnumerable<Guid> ids)
        {
            return await GetListAsync(
                new List<Expression<Func<Movie, bool>>>
                {
                    x => ids.Any(id => id == x.Id)
                }, new List<Expression<Func<Movie, object>>>
                {
                    x => x.Descriptions,
                    x=>x.Jobs,
                    x=>x.Tags
                });
        }

        public async Task<ICollection<Movie>> GetMoviesAsync(bool getFullInfo = false)
        {
            var Movies = (await GetListAsync(
                null,
                getFullInfo
                    ? new List<Expression<Func<Movie, object>>>
                    {
                        x => x.Descriptions,
                        x => x.Tags,
                        x => x.Jobs
                    }
                    : null)).ToList();

            if (getFullInfo && Movies != null && Movies.Any())
            {
                foreach (var Movie in Movies)
                {
                    Movie.Jobs = (await GetListAsync(
                        new List<Expression<Func<Job, bool>>>
                        {
                            x => x.MovieId == Movie.Id
                        }, new List<Expression<Func<Job, object>>>
                        {
                            x=>x.Person
                        })).ToList();
                }
            }

            return Movies;
        }

        public async Task<IEnumerable<Movie>> GetMovies(Pagination pagination)
        {
            return await context.Movies
                   .Include(x => x.Items)
                   .Include(x => x.Descriptions)
                   .Include(x => x.Pictures)
                   .Include(x => x.Rates)
                   .Include(x => x.Tags)
                   .Include(x => x.Jobs)
                                .ThenInclude(x => x.Person).Skip((pagination.Page - 1) * pagination.Size)
                                .Take(pagination.Size).ToListAsync();
        }

        //public async Task<MovieFilterResult> GetPaggedAsync(MovieFilterBindingModel model)
        //{
        //    var skipAmount = model.PageSize * (model.Page - 1);

        //    var projection = (await GetListAsync(
        //        new List<Expression<Func<Movie, bool>>>
        //        {
        //            x => x.DateFabrication.Year >= model.YearFrom && x.DateFabrication.Year <= model.YearUntil,
        //            x => x.Price >= model.PriceFrom && x.Price <= model.PriceUntil,
        //            x => x.Mileage >= model.MilleageFrom && x.Mileage <= model.MilleageUntil
        //        },
        //        new List<Expression<Func<Movie, object>>>
        //        {
        //            x => x.MovieOwner
        //        })).OrderByDescending(x => x.CreationTime).ToList();


        //    if (!model.MovieMake.IsNullOrWhiteSpace())
        //    {
        //        projection = projection.Where(x => x.MovieMake == model.MovieMake).ToList();
        //    }

        //    if (model.TransmissionType != TransmissionType.Null)
        //    {
        //        projection = projection.Where(x => x.TransmissionType == model.TransmissionType).ToList();
        //    }

        //    if (model.FuelType != FuelType.Null)
        //    {
        //        projection = projection.Where(x => x.FuelType == model.FuelType).ToList();
        //    }

        //    if (model.EngineVolume != null)
        //    {
        //        projection = projection.Where(x => x.EngineVolume == model.EngineVolume).ToList();
        //    }

        //    if (!model.FirstName.IsNullOrWhiteSpace() && !model.LastName.IsNullOrWhiteSpace())
        //    {
        //        projection = projection
        //            .Where(x => x.MovieOwner.FirstName.Contains(model.FirstName) &&
        //                        x.MovieOwner.LastName.Contains(model.LastName))
        //            .ToList();
        //    }

        //    var totalNumberOfRecords = projection.Count;

        //    var mod = totalNumberOfRecords % model.PageSize;
        //    var totalPageCount = totalNumberOfRecords / model.PageSize + (mod == 0 ? 0 : 1);

        //    var paggedResult = projection
        //        .Skip(skipAmount)
        //        .Take(model.PageSize).ToList();

        //    var result = new MovieFilterResult
        //    {
        //        Filter = model,
        //        PageSize = paggedResult.Count,
        //        PageNumber = model.Page,
        //        TotalNumberOfPages = totalPageCount,
        //        TotalNumberOfRecords = totalNumberOfRecords,
        //        Result = paggedResult
        //    };

        //    return result;
        //}

        //public async Task<MovieFilterResult> GetPaggedMoviesAsync(MoviePaggedModel model)
        //{
        //    var skipAmount = model.PageSize * (model.Page - 1);

        //    var projection = (await GetListAsync(
        //        new List<Expression<Func<Movie, bool>>>
        //        {
        //            x => x.DateFabrication.Year >= model.YearFrom && x.DateFabrication.Year <= model.YearUntil,
        //            x => x.Price >= model.PriceFrom && x.Price <= model.PriceUntil,
        //            x => x.Mileage >= model.MilleageFrom && x.Mileage <= model.MilleageUntil
        //        },
        //        new List<Expression<Func<Movie, object>>>
        //        {
        //            x => x.MovieOwner
        //        })).ToList();


        //    if (!model.MovieMake.IsNullOrWhiteSpace())
        //    {
        //        projection = projection.Where(x => x.MovieMake == model.MovieMake).ToList();
        //    }

        //    if (model.TransmissionType != TransmissionType.Null)
        //    {
        //        projection = projection.Where(x => x.TransmissionType == model.TransmissionType).ToList();
        //    }

        //    if (model.FuelType != FuelType.Null)
        //    {
        //        projection = projection.Where(x => x.FuelType == model.FuelType).ToList();
        //    }

        //    if (model.EngineVolume != null)
        //    {
        //        projection = projection.Where(x => x.EngineVolume == model.EngineVolume).ToList();
        //    }

        //    if (!model.FirstName.IsNullOrWhiteSpace() && !model.LastName.IsNullOrWhiteSpace())
        //    {
        //        projection = projection
        //            .Where(x => x.MovieOwner.FirstName.Contains(model.FirstName) &&
        //                        x.MovieOwner.LastName.Contains(model.LastName))
        //            .ToList();
        //    }

        //    switch (model.OrderBy)
        //    {
        //        case "0":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.NumericId).ToList() : projection.OrderByDescending(x => x.NumericId).ToList();
        //                break;
        //            }
        //        case "1":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.DateFabrication).ToList() : projection.OrderByDescending(x => x.DateFabrication).ToList();
        //                break;
        //            }
        //        case "2":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.MovieMake).ToList() : projection.OrderByDescending(x => x.MovieMake).ToList();
        //                break;
        //            }
        //        case "3":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.MovieModel).ToList() : projection.OrderByDescending(x => x.MovieModel).ToList();
        //                break;
        //            }
        //        case "4":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.MovieOwner.LastName).ToList() : projection.OrderByDescending(x => x.MovieOwner.LastName).ToList();
        //                break;
        //            }
        //        case "5":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.DateFabrication.Year).ToList() : projection.OrderByDescending(x => x.DateFabrication.Year).ToList();
        //                break;
        //            }
        //        case "6":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.FuelType).ToList() : projection.OrderByDescending(x => x.FuelType).ToList();
        //                break;
        //            }
        //        case "7":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.Price).ToList() : projection.OrderByDescending(x => x.Price).ToList();
        //                break;
        //            }
        //        case "8":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.EngineVolume).ToList() : projection.OrderByDescending(x => x.EngineVolume).ToList();
        //                break;
        //            }
        //        case "9":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.TransmissionType).ToList() : projection.OrderByDescending(x => x.TransmissionType).ToList();
        //                break;
        //            }
        //        case "10":
        //            {
        //                projection = model.Asc ? projection.OrderBy(x => x.Mileage).ToList() : projection.OrderByDescending(x => x.Mileage).ToList();
        //                break;
        //            }
        //    }

        //    var totalNumberOfRecords = projection.Count;

        //    var mod = totalNumberOfRecords % model.PageSize;
        //    var totalPageCount = totalNumberOfRecords / model.PageSize + (mod == 0 ? 0 : 1);

        //    var paggedResult = projection
        //        .Skip(skipAmount)
        //        .Take(model.PageSize).ToList();

        //    var result = new MovieFilterResult
        //    {
        //        Filter = model,
        //        PageSize = paggedResult.Count,
        //        PageNumber = model.Page,
        //        TotalNumberOfPages = totalPageCount,
        //        TotalNumberOfRecords = totalNumberOfRecords,
        //        Result = paggedResult
        //    };

        //    return result;
        //}

        public async Task SetMovieRate(Guid id, decimal rate)
        {
            return;
            //var Movie = await GetByIdAsync<Movie>(id);
            //Movie.Rate = rate;
            //Movie.RatedPeople++;
            //await Save(Movie);
        }



        //public async Task<Movie> GetLotMovieAsync(Guid lotId, bool getFullInfo = false)
        //{
        //    var Movie = await GetSingleAsync(
        //        new List<Expression<Func<Movie, bool>>>
        //        {
        //            x => x.Id!=null && x.Id == lotId
        //        },
        //        getFullInfo
        //            ? new List<Expression<Func<Movie, object>>>
        //            {
        //                x => x.MovieReport,
        //                x => x.MovieOwner,
        //                x => x.CalendarEvent,
        //                x => x.Lot
        //            }
        //            : null);

        //    if (getFullInfo && Movie != null)
        //    {
        //        Movie.MovieImages = await GetListAsync(
        //            new List<Expression<Func<MovieImage, bool>>>
        //            {
        //                x => x.MovieId == Movie.Id
        //            });
        //    }

        //    return Movie;
        //}

        //public async Task<Guid> CreateMovieAsync(CreateMovieModel model, bool isNotFound = false)
        //{
        //    string MovieMark;
        //    string MovieModel;
        //    string price = "0";

        //    using (var parserManager = new MoviesParserManager())
        //    {
        //        MovieMark = await parserManager.GetMovieMarkById(model.MovieMakeId);
        //        MovieModel = await parserManager.GetMovieModelById(model.MovieModelId);

        //        if (!isNotFound)
        //        {
        //            try
        //            {
        //                price = await parserManager.GetMoviePriceAsync(new MoviePriceModel
        //                {
        //                    Year = model.DateFabrication.Year,
        //                    EngineVolume = model.EngineVolume,
        //                    Mileage = model.Mileage,
        //                    ModelId = model.MovieModelId
        //                });
        //            }
        //            catch (Exception)
        //            {
        //                throw new Exception("Тaкой машины нет в парсере");
        //            }

        //        }
        //    }

        //    User MovieOwner;
        //    using (var userManager = new UserManager())
        //    {
        //        MovieOwner = await userManager.GetUserAsync(model.UserInfoId);
        //    }

        //    var Movie = new Movie
        //    {
        //        Id = Guid.NewGuid(),
        //        MovieMake = MovieMark,
        //        MovieModel = MovieModel,
        //        MovieModelId = model.MovieModelId,
        //        DateFabrication = model.DateFabrication,
        //        FuelType = model.FuelType,
        //        EngineVolume = model.EngineVolume,
        //        TransmissionType = model.TransmissionType,
        //        AirConditionPresent = model.AirConditionPresent,
        //        Mileage = model.Mileage,
        //        MovieOwnerId = MovieOwner.Id,
        //        Price = decimal.Parse(price),
        //        CreationTime = DateTime.Now.ToUniversalTime(),
        //        IsDeleted = false,
        //        NumericId = Randomizer.GetRandomNumber(),
        //        IsMovieNotFound = isNotFound
        //    };

        //    await SaveMovieAsync(Movie);

        //    return Movie.Id;
        //}

        public async Task SaveMovieAsync(Movie movie)
        {
            await Save(movie);
        }

        public async Task DeleteMovieAsync(Guid movieId)
        {
            await Delete<Movie>(movieId);
        }

        //public async Task<bool> DeleteMovieFullAsync(Guid id)
        //{
        //    bool res;
        //    using (var manager = new MovieReportsManager())
        //    {
        //        res = await manager.DeleteMovieReportFullAsync(id);
        //    }

        //    if (res)
        //    {
        //        var list = new List<EntityWrapper>();
        //        var Movie = await GetMovieAsync(id);

        //        using (var manager = new CalendarEventsManager())
        //        {
        //            var calendarEvent = await manager.GetCalendarEventAsync(id);
        //            if (calendarEvent != null)
        //            {
        //                list.Add(new EntityWrapper()
        //                {
        //                    EntityObject = calendarEvent,
        //                    Operation = CrudOperation.Delete
        //                });
        //            }
        //        }

        //        if (Movie != null)
        //        {
        //            using (var manager = new MovieImagesManager())
        //            {
        //                var MovieImages = await manager.GetMovieImagesByMovieIdAsync(Movie.Id);

        //                foreach (var MovieImage in MovieImages)
        //                {
        //                    list.Add(new EntityWrapper()
        //                    {
        //                        EntityObject = MovieImage,
        //                        Operation = CrudOperation.Delete
        //                    });
        //                }
        //            }


        //            list.Add(new EntityWrapper()
        //            {
        //                EntityObject = Movie,
        //                Operation = CrudOperation.Delete
        //            });

        //            using (var manager = new NotFoundMoviesManager())
        //            {
        //                var notFoundMovie = await manager.GetNotFoundMovieByMovieId(Movie.Id);

        //                if (notFoundMovie != null)
        //                {
        //                    list.Add(new EntityWrapper()
        //                    {
        //                        EntityObject = notFoundMovie,
        //                        Operation = CrudOperation.Delete
        //                    });
        //                }

        //            }
        //        }

        //        await ChangeEntities(list);
        //    }
        //    return (res && (await GetMovieAsync(id)) == null);
        //}

        // Get Movies without CalendarEvents
        public async Task<IEnumerable<Movie>> GetIncorrectMoviesAsync()
        {
            return await GetListAsync(new List<Expression<Func<Movie, bool>>>
            {
                x => x.Descriptions == null
            }, new List<Expression<Func<Movie, object>>>
            {
                x => x.Descriptions
            });


        }

        public async Task DeleteIncorrectMovies()
        {
            var movies = await GetIncorrectMoviesAsync();

            foreach (var movie in movies)
            {
                await DeleteMovieAsync(movie.Id);
            }
        }

        public async Task<IEnumerable<Description>> GetMovieDescriptions(Guid movieId)
        {

            return (await GetByIdAsync(movieId,
              new List<Expression<Func<Movie, object>>>
             {
                x => x.Descriptions
             })).Descriptions;
        }

        public int TotalCount()
        {
            return context.Movies.Count();
        }
    }
}