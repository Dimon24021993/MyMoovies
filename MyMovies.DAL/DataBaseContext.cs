using Microsoft.EntityFrameworkCore;
using MyMovies.Domain.Entities;

namespace MyMovies.DAL
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().Property(x => x.Rate).HasColumnType("decimal(12,10)");
        }
    }
}