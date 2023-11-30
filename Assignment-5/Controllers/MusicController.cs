using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment_5.Data;
using Assignment_5.Models;

namespace Assignment_5.Controllers
{
    public class MusicController : Controller
    {
        private readonly Assignment_5Context _context;

        public MusicController(Assignment_5Context context)
        {
            _context = context;
        }

        // GET: Music
        public async Task<IActionResult> Index()
        {
              return _context.Music != null ? 
                          View(await _context.Music.ToListAsync()) :
                          Problem("Entity set 'Assignment_5Context.Music'  is null.");
        }

        // GET: Music/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Music == null)
            {
                return NotFound();
            }

            var music = await _context.Music
                .FirstOrDefaultAsync(m => m.ID == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        // GET: All genres of music
        public async Task<IActionResult> GetAllMusicItems()
        {
            var musicItems = await _context.Music.ToListAsync();
            return Json(musicItems);
        }

        // GET: Music items by genre
        public async Task<IActionResult> GetMusicItemsByGenre(string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return BadRequest("Genre is required.");
            }

            var musicItems = await _context.Music
                .Where(item => item.Genre == genre)
                .ToListAsync();

            return Json(musicItems);
        }

        // GET: Music/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Music/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Genre,Performer,Price")] Music music)
        {
            if (ModelState.IsValid)
            {
                _context.Add(music);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(music);
        }

        // GET: Music/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Music == null)
            {
                return NotFound();
            }

            var music = await _context.Music.FindAsync(id);
            if (music == null)
            {
                return NotFound();
            }
            return View(music);
        }

        // POST: Music/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Genre,Performer,Price")] Music music)
        {
            if (id != music.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(music);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicExists(music.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(music);
        }

        // GET: Music/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Music == null)
            {
                return NotFound();
            }

            var music = await _context.Music
                .FirstOrDefaultAsync(m => m.ID == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        // POST: Music/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Music == null)
            {
                return Problem("Entity set 'Assignment_5Context.Music'  is null.");
            }
            var music = await _context.Music.FindAsync(id);
            if (music != null)
            {
                _context.Music.Remove(music);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicExists(int id)
        {
          return (_context.Music?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
