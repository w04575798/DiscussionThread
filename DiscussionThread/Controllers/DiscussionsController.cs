using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
using DiscussionThread.Models;

namespace DiscussionThread.Controllers
{
    [Authorize]
    public class DiscussionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscussionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Discussions.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null) return NotFound();

            var viewModel = new DiscussionViewModel
            {
                Discussion = discussion,
                Comments = discussion.Comments.ToList()
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discussion discussion)
        {
            // Assuming model is valid, no need to check ModelState.IsValid

            discussion.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            discussion.CreateDate = DateTime.Now;

            if (discussion.ImageFile != null && discussion.ImageFile.Length > 0)
            {
                string uploadDir = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadDir);

                string fileName = Guid.NewGuid() + Path.GetExtension(discussion.ImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(stream);
                }

                discussion.ImageFilename = fileName;
            }

            _context.Add(discussion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null || discussion.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return View(discussion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Discussion discussion)
        {
            if (id != discussion.DiscussionId) return NotFound();

            var existingDiscussion = await _context.Discussions.FindAsync(id);
            if (existingDiscussion == null || existingDiscussion.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            existingDiscussion.Title = discussion.Title;
            existingDiscussion.Content = discussion.Content;

            if (discussion.ImageFile != null && discussion.ImageFile.Length > 0)
            {
                string uploadDir = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadDir);

                if (!string.IsNullOrEmpty(existingDiscussion.ImageFilename))
                {
                    string oldFilePath = Path.Combine(uploadDir, existingDiscussion.ImageFilename);
                    if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(discussion.ImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(stream);
                }

                existingDiscussion.ImageFilename = fileName;
            }

            _context.Update(existingDiscussion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null || discussion.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return View(discussion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null || discussion.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            if (!string.IsNullOrEmpty(discussion.ImageFilename))
            {
                string filePath = Path.Combine("wwwroot", "uploads", discussion.ImageFilename);
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            }

            _context.Discussions.Remove(discussion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.DiscussionId == id);
        }
    }
}
