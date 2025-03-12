using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Areas.Identity.Data;
using DiscussionThread.Models;

namespace DiscussionThread.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Inherit from IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets for your custom models
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
