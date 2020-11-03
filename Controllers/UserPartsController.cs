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
    public class UserPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserParts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserParts.Include(u => u.Part).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userParts = await _context.UserParts
                .Include(u => u.Part)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserPartsId == id);
            if (userParts == null)
            {
                return NotFound();
            }

            return View(userParts);
        }

        // GET: UserParts/Create
        public IActionResult Create()
        {
            ViewData["PartsId"] = new SelectList(_context.Parts, "PartsId", "PartName");
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: UserParts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserPartsId,UserId,PartsId")] UserParts userParts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userParts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartsId"] = new SelectList(_context.Parts, "PartsId", "PartName", userParts.PartsId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", userParts.UserId);
            return View(userParts);
        }

        // GET: UserParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userParts = await _context.UserParts.FindAsync(id);
            if (userParts == null)
            {
                return NotFound();
            }
            ViewData["PartsId"] = new SelectList(_context.Parts, "PartsId", "PartName", userParts.PartsId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", userParts.UserId);
            return View(userParts);
        }

        // POST: UserParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserPartsId,UserId,PartsId")] UserParts userParts)
        {
            if (id != userParts.UserPartsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userParts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPartsExists(userParts.UserPartsId))
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
            ViewData["PartsId"] = new SelectList(_context.Parts, "PartsId", "PartName", userParts.PartsId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", userParts.UserId);
            return View(userParts);
        }

        // GET: UserParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userParts = await _context.UserParts
                .Include(u => u.Part)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserPartsId == id);
            if (userParts == null)
            {
                return NotFound();
            }

            return View(userParts);
        }

        // POST: UserParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userParts = await _context.UserParts.FindAsync(id);
            _context.UserParts.Remove(userParts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPartsExists(int id)
        {
            return _context.UserParts.Any(e => e.UserPartsId == id);
        }
    }
}
