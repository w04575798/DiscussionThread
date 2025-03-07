using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionThread.Models

{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Foreign Key
        public int DiscussionId { get; set; }

        // Navigation Property
        [ForeignKey("DiscussionId")]
        public Discussion Discussion { get; set; }
    }
}
