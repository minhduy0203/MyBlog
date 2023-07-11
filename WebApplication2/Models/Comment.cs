using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseReply = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public int? ReplyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; } = null!;

        public virtual Post Post { get; set; } = null!;
        public virtual Comment? Reply { get; set; }
        public virtual ICollection<Comment> InverseReply { get; set; }
    }
}
