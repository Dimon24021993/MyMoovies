using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMovies.Domain.Entities;

namespace MyMovies.BLL.Interfaces
{
    public interface IMoviesService
    {
        Task<Movie> GetMovieAsync(Guid movieId, bool getFullInfo = false);
        Task<ICollection<Movie>> GetMoviesAsync(IEnumerable<Guid> ids);
        Task<ICollection<Movie>> GetMoviesAsync(bool getFullInfo = false);
        Task SetMovieRate(Guid id, decimal rate);
        Task SaveMovieAsync(Movie movie);
        Task DeleteMovieAsync(Guid movieId);
        Task<IEnumerable<Movie>> GetIncorrectMoviesAsync();
        Task DeleteIncorrectMovies();
        Task<IEnumerable<Description>> GetMovieDescriptions(Guid movieId);
    }
}