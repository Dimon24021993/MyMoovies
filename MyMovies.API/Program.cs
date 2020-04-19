using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;

//[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace MyMovies.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).UseNLog()
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
        }
    }
}
