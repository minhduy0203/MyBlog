using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Categories = new HashSet<Category>();
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }
        public int AuthorId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title can't be empty")]
        public string Title { get; set; } = null!;
        public string? Summary { get; set; }
        public DateTime? CreateAt { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Content can't be empty")]
        public string Content { get; set; } = null!;
        public string? ImagePreview { get; set; }

        public virtual User? Author { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
