using Microsoft.EntityFrameworkCore;
using MyMovies.Domain.Entities;

namespace MyMovies.DAL
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>().Property(x => x.Value).HasColumnType("decimal(12,10)");
            modelBuilder.Entity<Role>().HasKey(x => new {x.UserId, x.RoleName});
        }
    }
}