using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyMovies.BLL.BllModels;
using MyMovies.Domain.Entities;

namespace MyMovies.BLL.Interfaces
{
    public interface IMoviesService
    {
        Task<Movie> GetMovieAsync(Guid movieId);
        Task<ICollection<Movie>> GetMoviesAsync(IEnumerable<Guid> ids);
        Task<ICollection<Movie>> GetMoviesAsync(bool getFullInfo = false);
        IQueryable<Movie> GetMovies(Pagination pagination);
        Task SetMovieRate(Guid id, decimal rate);
        Task SaveMovieAsync(Movie movie);
        Task DeleteMovieAsync(Guid movieId);
        Task<IEnumerable<Movie>> GetIncorrectMoviesAsync();
        Task DeleteIncorrectMovies();
        Task<IEnumerable<Description>> GetMovieDescriptions(Guid movieId);
        int TotalCount();
    }
}