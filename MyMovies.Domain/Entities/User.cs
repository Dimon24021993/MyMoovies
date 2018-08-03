using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyMovies.Domain.Enums;

namespace MyMovies.Domain.Entities
{
    public class User
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        [Required]
        public Language Language { get; set; }

        public virtual List<User> Friends { get; set; }
    }
}
