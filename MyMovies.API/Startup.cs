using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyMovies.API.Config;
using MyMovies.BLL.Interfaces;
using MyMovies.BLL.Services;
using MyMovies.DAL;
using Newtonsoft.Json;

namespace MyMovies.API
{
    public class Startup//test
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)

                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
#if DEBUG
                    options.RequireHttpsMetadata = false;
#endif
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = false,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            //services.AddTransient<IAccountService, AccountService>();
            //services.AddTransient<IUserService, UserService>();

            services.AddTransient<IMoviesService, MoviesService>();
            services.AddTransient<IUserService, UserService>();

            services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DataBase")));

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "MyMovies API", Version = "v1" });
            //    // c.AddSecurityDefinition("token", new BasicAuthScheme());
            //});
            services.AddSwaggerDocument(settings =>
            {
                settings.DocumentName = "v1";
                settings.Title = "MyMovies API";

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger(settings =>
            {
                settings.DocumentName = "v1";
                settings.Path = "/swagger/{documentName}/swagger.json";

            });
            app.UseSwaggerUi3();

            app.UseRewriter(new RewriteOptions()
                .AddRedirectToHttps()
                .AddRedirectToHttpsPermanent()
                .AddRedirect("^/?$", "/swagger"));

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader()
                .WithOrigins(new[] {
#if true
                    
                    "localhost", 
#endif
                    "http://api.mymoovies.ga",
                    "https://api.mymoovies.ga", 
                    "http://mymoovies.ga",
                    "https://mymoovies.ga"
                })
                .AllowCredentials().Build());

            app.UseAuthentication();
            
            
            app.UseAuthorization();

            app.UseEndpoints(builder =>
            {
                builder.MapDefaultControllerRoute();
                builder.MapRazorPages();
            });
        }
    }
}
