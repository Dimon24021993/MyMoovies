using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMovies.Domain.Entities;

namespace MyMovies.DAL
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(IConfiguration configuration) : base()
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Server=109.251.203.58:1433;Database=MyMovies;User Id=sa;Password=saadmin!");
        }
    }
}