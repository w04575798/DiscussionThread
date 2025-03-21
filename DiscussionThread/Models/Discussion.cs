﻿using System;
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

        public string? ImageFilename { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public List<Comment> Comments { get; set; } = new List<Comment>();

        // Foreign key to ApplicationUser (logged-in user)
        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
