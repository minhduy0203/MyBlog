using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Content { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; }
    }
}
