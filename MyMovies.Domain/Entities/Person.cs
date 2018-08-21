using MyMovies.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Person : Entity
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public virtual List<Job> Jobs { get; set; }
        [NotMapped]
        public virtual List<string> Photos { get; set; }

    }
}