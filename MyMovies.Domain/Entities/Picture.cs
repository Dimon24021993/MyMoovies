using MyMovies.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Domain.Entities
{
    public class Picture : Entity
    {
        public Guid MovieId { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
        public PictureType Type { get; set; }
        public SourceType SourceType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
        public int Sort { get; set; }


    }
}