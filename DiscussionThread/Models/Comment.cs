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

        // Foreign key to Discussion
        [Required]
        public int DiscussionId { get; set; }

        [ForeignKey("DiscussionId")]
        public Discussion Discussion { get; set; }

        // Foreign key to ApplicationUser (logged-in user)
        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
