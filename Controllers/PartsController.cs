using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_Manager.Data;
using Inventory_Manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Inventory_Manager.Controllers
{
    public class PartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public PartsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Parts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Parts.Include(p => p.partType);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> MainLineIndex()
        {
            var applicationDbContext = _context.Parts.Include(p => p.partType).Where(p => p.partType.Section == "Main Line");
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> ServiceLineIndex()
        {
            var applicationDbContext = _context.Parts.Include(p => p.partType).Where(p => p.partType.Section == "Service Lines");
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> HydrantIndex()
        {
            var applicationDbContext = _context.Parts.Include(p => p.partType).Where(p => p.partType.Section == "Hydrants");
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> UserParts()
        {
            var applicationDbContext = _context.UserParts.Include(p => p.Part);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts
                .Include(p => p.partType)
                .Include(p => p.UserParts)
                .FirstOrDefaultAsync(m => m.PartsId == id);
            

            if (parts == null)
            {
                return NotFound();
            }
            
            return View(parts);
        }

        // GET: Parts/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["PartTypeId"] = new SelectList(_context.PartType, "PartTypeId", "Section");
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartsId,PartTypeId,PartName,Quantity,Size,SerialNumber")] Parts parts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartTypeId"] = new SelectList(_context.PartType, "PartTypeId", "Section", parts.PartTypeId);
            return View(parts);
        }

        // GET: Parts/Edit/5
       [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts.FindAsync(id);
            if (parts == null)
            {
                return NotFound();
            }
            ViewData["PartTypeId"] = new SelectList(_context.PartType, "PartTypeId", "Section", parts.PartTypeId);
            return View(parts);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PartsId,PartTypeId,PartName,Quantity,Size,SerialNumber")] Parts parts)
        {
            if (id != parts.PartsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartsExists(parts.PartsId))
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
            ViewData["PartTypeId"] = new SelectList(_context.PartType, "PartTypeId", "Section", parts.PartTypeId);
            return View(parts);
        }

        // GET: Parts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts
                .Include(p => p.partType)
                .FirstOrDefaultAsync(m => m.PartsId == id);
            if (parts == null)
            {
                return NotFound();
            }

            return View(parts);
        }

        // POST: Parts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parts = await _context.Parts.FindAsync(id);
            _context.Parts.Remove(parts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartsExists(int id)
        {
            return _context.Parts.Any(e => e.PartsId == id);
        }

       [Authorize]
        public async Task<IActionResult> Checkout(int? id, Parts Parts)
        {
            var user = await GetCurrentUserAsync();
           // Parts partsToCheckout = await _context.Parts
                //.Include(p => p.partType)
               // .Include(p => p.UserParts)
               // .FirstOrDefaultAsync(m => m.PartsId == id);
            var parts = await _context.Parts
                .Include(p => p.partType)
                .Include(p => p.UserParts)
                .FirstOrDefaultAsync(m => m.PartsId == id);
            var newPartQuantity = parts.Quantity - 1;
            parts.Quantity = newPartQuantity;
            _context.Update(parts);
            await _context.SaveChangesAsync();


            UserParts newPart = new UserParts()
            {
                UserId = user.Id,
                PartsId = id.Value
            };

            _context.Add(newPart);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
            // _context.Add(newPart);

            // await _context.SaveChangesAsync();
            // }
            //partsToCheckout.Quantity -= partsToCheckout.UserParts.Count();

            
        
    }
}

