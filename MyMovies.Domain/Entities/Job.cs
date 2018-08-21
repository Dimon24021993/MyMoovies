using MyMovies.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Job : Entity
    {
        public JobType JobType { get; set; }

        public Guid MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        public Guid PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }

    }
}