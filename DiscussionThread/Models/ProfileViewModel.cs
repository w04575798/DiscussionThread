using System.Collections.Generic;
namespace DiscussionThread.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Discussion> Discussions { get; set; }

        // Computed property for profile picture path
        public string ProfilePicturePath => string.IsNullOrEmpty(User.ImageFilename) 
            ? "/images/default.png"  // Default profile picture
            : $"/uploads/{User.ImageFilename}"; // Path to uploaded image
    }
}
