using MyMovies.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace MyMovies.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserAsync(string login, string password);

    }
}