using MyMovies.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Item : Entity
    {
        public ItemType ItemType { get; set; }
        public SourceType SourceType { get; set; }
        public Guid AddByUserId { get; set; }

        public Guid MovieId { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public Guid PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public string Value { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }


    }
}