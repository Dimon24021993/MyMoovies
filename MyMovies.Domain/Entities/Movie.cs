using System;
using System.Collections.Generic;

namespace MyMovies.Domain.Entities
{
    public class Movie : Entity
    {
        public int RatedPeople { get; set; }
        public string OriginalName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string Country { get; set; }
        public virtual List<Job> Jobs { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual List<Description> Descriptions { get; set; }
        public virtual List<Rate> Rates { get; set; }
    }
}