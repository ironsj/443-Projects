#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Data;
using BankingSystem.Models;

namespace BankingSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly BankingSystemContext _context;

        public CustomersController(BankingSystemContext context)
        {
            _context = context;
        }


        // GET: Customers
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

            ViewData["SortByName"] = String.IsNullOrEmpty(sortOrder) ? "name_descending" : "";

            // The following ternary toggles SortByCustomerID in the data dictionary
            ViewData["SortByCustomer"] = sortOrder == "Customer" ? "customer_descending" : "Customer";

            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;


            var customers = from s in _context.Customers
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                // Search by Email
                customers = customers.Where(s => s.Email.Equals(searchString));

                // Search by Birthday, another example
                //    customers = customers.Where(s => s.Birthday.Date.Equals(DateTime.Parse(searchString, null).Date));
            }

            // Determine an order for the customers collection
            switch (sortOrder)
            {
                case "name_descending":
                    customers = customers.OrderByDescending(s => s.LastName);
                    break;
                case "Customer":
                    customers = customers.OrderBy(s => s.CustomerID);
                    break;
                case "customer_descending":
                    customers = customers.OrderByDescending(s => s.CustomerID);
                    break;
                default:
                    customers = customers.OrderBy(s => s.LastName);
                    break;
            }

            // return the View for the customers with determined order
            return View(await customers.AsNoTracking().ToListAsync());
        }




        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? customerID, bool? bills, bool? allBills, int? toAccountID, decimal? amountDue)
        {
            if (customerID == null)
            {
                return NotFound();
            }

            //Query the context for the customer with the primary key equal to the parameter customerID
            var customer = await _context.Customers
              .Include(s => s.Accounts)                    // added after scaffolding
              .Include(b => b.Bills)
              .AsNoTracking()                              // added after scaffolding
              .FirstOrDefaultAsync(m => m.CustomerID == customerID);

            if (customer == null)
            {
                return NotFound();
            }

            if (bills == null)
            {
                ViewData["Bills"] = false;
            }
            else
            {
                ViewData["Bills"] = bills;
            }

            if (allBills == null)
            {
                ViewData["AllBills"] = false;
            }
            else
            {
                ViewData["AllBills"] = allBills;
            }

            ViewData["AmoutDue"] = amountDue;
            ViewData["ToAccountID"] = toAccountID;

            return View(customer);
        }



        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,LastName,FirstName,Birthday,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? customerID)
        {
            if (customerID == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(customerID);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int customerID, [Bind("CustomerID,LastName,FirstName,Birthday,Email")] Customer customer)
        {
            if (customerID != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? customerID)
        {
            if (customerID == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == customerID);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int customerID)
        {
            var customer = await _context.Customers.FindAsync(customerID);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }

        public async Task<IActionResult> GetTransfer(int? customerID, int? toAccountID)
        {
            if (customerID == null)
            {
                return NotFound();
            }

            //Query the context for the customer with the primary key equal to the parameter customerID
            var customer = await _context.Customers
              .Include(s => s.Accounts)                                 // added after scaffolding
              .Include(b => b.Bills)                                    // added after scaffolding
              .AsNoTracking()                                           // added after scaffolding
              .FirstOrDefaultAsync(m => m.CustomerID == customerID);

            if (customer == null)
            {
                return NotFound();
            }

 
            var toAccount = await _context.Accounts
              .AsNoTracking()
              .FirstOrDefaultAsync(m => m.AccountID == toAccountID);
            if (toAccount.Balance < 0)
            {
                ViewData["Payment"] = (decimal)-toAccount.Balance;
            }
            else
            {
                ViewData["Payment"] = 0;
            }

            ViewData["ToAccountID"] = toAccountID;

            
            if (toAccount.Kind == Account.Kinds.checking || toAccount.Kind == Account.Kinds.savings)
            {
                ViewData["Filter"] = 1;
            }
            else if (toAccount.Kind == Account.Kinds.credit || toAccount.Kind == Account.Kinds.debit)

            {
                ViewData["Filter"] = 2;
            }
            else if (toAccount.Kind == Account.Kinds.bill || toAccount.Kind == Account.Kinds.other)

            {
                ViewData["Filter"] = 3;
            }
            else
            {
                ViewData["Filter"] = 0;
            }


            return View(customer);
        }

    }
}
