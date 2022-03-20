#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Data;
using ShoppingList.Models;

namespace ShoppingList.wwwroot
{
    public class ShoppersController : Controller
    {
        private readonly ShoppingListContext _context;

        public ShoppersController(ShoppingListContext context)
        {
            _context = context;
        }

        // GET: Shoppers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shoppers.ToListAsync());
        }

        // GET: Shoppers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers
                .FirstOrDefaultAsync(m => m.ShopperID == id);
            if (shopper == null)
            {
                return NotFound();
            }

            return View(shopper);
        }

        // GET: Shoppers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shoppers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShopperID,LastName,FirstName,Birthday,Email")] Shopper shopper)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopper);
        }

        // GET: Shoppers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers.FindAsync(id);
            if (shopper == null)
            {
                return NotFound();
            }
            return View(shopper);
        }

        // POST: Shoppers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShopperID,LastName,FirstName,Birthday,Email")] Shopper shopper)
        {
            if (id != shopper.ShopperID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopperExists(shopper.ShopperID))
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
            return View(shopper);
        }

        // GET: Shoppers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers
                .FirstOrDefaultAsync(m => m.ShopperID == id);
            if (shopper == null)
            {
                return NotFound();
            }

            return View(shopper);
        }

        // POST: Shoppers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopper = await _context.Shoppers.FindAsync(id);
            _context.Shoppers.Remove(shopper);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopperExists(int id)
        {
            return _context.Shoppers.Any(e => e.ShopperID == id);
        }
    }
}
