using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_Manager.Data;
using Inventory_Manager.Models;

namespace Inventory_Manager.Controllers
{
    public class PartTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PartTypes
        public async Task<IActionResult> Index()

        {
            var  applicationDbContext = _context.PartType.Include(p => p.parts);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PartTypes/Details/5
        public async Task<IActionResult> Details(int? id, string filter)
        {
            if (id == null)
            {
                return NotFound();
            }
            var partType = await _context.PartType.Include(p => p.parts)
                .FirstOrDefaultAsync(m => m.PartTypeId == id);
            
            if (partType == null)
            {
                return NotFound();
            }

            return View(partType);
        }

        // GET: PartTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartTypeId,Section")] PartType partType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partType);
        }

        // GET: PartTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partType = await _context.PartType.FindAsync(id);
            if (partType == null)
            {
                return NotFound();
            }
            return View(partType);
        }

        // POST: PartTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PartTypeId,Section")] PartType partType)
        {
            if (id != partType.PartTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartTypeExists(partType.PartTypeId))
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
            return View(partType);
        }

        // GET: PartTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partType = await _context.PartType
                .FirstOrDefaultAsync(m => m.PartTypeId == id);
            if (partType == null)
            {
                return NotFound();
            }

            return View(partType);
        }

        // POST: PartTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partType = await _context.PartType.FindAsync(id);
            _context.PartType.Remove(partType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartTypeExists(int id)
        {
            return _context.PartType.Any(e => e.PartTypeId == id);
        }
    }
}
