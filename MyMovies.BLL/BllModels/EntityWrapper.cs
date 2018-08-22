using MyMovies.Domain.Entities;
using MyMovies.Domain.Enums;

namespace MyMovies.BLL.BllModels
{
    public class EntityWrapper
    {
        public Entity EntityObject { get; set; }

        public CrudOperation Operation { get; set; }
    }
}
