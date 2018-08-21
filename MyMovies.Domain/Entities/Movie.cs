using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Movie : Entity
    {
        public decimal Rate { get; set; }
        public int RatedPeople { get; set; }
        public string OriginalName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string Country { get; set; }
        public virtual List<Job> Jobs { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual List<Description> Descriptions { get; set; }
    }
}