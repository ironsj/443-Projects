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
    public class OrdersController : Controller
    {
        private readonly OnlineShoppingContext _context;

        public OrdersController(OnlineShoppingContext context)
        {
            _context = context;
        }


        // GET: Orders
        /// <summary>
        /// 
        /// Returns a view for all orders
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var orders = _context.Orders
                .Include(o => o.Cart)
                    .ThenInclude(o => o.Shopper)
                .Include(o => o.Product);

            return View(orders);
        }

        // GET: Orders/Details/5
        /// <summary>
        /// 
        /// Returns a view for a particulat order.
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? orderID)
        {
            if (orderID == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Cart)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderID == orderID);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // GET: Orders/Create
        /// <summary>
        /// 
        /// Creates a new order for the cart denoted by cartID, if the cart is not checked out.
        /// 
        /// </summary>
        /// <param name="orderID">identifies a CartID</param>
        /// <returns></returns>
        public IActionResult Create(int? cartID, int? prodId, string prodName, int? prodAvailable)
        {
            // Query the Cart context for the cart denoted by CartID.
            var cart = _context.Carts.Include(c => c.Shopper).FirstOrDefault(c => c.CartID == cartID);
            if (cart.CheckedOut)
            {
                // Continue to use the Cart not yet checked out.
                return RedirectToAction(nameof(Details), "Shoppers", new { shopperID = cart.Shopper.ShopperID });
            }

            // Create a new Order
            ViewData["CartID"] = cartID;
            ViewData["ShopperEmail"] = cart.Shopper.Email;
            ViewData["ProductID"] = prodId;
            ViewData["ProductName"] = prodName;
            ViewData["ProductAvailable"] = prodAvailable;
            return View();
        }


        /// <summary>
        /// 
        /// Creates a new order.
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,CartID,ProductID,Quantity")] Order order)
        {
            // Query the Cart context for the cart that contains the given order
            int cartID = order.CartID;
            Cart cart = _context.Carts.Include(c => c.Shopper).FirstOrDefault(m => m.CartID == cartID);

            int availability = 0;
            if (ModelState.IsValid)
            {
                // Query the Product context for the product obtained by the given order
                int productID = order.ProductID;
                Product product = _context.Products.FirstOrDefault(m => m.ProductID == productID);
                availability = product.Available;

                // Query the Order context for a previous order in the Cart for the same product,
                // denoted by productID.
                Order pastOrder = _context.Orders
                    .Include(r => r.Cart)
                    .Include(p => p.Product)
                    .FirstOrDefault(m => m.CartID == cartID && m.ProductID == productID);

                if (pastOrder == null)
                {
                    if (order.Quantity > availability)
                    {
                        order.Quantity = availability;
                    }
                }
                else
                {
                    if (order.Quantity > availability - pastOrder.Quantity)
                    {
                        order.Quantity = availability - pastOrder.Quantity;
                    }
                }

                // Initialize the order Cost and Tax. (Two lines of code)
                order.Cost = (Decimal)product.UnitPrice * order.Quantity;
                order.Tax = product.TaxRate * order.Cost;

                order.Total = order.Cost + order.Tax;

                // Either change the past order order or add order to the
                // Orders context for a new order.
                if (pastOrder != null)
                {
                    // Increment pastOrder properties with the added order.    (Four lines of code)
                    pastOrder.Quantity += order.Quantity;
                    pastOrder.Cost += order.Cost;
                    pastOrder.Tax += order.Tax;
                    pastOrder.Total = pastOrder.Cost + pastOrder.Tax;
                }
                else
                {
                    _context.Orders.Add(order);  // A first-time order for order.productID must be saved
                }
                // Initialize the order Cost and Tax. (Two lines of code)
                cart.Subtotal += order.Cost;
                cart.Tax += order.Tax;

                cart.TotalCost = cart.Subtotal + cart.Tax;

                // Update the database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Carts", new { cartID = order.CartID });
            }
            ViewData["CartID"] = order.CartID;
            ViewData["Quantity"] = availability;
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "ProductID", order.ProductID);
            return View(order);
        }



        /// <summary>
        /// 
        /// Edits an existing order.
        /// 
        /// </summary>
        /// <param name="orderID"> </param>
        /// <returns></returns>
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? orderID)
        {
            if (orderID == null)
            {
                return NotFound();
            }

            // Query the Orders context for the order denoted by orderID.
            var order = await _context.Orders.FindAsync(orderID);
            if (order == null)
            {
                return NotFound();
            }
            // Query the Cart context for the first cart that contains the order.
            var cart = await _context.Carts
                .Include(c => c.Shopper)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CartID == order.CartID)
                .ConfigureAwait(true);

            // To edit a checked out order is not admissible, so go back to previous view.
            if (cart.CheckedOut)
            {
                return RedirectToAction(nameof(Details), "Shoppers", new { shopperID = cart.Shopper.ShopperID });
            }

            // This Edit action pertains to changing the quantity in the earlier created Order 
            Product product = _context.Products.FirstOrDefault(p => p.ProductID == order.ProductID);
            ViewData["ProductAvailable"] = product.Available;

            ViewData["CartID"] = new SelectList(_context.Set<Cart>(), "CartID", "CartID", order.CartID);
            ViewData["ProductID"] = order.ProductID;
            ViewData["ProductName"] = order.Product.Name;

            return View(order);
        }


        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// Continue to edit the Order denoted by orderID
        /// 
        /// </summary>
        /// <param name="orderID"> </param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// 
            //**********************************************
            // FIX THE LOGIC ERROR IN THIS ACTION METHOD !!!
            //**********************************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int orderID, [Bind("OrderID,CartID,ProductID,Quantity")] Order order)
        {
            if (orderID != order.OrderID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Query for the cart that contains the order.
                int cartID = order.CartID;
                Cart cart = _context.Carts.FirstOrDefault(m => m.CartID == cartID);

                // Query for the product that is specified in the order
                int productID = order.ProductID;
                Product product = _context.Products.FirstOrDefault(m => m.ProductID == productID);

                // Query the context for the previous order for this productID and for this cartID
                Order pastOrder = _context.Orders
                    .Include(r => r.Cart)
                    .Include(p => p.Product)
                    .FirstOrDefault(m => m.CartID == cartID && m.ProductID == productID);

                if (cart.Subtotal == null)
                {
                    cart.Subtotal = 0.0M;
                }

                // Adjust the order quantity depending on product availability
                int availability = product.Available;
                if (order.Quantity > availability)
                {
                    order.Quantity = availability;
                }

                // Update the order Cost and Tax properties according to the revised order.Quantity  (Two lines of code)
                order.Cost = (Decimal)product.UnitPrice * order.Quantity;
                order.Tax = product.TaxRate * order.Cost;

                order.Total = order.Cost + order.Tax;

                // SUBTRACT PAST ORDER COST AND TAX FROM CART SUBTOTAL BEFORE UPDATING
                // Decrement cart Subtotal and Tax to remove the pastOrder. (Two lines of code)
                cart.Subtotal -= pastOrder.Cost;
                cart.Tax -= pastOrder.Tax;

                // Revise the pastOrder properties, as altered by the edited order  (Four lines of code)
                pastOrder.Quantity = order.Quantity;
                pastOrder.Cost = order.Cost;
                pastOrder.Tax = order.Tax;
                pastOrder.Total = order.Total;


                // Increment the cart Subtotal and Tax for the edited order. (Two lines of code)
                cart.Subtotal += order.Cost;
                cart.Tax += order.Tax;

                cart.TotalCost = cart.Subtotal + cart.Tax;

                if (order.Quantity == 0)
                {
                    return RedirectToAction(nameof(Delete), "Orders", new { orderID = order.OrderID });
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirect to the Carts Details action method for the user to process another order.
                return RedirectToAction(nameof(Details), "Carts", new { cartID = order.CartID });
            }
            ViewData["CartID"] = new SelectList(_context.Carts, "CartID", "CartID", order.CartID);
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "ProductID", order.ProductID);
            return View(order);
        }



        /// <summary>
        /// 
        /// Deletes the order denoted by orderID.
        /// 
        /// </summary>
        /// <param name="orderID"> </param>
        /// <returns></returns>
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? orderID)
        {
            if (orderID == null)
            {
                return NotFound();
            }

            // Query the Orders context for the order denoted by orderID
            var order = await _context.Orders
                .Include(o => o.Cart)
                .ThenInclude(c => c.Shopper)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderID == orderID);
            if (order == null)
            {
                return NotFound();
            }

            var cart = order.Cart;
            if (cart.CheckedOut)
            {
                // Deleting an order from a checked out cart is not admissible in this application,
                // so go back to the previous view.
                return RedirectToAction(nameof(Details), "Shoppers", new { shopperID = cart.Shopper.ShopperID });
            }

            // Continue the proess to delete the order
            return View(order);
        }


        /// <summary>
        /// 
        ///  Deletes an order denoted by orderID and updates the containing cart
        ///  
        /// </summary>
        /// <param name="orderID"> </param>
        /// <returns></returns>
        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int orderID)
        {
            // Query for the order to be deleted.
            var order = await _context.Orders
                .Include(o => o.Cart)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderID == orderID);

            var cart = order.Cart;
            // Decrement the cart Subtotal and Tax for the deleted order.  (Two lines of code)
            cart.Subtotal -= order.Cost;
            cart.Tax -= order.Tax;

            cart.TotalCost = cart.Subtotal + cart.Tax;

            // Remove the order from the context and update the database
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), "Carts", new { cartID = order.CartID });
        }




        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
