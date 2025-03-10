using System.Diagnostics;
using DiscussionThread.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;

namespace DiscussionThread.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        // Inject ApplicationDbContext through constructor
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Index action to display discussions in descending order
        public IActionResult Index()
        {
            var discussions = _context.Discussions
                                      .Include(d => d.Comments)  // Include comments
                                      .OrderByDescending(d => d.CreateDate)  // Order discussions by creation date
                                      .ToList();

            return View(discussions);
        }

        // Action to show privacy page (if needed)
        public IActionResult Privacy()
        {
            return View();
        }

        // Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Action to view a specific discussion (with comments)
        public IActionResult GetDiscussion(int id)
        {
            var discussion = _context.Discussions
                                     .Include(d => d.Comments)
                                     .FirstOrDefault(d => d.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            var viewModel = new DiscussionViewModel
            {
                Discussion = discussion,
                Comments = discussion.Comments.ToList()
            };

            return View(viewModel);
        }
    }
}
