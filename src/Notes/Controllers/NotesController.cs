using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace Notes.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesController(ApplicationDbContext context, ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<NotesController>();
            _userManager = userManager;
        }

        // GET: Notes
        [Authorize]
        public async Task<IActionResult> Index(Boolean partial = false)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = await _userManager.GetUserIdAsync(user);

            var notes = from m in _context.Note
                        select m;

            if (!String.IsNullOrEmpty(userId))
            {
                notes = notes.Where(s => s.UserId.Equals(userId));
            }

            if (!user.IncludeFinished)
            {
                notes = notes.Where(s => !s.Finished);
            }

            // Sort Notes
            switch (user.SortingField)
            {
                case "FinishDate":
                    notes = user.SortingAscending ? notes.OrderBy(s => s.FinishDate) : notes.OrderByDescending(s => s.FinishDate);
                    break;
                case "CreationDate":
                    notes = user.SortingAscending ? notes.OrderBy(s => s.CreationDate) : notes.OrderByDescending(s => s.CreationDate);
                    break;
                case "PriorityEnum":
                    notes = user.SortingAscending ? notes.OrderBy(s => s.PriorityEnum) : notes.OrderByDescending(s => s.PriorityEnum);
                    break;
            }

            var applicationDbContext = _context.Note.Include(n => n.User);
            ViewData["activeSortField"] = user.SortingField;
            ViewData["includeFinished"] = user.IncludeFinished;

            if (partial)
            {
                return PartialView("_List", await notes.ToListAsync());
            }
            return View(await notes.ToListAsync());
        }

        // Ajax: ToggleHideFinished
        [Authorize]
        public async Task<IActionResult> ToggleHideFinished()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.Equals(null))
            {
                user.IncludeFinished = !user.IncludeFinished;
                await _userManager.UpdateAsync(user);
                _context.SaveChanges();
            }

            return await Index(true);
        }

        // POST Ajax: Set Sorting
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSortSetting(string fieldToSortBy)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.Equals(null) && (fieldToSortBy.Equals("FinishDate") || fieldToSortBy.Equals("CreationDate") || fieldToSortBy.Equals("PriorityEnum")))
            {
                if (user.SortingField.Equals(fieldToSortBy))
                {
                    user.SortingAscending = !user.SortingAscending;
                }
                else
                {
                    user.SortingField = fieldToSortBy;
                    user.SortingAscending = true;
                }
                await _userManager.UpdateAsync(user);
                _context.SaveChanges();
            }

            return await Index(true);
        }

        // GET: Notes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.SingleOrDefaultAsync(m => m.ID == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,User,CreationDate,UserId,FinishDate,Finished,NoteText,PriorityEnum,Title")] Note note)
        {
            if (ModelState.IsValid)
            {
                note.CreationDate = DateTime.Now;
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogWarning($"The model state is invalid {ModelState.ValidationState}");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", note.UserId);
            return View(note);
        }

        // GET: Notes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.SingleOrDefaultAsync(m => m.ID == id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", note.UserId);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ID,CreationDate,UserId,FinishDate,Finished,NoteText,PriorityEnum,Title")] Note note)
        {
            if (id != note.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", note.UserId);
            return View(note);
        }

        // GET: Notes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.SingleOrDefaultAsync(m => m.ID == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.SingleOrDefaultAsync(m => m.ID == id);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
    }
}