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

namespace ShoppingList.Controllers
{
    public class ShoppersController : Controller
    {
        private readonly ShoppingListContext _context;

        public ShoppersController(ShoppingListContext context)
        {
            _context = context;
        }

        // GET: Shoppers
        /// <summary>
        /// Searches _context.Shopper for a shopper by Email address.
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(
           string sortOrder,
           string currentFilter,
           string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SortParm"] = String.IsNullOrEmpty(sortOrder) ? "sort_order" : "";

            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var shoppers = from s in _context.Shoppers
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                shoppers = shoppers.Where(s => s.Email.Equals(searchString));
            }

            switch (sortOrder)
            {
                case "sort_order":
                    shoppers = shoppers.OrderByDescending(s => s.LastName);
                    break;

                default:
                    shoppers = shoppers.OrderBy(s => s.LastName);
                    break;
            }
            return View(await shoppers.ToListAsync());

        }


        /// <summary>
        /// 
        /// Queries the context for the Shopper with the primary key, shopperID,
        /// and includes, for each Shopper, 
        ///         a reference to the Lists created for the Shopper, and then
        ///         Items included within each list.
        /// 
        /// </summary>
        /// <param name="shopperID"></param>
        /// <returns></returns>

        public async Task<IActionResult> Details(int? shopperID)
        {
            if (shopperID == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers
                .Include(s => s.Lists)
                    .ThenInclude(t => t.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ShopperID == shopperID)
                .ConfigureAwait(true);
            if (shopper == null)
            {
                return NotFound();
            }

            return View(shopper);
        }


        /// <summary>
        /// 
        /// Creates a new Shopper.
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// Edits the Shopper with primary key shopperID.
        /// 
        /// </summary>
        /// <param name="shopperID"></param>
        /// <returns></returns>
        // GET: Shoppers/Edit/5
        public async Task<IActionResult> Edit(int? shopperID)
        {
            if (shopperID == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers.FindAsync(shopperID);
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
        public async Task<IActionResult> Edit(int shopperID, [Bind("ShopperID,LastName,FirstName,Birthday,Email")] Shopper shopper)
        {
            if (shopperID != shopper.ShopperID)
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

        /// <summary>
        /// 
        /// Does it make sense to have the capability to delete a Shopper?
        /// 
        /// </summary>
        /// <param name="shopperID"></param>
        /// <returns></returns>
        // GET: Shoppers/Delete/5
        public async Task<IActionResult> Delete(int? shopperID)
        {
            if (shopperID == null)
            {
                return NotFound();
            }

            var shopper = await _context.Shoppers
                .FirstOrDefaultAsync(m => m.ShopperID == shopperID);
            if (shopper == null)
            {
                return NotFound();
            }

            return View(shopper);
        }

        // POST: Shoppers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int shopperID)
        {
            var shopper = await _context.Shoppers.FindAsync(shopperID);
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
