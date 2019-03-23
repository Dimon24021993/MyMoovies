using MyMovies.Domain.Entities;
using System.Collections.Generic;

namespace MyMovies.BLL.BllModels
{
    public class Pagination
    {
        public bool Desc { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Pages { get; set; }
        public IEnumerable<Entity> Entities { get; set; }
    }
}