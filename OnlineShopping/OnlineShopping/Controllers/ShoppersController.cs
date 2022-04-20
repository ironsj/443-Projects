#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class ShoppersController : Controller
    {
        private readonly OnlineShoppingContext _context;

        public ShoppersController(OnlineShoppingContext context)
        {
            _context = context;
        }


        // GET: Shoppers
        /// <summary>
        /// Searches _context.Shopper for a shopper with Email address.
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



        // GET: Shoppers/Details/5
        public async Task<IActionResult> Details(int? shopperID)
        {
            if (shopperID == null)
            {
                return NotFound();
            }

            // Queries the context for the shopper denoted by shopperID and
            // includes the multiple cart references for each shopper
            var shopper = await _context.Shoppers             
                .Include(s => s.Carts)
                    .ThenInclude(o => o.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ShopperID == shopperID)
                .ConfigureAwait(true);
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


        //POST: Shoppers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// Creates a new shopper, but only if the Email address is not used by another shopper.
        /// 
        /// </summary>
        /// <param name="searchString">An email address has the form: str@example.com</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShopperID,LastName,FirstName,Email")] Shopper shopper)
        {
            var shoppers = from s in _context.Shoppers
                           select s;

            // Narrow down shoppers to the one shopper with the Email address referenced by shopper.Email.
            shoppers = shoppers.Where(s => s.Email.Equals(shopper.Email));
            Shopper temp = shoppers.FirstOrDefault();

            // Emails are unque, so that (temp == null) denotes a new shopper.
            if (temp == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Shoppers.Add(shopper);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), "Shoppers", new { shopperID = shopper.ShopperID });
                }
            }

            // The Email address alreay exists. Go back to the orginal view..
            // Response.StatusCode = 404;
            // return View("Error: email address already used");
            return View(shopper);
        }



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
        public async Task<IActionResult> Edit(int shopperID, [Bind("ShopperID,LastName,FirstName,Email")] Shopper shopper)
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
