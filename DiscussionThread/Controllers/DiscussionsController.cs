using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscussionThread.Data;
using DiscussionThread.Models;

namespace DiscussionThread.Controllers
{
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

            var discussion = await _context.Discussions.FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null) return NotFound();

            return View(discussion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discussion discussion)
        {
            // Handle image upload if provided
            if (discussion.ImageFile != null && discussion.ImageFile.Length > 0)
            {
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadDir);

                string fileName = Guid.NewGuid() + Path.GetExtension(discussion.ImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(stream);
                }

                discussion.ImageFilename = fileName;
            }

            // Set creation date and save the discussion
            discussion.CreateDate = DateTime.Now;
            _context.Add(discussion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null) return NotFound();

            return View(discussion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Discussion discussion)
        {
            if (id != discussion.DiscussionId) return NotFound();

            try
            {
                // Handle image upload if provided
                if (discussion.ImageFile != null && discussion.ImageFile.Length > 0)
                {
                    string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadDir);

                    if (!string.IsNullOrEmpty(discussion.ImageFilename))
                    {
                        string oldFilePath = Path.Combine(uploadDir, discussion.ImageFilename);
                        if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
                    }

                    string fileName = Guid.NewGuid() + Path.GetExtension(discussion.ImageFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await discussion.ImageFile.CopyToAsync(stream);
                    }

                    discussion.ImageFilename = fileName;
                }

                // Update the discussion
                _context.Update(discussion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscussionExists(discussion.DiscussionId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions.FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null) return NotFound();

            return View(discussion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion != null)
            {
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Delete the image if it exists
                if (!string.IsNullOrEmpty(discussion.ImageFilename))
                {
                    string filePath = Path.Combine(uploadDir, discussion.ImageFilename);
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }

                // Remove the discussion
                _context.Discussions.Remove(discussion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.DiscussionId == id);
        }
    }
}
