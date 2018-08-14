using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Person
    {
        [Key]
        [Required]
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public bool Male { get; set; }
        public virtual List<Job> Jobs { get; set; }
        [NotMapped]
        public virtual List<string> Photos { get; set; }
    }
}