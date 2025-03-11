using System.Diagnostics;
using DiscussionThread.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
using Microsoft.Extensions.Logging;

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
            var discussions = _context.Discussions
                                      .Include(d => d.Comments)
                                      .OrderByDescending(d => d.CreateDate)
                                      .ToList();

            return View(discussions);
        }

        public IActionResult GetDiscussion(int id)
        {
            var discussion = _context.Discussions
                                     .Include(d => d.Comments)
                                     .FirstOrDefault(d => d.DiscussionId == id);

            return discussion == null
                ? NotFound()
                : View(new DiscussionViewModel { Discussion = discussion, Comments = discussion.Comments.ToList() });
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
