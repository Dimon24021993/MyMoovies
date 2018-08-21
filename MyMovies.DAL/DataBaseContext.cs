using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMovies.Domain.Entities;

namespace MyMovies.DAL
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataBaseContext : DbContext
    {
        private IConfiguration Configuration { get; set; }
        public DataBaseContext(IConfiguration configuration) : base()
        {
            this.Configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Entity<Description>().
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=MyMovies;User Id=MyMovies;Password=356v7hnCTWyhFa3A;Max Pool Size=2048;Pooling=true;MultipleActiveResultSets=True;");
            optionsBuilder.UseSqlServer(@"Data Source=109.251.203.58;Initial Catalog=MyMovies;User Id=MyMovies;Password=356v7hnCTWyhFa3A;Max Pool Size=2048;Pooling=true;MultipleActiveResultSets=True;");
        }
    }
}