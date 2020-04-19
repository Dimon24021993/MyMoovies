using MyMovies.Domain.Entities;
using System.Collections.Generic;

namespace MyMovies.BLL.BllModels
{
    public class Pagination
    {
        public bool Desc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 1;
        public int Pages { get; set; } = 1;
        public IEnumerable<Entity> Entities { get; set; }
    }
}