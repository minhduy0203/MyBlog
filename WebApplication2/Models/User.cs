using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? RegisterAt { get; set; }
        public string? Intro { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
