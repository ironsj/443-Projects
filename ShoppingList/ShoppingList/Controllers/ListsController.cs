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
    public class ListsController : Controller
    {
        private readonly ShoppingListContext _context;

        public ListsController(ShoppingListContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 
        /// Returns a View for a list of shopping Lists, possibly filtered down by
        /// a shopper Email.
        /// 
        /// </summary>
        /// <param name="currentFilter"></param>
        /// <param name="searchString">list by Email</param>
        /// <param name="searchString">to identify a month-day-year for the list creation</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(
            string currentFilter,
            string searchString)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var lists = from s in _context.Lists
                        select s;
            lists = lists.Include(s => s.Shopper);

            if (!String.IsNullOrEmpty(searchString))
            {
                lists = lists.Where(s => s.Shopper.Email.Equals(searchString));
            }

            // For a different kind of search.
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //string[] date = searchString.Split('/');
            //lists = lists.Where(s =>
            //    s.TimeSlot.Value.Date.Month == Int16.Parse(date[0].Trim()) &&
            //    s.TimeSlot.Value.Date.Day == Int16.Parse(date[1].Trim()) &&
            //    s.TimeSlot.Value.Date.Year == Int16.Parse(date[2].Trim())
            //  );
            //}

            return View(await lists.ToListAsync());
        }




        // GET: Lists/Details/5
        /// <summary>
        /// 
        /// Extracts the list specified by the parameter listID from _context.Lists,
        /// including links for 
        ///         the Shopper and 
        ///         the List collection of the Items in the list
        ///         
        /// Returns an appropriate view.
        /// 
        /// ------------------------------------------------------------
        /// </summary>
        /// <param name="listID"></param>
        /// <returns> View for a list </returns>
        public async Task<IActionResult> Details(int? listID)
        {
            if (listID == null)
            {
                return NotFound();
            }

            var list = await _context.Lists

                .Include(a => a.Shopper)
                .Include(a => a.Items)
 
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ListID == listID)
                .ConfigureAwait(true);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }



        /// <summary>
        /// 
        /// Creates a new list for a Shopper with the parameter specified ShopperID.
        /// 
        /// </summary>
        /// <param name="shopperID">identifies a Shopper</param>
        /// <returns>View for a List</returns>
        // GET: Lists/Create
        public IActionResult Create(int? shopperID)
        {
            var shopper = _context.Shoppers
                .FirstOrDefault(c => c.ShopperID == shopperID);

            ViewData["ShopperID"] = shopperID;
            ViewData["ShopperName"] = shopper.FullName;
            ViewData["ShopperEmail"] = shopper.Email;
            return View();
        }

        // POST: Lists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
 
        /// <summary>
        ///
        /// 
        /// Initializes the nullable properties of the newly created List
        ///   A) Subtotal   to 0M
        ///   B) Tax        to 0M
        ///   C) TotalCost  to 0M
        ///   D) TimeSlot   with the current DateTime, a time stamp for the List time of creation
        ///   ***********************************************************************************
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListID,ShopperID","Name")] List list)
        {
            if (ModelState.IsValid)
            {
                list.Subtotal = 0M;
                list.Tax = 0M;
                list.TotalCost = 0M;
                list.TimeSlot = System.DateTime.Now;

                _context.Lists.Add(list);
                await _context.SaveChangesAsync();
 
                return RedirectToAction(nameof(Details), "Shoppers", new { shopperID = list.ShopperID });
            }

            // Try again.
            //*****************************************************
            // 5) Navigate
            //      A) to the Details action
            //      B) in the Lists controller
            //      C) for the specific list.
            //*****************************************************
            return RedirectToAction(nameof(Details), "Lists", new { listID = list.ListID });
        }

        /// <summary>
        /// 
        /// Does it make sense in this application to execute an Edit action, other than the list Title ?
        /// 
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        /// 
        // GET: Lists/Edit/5
        public async Task<IActionResult> Edit(int? listID)
        {
            if (listID == null)
            {
                return NotFound();
            }

            var list = await _context.Lists.FindAsync(listID);
            if (list == null)
            {
                return NotFound();
            }
            ViewData["ShopperID"] = new SelectList(_context.Shoppers, "ShopperID", "ShopperID", list.ShopperID);
            return View(list);
        }

        /// <summary>
        /// 
        /// Does it make sense in this application to execute an Edit action, other than the list Title?
        /// 
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        /// 
        // POST: Lists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int listID, [Bind("ListID,ShopperID,Name,TimeSlot,Subtotal,Tax,TotalCost")] List list)
        {
            if (listID != list.ListID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(list);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.ListID))
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
            ViewData["ShopperID"] = new SelectList(_context.Shoppers, "ShopperID", "ShopperID", list.ShopperID);
            return View(list);
        }

        // GET: Lists/Delete/5
        public async Task<IActionResult> Delete(int? listID)
        {
            if (listID == null)
            {
                return NotFound();
            }

            var list = await _context.Lists
                .Include(l => l.Shopper)
                .FirstOrDefaultAsync(m => m.ListID == listID);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        /// <summary>
        /// 
        /// **************
        /// Assignment U12
        /// **************
        ///    Write code to remove from
        ///                             _context.Items
        ///    each item in the List that is specified by the parameeter listID, i.e.
        ///       
        ///                   list.Items.Where(e => e.ListID == listID).ToList();
        ///                   
        ///    Remark. Note that, in this application, the items in a list are not
        ///            shared by any other list, so delete them along with the list.
        /// 
        ///    Remark. Be reminded that the scaffolder writes the code to remove the list
        ///            from _Context.Lists
        /// *****************************************************************************
        /// 
        /// </summary>
        /// 
        /// <param name="listID"></param>
        /// <returns></returns>
        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int listID)
        {
            //  Redefine a list declaration.  Why?
            //   var list = await _context.Lists.FindAsync(id); 

            // Replacement code for a list declaration.  What is gained by this?
            var list = await _context.Lists

                .Include(a => a.Items)   // The value given to List.Items is needed for the following code
                .AsNoTracking()

                .FirstOrDefaultAsync(m => m.ListID == listID);

            // The value given to List.Items is needed in the following line of code
            var items = list.Items.Where(e => e.ListID == listID).ToList();
            foreach (Item e in items)
            {

                // ********************************************************************************
                //  For ten points, FIX THIS 1 line of code to remove each of the items from Items
                // ********************************************************************************

               _context.Items.Remove(e);

            }

            _context.Lists.Remove(list);        // This line of code is provided by the scaffolder,
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ListExists(int id)
        {
            return _context.Lists.Any(e => e.ListID == id);
        }
    }
}
