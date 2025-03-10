using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
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

        // Index action to display discussions in descending order
        public IActionResult Index()
        {
            var discussions = _context.Discussions
                                      .Include(d => d.Comments)  // Include comments
                                      .OrderByDescending(d => d.CreateDate)  // Order discussions by creation date
                                      .ToList();

            return View(discussions);
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
