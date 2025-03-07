using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
using DiscussionThread.Models;

namespace DiscussionThread.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewBag.DiscussionId = discussionId; // Pass discussion ID to the view
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content, DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Set the current date/time for the comment creation
                comment.CreateDate = DateTime.Now;

                // Add the comment to the database
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect to the discussion details after successfully adding the comment
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }

            // If validation fails, return the same view with the current data
            ViewBag.DiscussionId = comment.DiscussionId;
            return View(comment);
        }
    }
}
