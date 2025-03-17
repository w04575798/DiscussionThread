using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
using DiscussionThread.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DiscussionThread.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewBag.DiscussionId = discussionId;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content, DiscussionId")] Comment comment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if user is not logged in
            }

            // Assign user information and other properties
            comment.ApplicationUserId = user.Id;
            comment.CreateDate = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Redirect to GetDiscussion action in HomeController
            return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
        }
    }
}
