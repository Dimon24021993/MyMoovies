using System;
using System.ComponentModel.DataAnnotations;

namespace MyMovies.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}