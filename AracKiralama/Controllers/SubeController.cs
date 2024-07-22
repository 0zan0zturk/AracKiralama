using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AracKiralama.Models;

namespace AracKiralama.Controllers
{
    [SessionCheckFilterAdmin]
    public class SubeController : Controller
    {
        DataContext _context = new DataContext();

        // GET: Subes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subeler.ToListAsync());
        }

        // GET: Subes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sube = await _context.Subeler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sube == null)
            {
                return NotFound();
            }

            return View(sube);
        }

        // GET: Subes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubeAdi,SubeAdresi")] Sube sube)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sube);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sube);
        }

        // GET: Subes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sube = await _context.Subeler.FindAsync(id);
            if (sube == null)
            {
                return NotFound();
            }
            return View(sube);
        }

        // POST: Subes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubeAdi,SubeAdresi")] Sube sube)
        {
            if (id != sube.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sube);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubeExists(sube.Id))
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
            return View(sube);
        }

        // GET: Subes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sube = await _context.Subeler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sube == null)
            {
                return NotFound();
            }

            return View(sube);
        }

        // POST: Subes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sube = await _context.Subeler.FindAsync(id);
            if (sube != null)
            {
                _context.Subeler.Remove(sube);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubeExists(int id)
        {
            return _context.Subeler.Any(e => e.Id == id);
        }
    }
}
