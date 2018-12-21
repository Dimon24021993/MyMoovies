﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyMovies.DAL;

namespace MyMovies.DAL.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyMovies.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<Guid>("MovieId");

                    b.Property<string>("Text");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Description", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DescriptionText");

                    b.Property<int>("Language");

                    b.Property<Guid>("MovieId");

                    b.Property<string>("MovieName");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("UserId");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("JobType");

                    b.Property<Guid>("MovieId");

                    b.Property<Guid>("PersonId");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("PersonId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country");

                    b.Property<DateTime>("Date");

                    b.Property<TimeSpan>("Duration");

                    b.Property<string>("OriginalName");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(12,10)");

                    b.Property<int>("RatedPeople");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<int>("Gender");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Height");

                    b.Property<string>("Href");

                    b.Property<Guid>("MovieId");

                    b.Property<string>("Name");

                    b.Property<int>("Sort");

                    b.Property<int>("Type");

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessLevel");

                    b.Property<int>("Language");

                    b.Property<Guid>("MovieId");

                    b.Property<string>("TagText");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<int>("Language");

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Comment", b =>
                {
                    b.HasOne("MyMovies.Domain.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyMovies.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Description", b =>
                {
                    b.HasOne("MyMovies.Domain.Entities.Movie")
                        .WithMany("Descriptions")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyMovies.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Job", b =>
                {
                    b.HasOne("MyMovies.Domain.Entities.Movie", "Movie")
                        .WithMany("Jobs")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyMovies.Domain.Entities.Person", "Person")
                        .WithMany("Jobs")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Picture", b =>
                {
                    b.HasOne("MyMovies.Domain.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Tag", b =>
                {
                    b.HasOne("MyMovies.Domain.Entities.Movie")
                        .WithMany("Tags")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
