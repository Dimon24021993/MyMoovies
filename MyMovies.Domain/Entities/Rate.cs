using MyMovies.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Rate : Entity
    {
        public RateType RateType { get; set; }
        public decimal Value { get; set; }

        public Guid MovieId { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}