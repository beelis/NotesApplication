using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Notes.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesController(ApplicationDbContext context, ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<NotesController>();
            _userManager = userManager;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = await _userManager.GetUserIdAsync(user);

            var notes = from m in _context.Note
                         select m;

            if (!String.IsNullOrEmpty(userId))
            {
                notes = notes.Where(s => s.UserId.Equals(userId));
            }

            var applicationDbContext = _context.Note.Include(n => n.User);
            return View(await notes.ToListAsync());
        }

        // GET: Notes/Details/5
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
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,User,CreationDate,UserId,FinishDate,Finished,NoteText,PriorityEnum,Title")] Note note)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CreationDate,UserId,FinishDate,Finished,NoteText,PriorityEnum,Title")] Note note)
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
