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
    public class TransactionsController : Controller
    {
        private readonly BankingSystemContext _context;

        public TransactionsController(BankingSystemContext context)
        {
            _context = context;
        }


        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            bool? enableFilter)
        {
            ViewData["SortByAccount"] = String.IsNullOrEmpty(sortOrder) ? "account_descinding" : "";
            ViewData["SortByCustomer"] = sortOrder == "Customer" ? "customer_descinding" : "Customer";

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

            var transactions = from s in _context.Transactions
                               .Include(s => s.Account)
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Int32 num;
                if (Int32.TryParse(searchString, out num))
                {
                    transactions = transactions.Where(s => s.AccountID.Equals(num));
                }
            }


            switch (sortOrder)
            {
                case "account_descinding":
                    transactions = transactions.OrderByDescending(s => s.AccountID);
                    break;
                case "Customer":
                    transactions = transactions.OrderBy(s => s.Account.CustomerID);
                    break;
                case "customer_descinding":
                    transactions = transactions.OrderByDescending(s => s.Account.CustomerID);
                    break;
                default:
                    transactions = transactions.OrderBy(s => s.AccountID);
                    break;
            }
            return View(await transactions.AsNoTracking().ToListAsync());
        }



        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? transactionID)
        {
            if (transactionID == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransactionID == transactionID);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["AccountID"] = new SelectList(_context.Set<Account>(), "AccountID", "AccountID");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionID,AccountID,TimeSlot,Action,Amount,NewBalance")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountID"] = new SelectList(_context.Set<Account>(), "AccountID", "AccountID", transaction.AccountID);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? transactionID)
        {
            if (transactionID == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(transactionID);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Set<Account>(), "AccountID", "AccountID", transaction.AccountID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int transactionID, [Bind("TransactionID,AccountID,TimeSlot,Action,Amount,NewBalance")] Transaction transaction)
        {
            if (transactionID != transaction.TransactionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionID))
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
            ViewData["AccountID"] = new SelectList(_context.Set<Account>(), "AccountID", "AccountID", transaction.AccountID);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? transactionID)
        {
            if (transactionID == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransactionID == transactionID);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int transactionID)
        {
            var transaction = await _context.Transactions.FindAsync(transactionID);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionID == id);
        }

        /// <summary>
        /// Deposits into the account with primary key accountID. This action is comparable to the Create method.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public async Task<IActionResult> Deposit(int accountID)
        {
            Account account = _context.Accounts.Find(accountID);

            ViewData["CustomerID"] = account.CustomerID;
            ViewData["Balance"] = account.Balance;
            ViewData["AccountID"] = accountID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit([Bind("AccountID,Amount")] Transaction transaction)
        {
            if (transaction != null)
            {
                // Identifies the account into which to make the deposit
                int accountID = transaction.AccountID;
                Account account = _context.Accounts
                    .FirstOrDefault(m => m.AccountID == accountID);

                // Updates the account balance, etc. in the context
                account.Balance += (Decimal)transaction.Amount;
                transaction.NewBalance = account.Balance;
                transaction.TimeSlot = System.DateTime.Now;
                transaction.Action = Transaction.Actions.deposit;

                if (ModelState.IsValid)
                {
                    // Adds the modified tranaction parameter to the context and updates the database
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Index), "Accounts", new { searchString = account.AccountID, enableFilter = false, customerID = account.CustomerID });
            }

            return NotFound();
        }

        /// <summary>
        /// 
        /// Withdraws from the account with primary key accountID. This action is comparable to the Create method.
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public async Task<IActionResult> Withdraw(int accountID)
        {

            // fix this with 4 lines of code
            Account account = _context.Accounts.Find(accountID);

            ViewData["CustomerID"] = account.CustomerID;
            ViewData["Balance"] = account.Balance;
            ViewData["AccountID"] = accountID;

            // fix the Razor code in the Withdraw View , whoch should be similar to that for the Deposit View.

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw([Bind("AccountID,Amount")] Transaction transaction)
        {
            if (transaction != null)
            {
                // Fix this with code similar to Deposit.  If the transaction amount is greater that the account balance, no changes are to be made.
                // Identifies the account into which to make the withdrawal
                int accountID = transaction.AccountID;
                Account account = _context.Accounts
                    .FirstOrDefault(m => m.AccountID == accountID);

                // Updates the account balance, etc. in the context
                if(account.Balance >= transaction.Amount)
                {
                    account.Balance -= (Decimal)transaction.Amount;
                    transaction.NewBalance = account.Balance;
                    transaction.TimeSlot = System.DateTime.Now;
                    transaction.Action = Transaction.Actions.withdraw;

                    if (ModelState.IsValid)
                    {
                        // Adds the modified tranaction parameter to the context and updates the database
                        _context.Transactions.Add(transaction);
                        await _context.SaveChangesAsync().ConfigureAwait(true);
                    }
                }
                

                
                return RedirectToAction(nameof(Index), "Accounts", new { searchString = account.AccountID, enableFilter = false, customerID = account.CustomerID });
            }

            return NotFound();
        }

        /// <summary>
        /// 
        /// Adds interest to the account with primary key accountID. This action is comparable to the Create method.
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInterest(int accountID)
        {
            var account = await _context.Accounts
                         .FirstOrDefaultAsync(m => m.AccountID == accountID)
                         .ConfigureAwait(true);

            decimal transactionAmount = account.Balance * (Decimal)account.InterestRate; ;

            ViewData["CustomerID"] = account.CustomerID;
            ViewData["AccountID"] = accountID;
            ViewData["PreviousBalance"] = ((double)account.Balance).ToString("C");
            ViewData["InterestRate"] = account.InterestRate.ToString("P");
            ViewData["Interest"] = transactionAmount.ToString("C");
            ViewData["PostBalance"] = (((double)account.Balance) + ((double)transactionAmount)).ToString("C");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInterest([Bind("AccountID,Amount")] Transaction transaction)
        {
            if (transaction != null)
            {
                // Identifies the account for the transaction
                int accountID = transaction.AccountID;
                Account account = _context.Accounts.FirstOrDefault(m => m.AccountID == accountID);

                if (account == null)
                {
                    return NotFound();
                }

                // Updates the account balance, etc. in the context
                transaction.Amount = account.Balance * account.InterestRate;
                account.Balance *= 1 + account.InterestRate;
                transaction.NewBalance = account.Balance;
                transaction.TimeSlot = System.DateTime.Now;
                transaction.Action = Transaction.Actions.interest;

                if (ModelState.IsValid)
                {
                    // Adds the modified transaction to the context and updates the database
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Index), "Accounts", new { searchString = account.AccountID, enableFilter = false, customerID = account.CustomerID });
            }

            return NotFound();
        }

    }
}
