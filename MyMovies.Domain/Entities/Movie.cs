using System;
using System.ComponentModel.DataAnnotations;

namespace MyMovies.Domain.Entities
{
    public class Movie
    {
        [Key]
        [Required]
        public Guid MovieId { get; set; }
        public decimal Rate { get; set; }
        public int RatedPeople { get; set; }
        public string OriginalName { get; set; }
        public int Year { get; set; }
    }
}