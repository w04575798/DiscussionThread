using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionThread.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Custom properties for  ApplicationUser
        public string Name { get; set; }
        public string? Location { get; set; } = "Unknown";
        public string ImageFilename { get; set; } = "default.png";

        // Navigation property for related discussions
        public virtual ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
    }
}