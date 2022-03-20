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
    public class ItemsController : Controller
    {
        private readonly ShoppingListContext _context;

        public ItemsController(ShoppingListContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var shoppingListContext = _context.Items.Include(i => i.List);
            return View(await shoppingListContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? itemID)
        {
            if (itemID == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.List)
                .FirstOrDefaultAsync(m => m.ItemID == itemID);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        /// <summary>
        /// 
        /// With the added parameter to identify a paraticular list, 
        /// the Create method creates an Item for that List.
        /// 
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>

        // GET: Items/Create
        public IActionResult Create(int? listID)
        {
            ViewData["ListID"] = listID;

            return View();
        }

        /// <summary>
        /// 
        /// Creates a new Item for a specified list.
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,ListID,ProductName,UnitPrice,Available,Quantity,IsTaxable,Cost,TaxRate,Tax,TotalCost")] Item item)
        {
            List list = _context.Lists.Include(c => c.Shopper).FirstOrDefault(m => m.ListID == item.ListID);

            if (ModelState.IsValid)
            {
                // *************************************************************************************
                // Defines nullable Item properties, Tax and TotalCost, with the following computations.
                item.Cost = item.UnitPrice * item.Quantity;
                if (item.TaxRate > 0M)
                {
                    item.Tax = item.Cost * item.TaxRate;
                }
                else
                {
                    item.Tax = 0.0M;
                }
                item.TotalCost = item.Cost + item.Tax;

                // Increments the nullible list totals for the newly created Item.
                if (item.Available)
                {
                    list.Subtotal += item.Cost;
                    list.Tax += item.Tax;
                    list.TotalCost = list.Subtotal + list.Tax;
                }
                //**************************************************************************************


                _context.Add(item);
                await _context.SaveChangesAsync();
 
                // Invoking an action in another controller and passing route data via the parameter listID.
                return RedirectToAction(nameof(Details), "Lists", new { listID = item.ListID });
            }

            ViewData["ListID"] = item.ListID;
            return View(item);
        }





        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? itemID)
        {
            if (itemID == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(itemID);
            if (item == null)
            {
                return NotFound();
            }
            //     ViewData["ListID"] = new SelectList(_context.Lists, "ListID", "ListID", item.ListID);
            ViewData["ListID"] = item.ListID;

            return View(item);
        }


        /// <summary>
        /// 
        /// **************
        /// Assignment U14
        /// **************
        ///    Write code to complete the following Edit Post method.
        /// *****************************************************************************
        /// 
        /// 
        /// </summary>

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int itemID, [Bind("ItemID,ListID,ProductName,UnitPrice,Available,Quantity,IsTaxable,Cost,TaxRate,Tax,TotalCost")] Item item)
        {
            // Parameter item specifies the changes to be made to replace the previously stored Item.
            if (itemID != item.ItemID)
            {
                return NotFound();
            }

            List list = _context.Lists
                .Include(c => c.Shopper)
                .FirstOrDefault(m => m.ListID == item.ListID);

            // The variable storedItem, which is extracted from the ShoppingList database via
            //                                   _context.Items,
            // contains the unedited version of Item before any changes to Item are to be made.
            Item storedItem = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ItemID == itemID);

            if (ModelState.IsValid)
            {
                try
                {
                    // ************************************************************************
                    item.Cost = item.UnitPrice * item.Quantity;
                    if (item.TaxRate > 0)
                    {
                        item.Tax = item.Cost * item.TaxRate;
                    }
                    else
                    {
                        item.Tax = 0.0M;
                    }
                    item.TotalCost = item.Cost + item.Tax;


                    if (item.Available)
                    {
                        if (storedItem.Available)
                        {
                            // Incrment the properties with the difference
                            // between the current item and the previously storedItem

                            list.Subtotal += item.Cost - storedItem.Cost;
                            list.Tax += item.Tax - storedItem.Tax;
                        }
                        else
                        {
                            list.Subtotal += item.Cost;
                            list.Tax += item.Tax;
                        }
                    }
                    else if (storedItem.Available)
                    {
                        list.Subtotal -= storedItem.Cost;  
                        list.Tax  -= storedItem.Tax;
                    }
                    list.TotalCost = list.Subtotal + list.Tax;
 

                    // Update the context with the edited item
                   
                     _context.Update(item);
                   // ***********************************************************************************

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
 
                return RedirectToAction(nameof(Details), "Lists", new { listID = item.ListID });
            }
            ViewData["ListID"] = new SelectList(_context.Lists, "ListID", "ListID", item.ListID);
            return View(item);
        }


        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? itemID)
        {
            if (itemID == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.List)
                .FirstOrDefaultAsync(m => m.ItemID == itemID);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        /// <summary>
        /// 
        /// **************
        /// Assignment U13
        /// **************
        ///    Write code:
        ///          to remove the Item with primary key, itemID, from
        ///                             _context.Items
        ///          and make appropriate changes to the List nullable properties.
        /// *****************************************************************************
        /// 
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int itemID)
        {
            // The item to remove from the database context, i.e. _context.Items.

            var item = await _context.Items.FindAsync(itemID);
            List list = _context.Lists
                .Include(c => c.Shopper)
                .FirstOrDefault(m => m.ListID == item.ListID);

            if (item.Available)
            {
                // ******************************************************************
                //  Complete two lines of code to reassign the list Subtotal and Tax
                // ******************************************************************

                list.Subtotal -= item.Cost;
                list.Tax -= item.Tax;

                list.TotalCost = list.Subtotal + list.Tax;
            }

            // ********************************************
            //  Remove the appropriate Item from
            //                _context.Items
            // *********************************************
            _context.Items.Remove(item);



            await _context.SaveChangesAsync();

            // Redirect to the following Action.
            return RedirectToAction(nameof(Details), "Lists", new { listID = item.ListID });
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemID == id);
        }
    }
}
