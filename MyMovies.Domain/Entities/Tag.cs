using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyMovies.Domain.Enums;

namespace MyMovies.Domain.Entities
{
    public class Tag : Entity
    {
        [Required]
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
        public string TagText { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Language Language { get; set; }

    }
}