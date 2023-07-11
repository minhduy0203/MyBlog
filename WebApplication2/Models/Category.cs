using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class Category
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Category1 { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; }
    }
}
