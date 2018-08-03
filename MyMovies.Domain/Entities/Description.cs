using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyMovies.Domain.Enums;

namespace MyMovies.Domain.Entities
{
    public class Description
    {
        [Key]
        public Guid DescriptionId { get; set; }
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public Language Language { get; set; }
        public string DescriptionText { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
    }
}
