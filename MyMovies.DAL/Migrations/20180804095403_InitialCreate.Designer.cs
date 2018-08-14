﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyMovies.DAL;

namespace MyMovies.DAL.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20180804095403_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyMovies.Domain.Entities.Description", b =>
                {
                    b.Property<Guid>("DescriptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DescriptionText");

                    b.Property<int>("Language");

                    b.Property<Guid>("MovieId");

                    b.Property<string>("MovieName");

                    b.Property<Guid>("UserId");

                    b.HasKey("DescriptionId");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("MovieId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Rate");

                    b.Property<int>("RatedPeople");

                    b.HasKey("MovieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessLevel");

                    b.Property<Guid>("MovieId");

                    b.Property<string>("TagText");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<int>("Language");

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<Guid?>("UserId1");

                    b.Property<string>("UserName");

                    b.HasKey("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyMovies.Domain.Entities.User", b =>
                {
                    b.HasOne("MyMovies.Domain.Entities.User")
                        .WithMany("Friends")
                        .HasForeignKey("UserId1");
                });
#pragma warning restore 612, 618
        }
    }
}
