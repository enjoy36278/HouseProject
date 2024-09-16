using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public class HouseTypesController : Controller
    {
        private readonly dbHouseContext _context;

        public HouseTypesController(dbHouseContext context)
        {
            _context = context;
        }

        // GET: HouseTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.HouseType.ToListAsync());
        }

        // GET: HouseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseType = await _context.HouseType
                .FirstOrDefaultAsync(m => m.TypeID == id);
            if (houseType == null)
            {
                return NotFound();
            }

            return View(houseType);
        }

        // GET: HouseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HouseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeID,TypeName")] HouseType houseType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(houseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(houseType);
        }

        // GET: HouseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseType = await _context.HouseType.FindAsync(id);
            if (houseType == null)
            {
                return NotFound();
            }
            return View(houseType);
        }

        // POST: HouseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeID,TypeName")] HouseType houseType)
        {
            if (id != houseType.TypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(houseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseTypeExists(houseType.TypeID))
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
            return View(houseType);
        }

        // GET: HouseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseType = await _context.HouseType
                .FirstOrDefaultAsync(m => m.TypeID == id);
            if (houseType == null)
            {
                return NotFound();
            }

            return View(houseType);
        }

        // POST: HouseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var houseType = await _context.HouseType.FindAsync(id);
            if (houseType != null)
            {
                _context.HouseType.Remove(houseType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HouseTypeExists(int id)
        {
            return _context.HouseType.Any(e => e.TypeID == id);
        }
    }
}
