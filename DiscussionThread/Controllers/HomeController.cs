using System.Diagnostics;
using DiscussionThread.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
using Microsoft.Extensions.Logging;
using System.Linq;
using DiscussionThread.Models;


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
                                      .Include(d => d.ApplicationUser)  // Include the ApplicationUser
                                      .OrderByDescending(d => d.CreateDate)
                                      .ToList();

            return View(discussions);
        }

        public IActionResult GetDiscussion(int id)
        {
            var discussion = _context.Discussions
                                     .Include(d => d.Comments)
                                     .Include(d => d.ApplicationUser)  // Include the ApplicationUser
                                     .FirstOrDefault(d => d.DiscussionId == id);

            return discussion == null
                ? NotFound()
                : View(new DiscussionViewModel { Discussion = discussion, Comments = discussion.Comments.ToList() });
        }

        public IActionResult Profile(string id)
        {
            var user = _context.Users
                               .Include(u => u.Discussions)  // Include related discussions
                               .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
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
