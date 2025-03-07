using Microsoft.EntityFrameworkCore;
using DiscussionThread.Models;

namespace DiscussionThread.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets for Discussion and Comment
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
