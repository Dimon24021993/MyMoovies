using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyMovies.Domain.Enums;

namespace MyMovies.Domain.Entities
{
    public class Job
    {
        [Key]
        public Guid JobId { get; set; }
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
        [ForeignKey("Person")]
        public Guid PersonId { get; set; }
        public JobType JobType { get; set; }

    }
}