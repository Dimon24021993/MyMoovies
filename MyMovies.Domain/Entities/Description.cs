using MyMovies.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Description : Entity
    {
        [ForeignKey("MovieId")]
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public Language Language { get; set; }
        public string DescriptionText { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
