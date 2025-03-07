using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DiscussionThread.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? ImageFilename { get; set; } // Stores the filename

        [NotMapped] // Prevents this property from being stored in the database
        public IFormFile ImageFile { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Navigation Property
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
