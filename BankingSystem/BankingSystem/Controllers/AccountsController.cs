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
    public class AccountsController : Controller
    {
        private readonly BankingSystemContext _context;

        public AccountsController(BankingSystemContext context)
        {
            _context = context;
        }


        // GET: Accounts
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="enableFilter">false => no filtering, e.g. list only</param>
        /// <param name="customerID"> allows for passing customerID as route data for navigation to Customers Details/param>
        /// <returns></returns>
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            bool? enableFilter,
            int? customerID)
        {

            ViewData["SortByAccount"] = String.IsNullOrEmpty(sortOrder) ? "account_descinding" : "";
            ViewData["SortByCustomer"] = sortOrder == "Customer" ? "customer_descinding" : "Customer";
            ViewData["CustomerID"] = customerID;

            if (enableFilter == null)
            {
                ViewBag.EnableFilter = true;
            }
            else
            {
                ViewBag.EnableFilter = enableFilter;
            }

            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;


            var accounts = from s in _context.Accounts
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Int32 num;
                if (Int32.TryParse(searchString, out num))
                {
                    accounts = accounts.Where(s => s.AccountID.Equals(num));
                }
            }
            accounts = accounts.Include(s => s.Customer);
            ViewData["MyAccounts"] = new SelectList(await accounts.Distinct().ToListAsync());

            // Filtering can be disabled by invoking code.
            if (ViewBag.EnableFilter)
            {
                switch (sortOrder)
                {
                    case "account_descinding":
                        accounts = accounts.OrderByDescending(s => s.AccountID);
                        break;
                    case "Customer":
                        accounts = accounts.OrderBy(s => s.CustomerID);
                        break;
                    case "customer_descinding":
                        accounts = accounts.OrderByDescending(s => s.CustomerID);
                        break;
                    default:
                        accounts = accounts.OrderBy(s => s.AccountID);
                        break;
                }
            }

            return View(await accounts.AsNoTracking().ToListAsync());
        }



        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }


        // GET: Accounts/Create/customerID
        public IActionResult Create(int? customerID)
        {
            System.Collections.IList collection = new System.Collections.ArrayList() { "checking", "savings", "credit", "debit", "bill", "other" };
            ViewData["CustomerID"] = customerID;
            ViewData["Kind"] = new SelectList(collection, "checking");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountID,CustomerID,AccountDate,Name,Kind,Balance,InterestRate")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.AccountDate = System.DateTime.Now;
                account.Balance = 0;

                _context.Accounts.Add(account);                              // _context.Add(account);
                await _context.SaveChangesAsync().ConfigureAwait(true);     // await _context.SaveChangesAsync();


                /********************************************************************************************
                 *
                 * The following code creates an initial Transaction, an Action for a deposit, to initialize
                 * the newly created Account with a balance of $0.00. 
                 * 
                 * ******************************************************************************************/
                Transaction t = new Transaction
                {
                    AccountID = account.AccountID,
                    TimeSlot = account.AccountDate,
                    Action = Transaction.Actions.deposit,
                    Amount = 0.0M,
                    NewBalance = 0.0M
                };
                _context.Add(t);
                await _context.SaveChangesAsync().ConfigureAwait(true);

                return RedirectToAction(nameof(Details), "Customers", new { customerID = account.CustomerID });
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", account.CustomerID);
            return View(account);
        }


        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? accountID)
        {
            if (accountID == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(accountID);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", account.CustomerID);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int accountID, [Bind("AccountID,CustomerID,AccountDate,Name,Kind,Balance,InterestRate")] Account account)
        {
            if (accountID != account.AccountID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", account.CustomerID);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? accountID)
        {
            if (accountID == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(m => m.AccountID == accountID);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int accountID)
        {
            var account = await _context.Accounts.FindAsync(accountID);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountID == id);
        }


    }
}
