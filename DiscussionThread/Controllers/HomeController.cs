using System.Diagnostics;
using DiscussionThread.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DiscussionThread.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve discussions along with associated comments and user
            var discussions = _context.Discussions
                                      .Include(d => d.Comments)
                                      .ThenInclude(c => c.ApplicationUser)  // Include ApplicationUser for each comment
                                      .Include(d => d.ApplicationUser)  // Include the ApplicationUser for the discussion
                                      .OrderByDescending(d => d.CreateDate)
                                      .ToList();

            return View(discussions);
        }

        public IActionResult GetDiscussion(int id)
        {
            var discussion = _context.Discussions
                                     .Include(d => d.Comments)
                                     .ThenInclude(c => c.ApplicationUser)  // Ensure each comment has its ApplicationUser loaded
                                     .Include(d => d.ApplicationUser)  // Ensure the discussion has its ApplicationUser loaded
                                     .FirstOrDefault(d => d.DiscussionId == id);

            // Check if the discussion was found
            if (discussion == null)
            {
                return NotFound();  // Return 404 if the discussion is not found
            }

            // Return the view with the discussion and its comments
            return View(new DiscussionViewModel
            {
                Discussion = discussion,
                Comments = discussion.Comments?.ToList() ?? new List<Comment>()  // Ensure Comments is not null
            });
        }

        public IActionResult Profile(string id)
        {
            var user = _context.Users
                               .Include(u => u.Discussions)  // Include related discussions
                               .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();  // Return 404 if the user is not found
            }

            var viewModel = new ProfileViewModel
            {
                User = user,
                Discussions = user.Discussions.ToList()  // Convert ICollection to List
            };

            return View("~/Views/Profile/Index.cshtml", viewModel);  // Custom view for profile
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
