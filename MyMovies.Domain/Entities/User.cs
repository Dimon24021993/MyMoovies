﻿using MyMovies.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMovies.Domain.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        [Required]
        public Language Language { get; set; }
        public virtual List<Role> Roles { get; set; }
        // public virtual List<User> Friends { get; set; }
    }
}
