using System;
using System.Collections.Generic;
using System.ComponentModel;
using MyMovies.Domain.Enums;

namespace MyMovies.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [DefaultValue(Language.Uk)]
        public Language Language { get; set; }

        public virtual List<User> Friends { get; set; }
    }
}
