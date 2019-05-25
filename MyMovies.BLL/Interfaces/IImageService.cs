using System.Threading.Tasks;

namespace MyMovies.BLL.Interfaces
{
    public interface IImageService
    {
        Task UploadImage(string path);
    }
}