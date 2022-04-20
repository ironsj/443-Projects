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
    public class CartsController : Controller
    {
        private readonly OnlineShoppingContext _context;

        public CartsController(OnlineShoppingContext context)
        {
            _context = context;
        }

 
        // GET: Carts
        /// <summary>
        /// 
        /// Queries the Carts context to include the Shopper navigation property 
        /// 
        /// Returns a View for a list of Shopper Carts.
        /// 
        /// </summary>
        /// <param name="currentFilter"></param>
        /// <param name="searchString">to identify a month-day-year for the cart creation</param>
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

            var carts = from s in _context.Carts
                        select s;
            carts = carts.Include(s => s.Shopper);

            if (!String.IsNullOrEmpty(searchString))
            {
                string[] date = searchString.Split('/');
                carts = carts.Where(s =>
                    s.TimeSlot.Value.Date.Month == Int16.Parse(date[0].Trim()) &&
                    s.TimeSlot.Value.Date.Day == Int16.Parse(date[1].Trim()) &&
                    s.TimeSlot.Value.Date.Year == Int16.Parse(date[2].Trim())
                );
            }

            return View(await carts.ToListAsync());
        }



        // GET: Carts/Details/5
        /// <summary>
        /// 
        /// Details, for a Cart, includes a list of Orders with Product references
        /// 
        /// Returns a View for the cart denoted by cartID.
        /// 
        /// </summary>
        /// <param name="cartID"> </param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? cartID)
        {
            if (cartID == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(a => a.Shopper)
                .Include(a => a.Orders)                         // added after scaffolding
                    .ThenInclude(a => a.Product)                // added after scaffolding
                .AsNoTracking()                                 // added after scaffolding
                .FirstOrDefaultAsync(m => m.CartID == cartID)
                .ConfigureAwait(true);                          // added after scaffolding
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }



        /// <summary>
        /// 
        /// Creates a cart for a shopper denoted by shopperID, unless the
        /// shopper already has a cart that has not yet been checked out.
        /// 
        /// </summary>
        /// <param name="shopperID"> </param>
        /// <returns></returns>
        // GET: Carts/Create
        public IActionResult Create(int? shopperID)
        {
            if (shopperID != null)
            {
                int num = (int)shopperID;

                // Find the cart, if it exists, for the shopper
                // denoted by shopperID, that is not yet checked out.

                var cart = _context.Carts
                    .Include(c => c.Shopper)
                    .AsNoTracking()
                    .FirstOrDefault(c => c.ShopperID == shopperID && !c.CheckedOut);

                // If the cart exists, where cart != null, have the shopper
                // continue to use the previously created cart.

                if (cart != null)
                {
                    return RedirectToAction(nameof(Details), "Carts", new { cartID = cart.CartID });
                }
            }

            //  Query the Shoppers context for the shopper denoted by shopperID
            //  and proceeds to create a cart.
            var shopper = _context.Shoppers
                .FirstOrDefault(c => c.ShopperID == shopperID);

            ViewData["ShopperID"] = shopperID;
            ViewData["ShopperName"] = shopper.LastName + ", " + shopper.FirstName;
            ViewData["ShopperEmail"] = shopper.Email;
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// Create initializes the nullable properties of the cart
        /// and adds it to the database context. 
        /// 
        /// Creates and then navigates back to the Carts Details view.
        /// 
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,ShopperID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                // Start with an empty cart
                cart.Subtotal = 0M;
                cart.Tax = 0M;
                cart.TotalCost = 0M;
                cart.TimeSlot = System.DateTime.Now;
                cart.CheckedOut = false;

                // Register the cart
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), "Carts", new { cartID = cart.CartID });
        }



 
        /// <summary>
        /// 
        /// Edit checks out a cart. The Carts Details view contains a Checkout hyperlink
        /// action to invoke Edit to complete the checkout.
        /// 
        /// Remark for reflection. A shopper does not have the capability to
        /// either edit or delete a Cart. Furthermore, an order in a checked
        /// out cart cannot be edited or deleted.
        /// 
        /// </summary>
        /// <param name="cartID"> </param>
        /// <returns></returns>
        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? cartID)
        {
            if (cartID == null)
            {
                return NotFound();
            }

            // Query the context for a cart denoted by cartID.
            var temp = await _context.Carts
                .Include(c => c.Shopper)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CartID == cartID)
                .ConfigureAwait(true);
            if (temp.CheckedOut)
            {
                // Eding a cart that is already checked out is not admissible in this application.
                return RedirectToAction(nameof(Details), "Shoppers", new { shopperID = temp.Shopper.ShopperID });
            }

            // Proceed to checkout the cart.
            var cart = await _context.Carts.FindAsync(cartID);
            if (cart == null)
            {
                return NotFound();
            }

            //if (cart.Subtotal == 0)
            //{
            //    return RedirectToAction(nameof(Details), "Carts", new { id = cart.CartID });
            //}

            // Proceed to complete Edit.
            ViewData["ShopperID"] = temp.Shopper.ShopperID;
            ViewData["ShopperName"] = temp.Shopper.LastName + ", " + temp.Shopper.FirstName;
            ViewData["CartID"] = cartID;
            ViewData["CartTotal"] = ((Decimal)cart.TotalCost).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            return View(cart);
        }


        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// Edit checks out a cart, where the Cart Details Razor page contains a Checkout hyperlink
        /// action to invoke Edit to complete a checkout.
        /// 
        /// Whenever a cart is checked out, the Order Quantity is checked not to exceed the Available
        /// property, where changes are made to the Order Quantity and Product Availablity to complete
        /// the transaction.
        /// 
        /// </summary>
        /// <param name="cartID"> </param>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int cartID, [Bind("CartID,ShopperID")] Cart cart)
        {
            if (cartID != cart.CartID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Query the Carts context for a Cart denoted by CartID,
                // tentatively, to be checked out.
                var savedCart = await _context.Carts
                    .Include(a => a.Orders)
                    .ThenInclude(a => a.Product)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.CartID == cartID)
                    .ConfigureAwait(true);

                // Include the required code to check out the cart.
 
                cart.Subtotal = 0M;
                cart.Tax = 0M;
                bool cartReadyToCheckOut = true;

                foreach (Order order in savedCart.Orders)
                {
                    // Query to find the saved Order in the Orders context, and the Product in the Products context
                    Order savedOrder = _context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
                    Product product = _context.Products.FirstOrDefault(m => m.ProductID == order.ProductID);

                    if (product.Available >= order.Quantity)
                    {
                        // Increment cart properties for the Order

                        cart.Subtotal += order.Cost;
                        cart.Tax += order.Tax;
                        cart.TotalCost = cart.Subtotal + cart.Tax;
                    }
                    else
                    {
                        // Zero out savedOrder properties, due to insufficient inventory
                        savedOrder.Quantity = 0;
                        savedOrder.Cost = 0M;
                        savedOrder.Tax = 0M;
                        savedOrder.Total = 0M;

                        //_context.Order.Update(savedOrder);  // Is this necessary?  // Maybe.  Be careful !!

                        cartReadyToCheckOut = false;
                    }
                }

                if (cartReadyToCheckOut)
                {
                    // Cart is still ready to check out.
                    cart.TimeSlot = DateTime.Now;
                    cart.CheckedOut = true;

                    foreach (Order order in savedCart.Orders)
                    {
                        // Query to find the saved Order in the Orders context, and the
                        // Product in the Products context
                        Order savedOrder = _context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
                        Product product = _context.Products.FirstOrDefault(m => m.ProductID == order.ProductID);

                        //Update the product inventory
                        product.Available -= order.Quantity;
                        _context.Products.Update(product);  // Is this step necessary?
                    }
                }
                //else
                //{
                //    cart.Subtotal = 0M;
                //    cart.Tax = 0M;
                //    cart.TotalCost = 0M;
                //    foreach (Order order in savedCart.Orders)
                //    {
                //        // Query to find the saved order in the order context, and the product in the product context
                //        Order savedOrder = _context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
                //        Product product = _context.Products.FirstOrDefault(m => m.ProductID == order.ProductID);

                //        if (product.Available >= order.Quantity)
                //        {
                //            // Update cart properties for the order(two lines of code).

                //            cart.Subtotal += order.Cost;
                //            cart.Tax += order.Tax;
                //            cart.TotalCost = cart.Subtotal + cart.Tax;
                //        }

                //            cartReadyToCheckOut = false;

                //    }
                //}


                // Update the database context and the database.
                try
                {
                    _context.Carts.Update(cart);   // Is this step necessary?

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Navigate to the Carts Details action.

                return RedirectToAction(nameof(Details), "Carts", new { cartID = cart.CartID });
            }

            ViewData["ShopperID"] = new SelectList(_context.Shoppers, "ShopperID", "FirstName", cart.ShopperID);
            return View(cart);
        }


        /// <summary>
        /// 
        /// A shopper does not have the capability to either delete or edit a
        /// Cart. However, a Cart Delete and a Cart Edit are accessible through
        /// the hyperlink on the navigation bar.
        /// 
        /// </summary>
        /// <param name="cartID"> </param>
        /// <returns></returns>
        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? cartID)
        {
            if (cartID == null)
            {
                return NotFound();
            }
            //Query the context for the cart with the parameter given primary key.
            var cart = await _context.Carts
                .Include(c => c.Shopper)
                .FirstOrDefaultAsync(m => m.CartID == cartID);
            if (cart == null)
            {
                return NotFound();
            }

            if (cart.CheckedOut)
            {
                // A cart, that is already checked out, is not eligible to be deleted.
                return RedirectToAction(nameof(Index));
            }

            // Continue the process to delete the cart. Nothing is
            // actually in the cart before checkout.  Explain this.
            return View(cart);
        }


        /// <summary>
        /// 
        ///  No orders are to be deleted from the cart, as the product inventory is
        ///  only changed during a Cart Edit, i.e. for Cart checkout.
        ///  
        /// </summary>
        /// <param name="cartID"> </param>
        /// <returns></returns>
        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int cartID)
        {
            //Query the context for the cart denoted by cartID
            var cart = await _context.Carts.FindAsync(cartID);

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartID == id);
        }
    }
}
