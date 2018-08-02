using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MyMovies.API.Config
{
    public class AuthOptions
    {
        private static IConfigurationSection Config => Startup.Configuration.GetSection("AuthOptions");
        private static string KEY => Config["KEY"];   // ключ для шифрации

        public static string ISSUER = Config["ISSUER"]; // издатель токена
        public static string AUDIENCE => Config["AUDIENCE"]; // потребитель токена
        public static int LIFETIME = int.Parse(Config["LIFETIME"]); // время жизни токена - 30 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}