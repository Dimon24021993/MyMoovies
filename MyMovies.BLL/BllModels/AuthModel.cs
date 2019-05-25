using System.ComponentModel.DataAnnotations;

namespace MyMovies.BLL.BllModels
{
    public class AuthModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}