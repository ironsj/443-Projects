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
using BankingSystem.Services;

namespace BankingSystem.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly BankingSystemContext _context;
        private readonly IBankService _bankService;

        /// <summary>
        /// 
        /// Dependency injection
        /// See the program class for registering the BankService
        /// 
        /// </summary>
        /// <param name="bankService"></param>
        /// <param name="context"></param>
        public TransactionsController(IBankService bankService, BankingSystemContext context)
        {
            _bankService = bankService;
            _context = context;
        }


        // GET: Transactions
        //public async Task<IActionResult> Index()
        //{
        //    var bankingSystemContext = _context.Transactions.Include(t => t.Account);
        //    return View(await bankingSystemContext.ToListAsync());
        //}
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
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SortByAccount"] = String.IsNullOrEmpty(sortOrder) ? "account_descinding" : "";
            ViewData["SortByCustomer"] = sortOrder == "Customer" ? "customer_descinding" : "Customer";

            //  ViewData["EnableFilter"] = enableFilter;

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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID");
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", transaction.AccountID);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", transaction.AccountID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionID,AccountID,TimeSlot,Action,Amount,NewBalance")] Transaction transaction)
        {
            if (id != transaction.TransactionID)
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", transaction.AccountID);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionID == id);
        }

        /// <summary>
        /// Deposit into an account, with primary key accountID, is comparable to the Create parameterless action method
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
                // Identify the account
                int accountID = transaction.AccountID;
                Account account = _context.Accounts
                    .FirstOrDefault(m => m.AccountID == accountID);

                // Updates the account Balance in the context and updates the Transaction.
                account.Balance += (Decimal)transaction.Amount;
                _bankService.Update(Transaction.Actions.deposit, account, transaction);

                if (ModelState.IsValid)
                {
                    // Adds the modified tranaction parameter to the context and update the database
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Index), "Accounts", new { searchString = account.AccountID, enableFilter = false, customerID = account.CustomerID });
            }

            return NotFound();
        }



        /// <summary>
        /// 
        /// Withdraw from an account, with primary key accountID, is comparable to the Create parameterless action method
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public async Task<IActionResult> Withdraw(int accountID)
        {
            Account account = _context.Accounts.Find(accountID);

            ViewData["AccountID"] = accountID;
            ViewData["CustomerID"] = account.CustomerID;
            ViewData["Balance"] = account.Balance;
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
                // Identifies the account via
                int accountID = transaction.AccountID;
                Account account = _context.Accounts.FirstOrDefault(m => m.AccountID == accountID);
                if (transaction.Amount < account.Balance || account.Kind == BankingSystem.Models.Account.Kinds.credit)
                {
                    // Updates the account Balance in the context and updates the Transaction.
                    account.Balance -= (Decimal)transaction.Amount;
                    _bankService.Update(Transaction.Actions.withdraw, account, transaction);

                    if (ModelState.IsValid)
                    {
                        // Adds the modified tranaction parameter to the context and update the database
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
        /// Withdraw from an account, with primary key accountID, is comparable to the Create parameterless action method
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public async Task<IActionResult> Charge(int accountID)
        {
            Account account = _context.Accounts.Find(accountID);

            ViewData["AccountID"] = accountID;
            ViewData["CustomerID"] = account.CustomerID;
            ViewData["Balance"] = account.Balance;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Charge([Bind("AccountID,Amount")] Transaction transaction)
        {
            if (transaction != null)
            {
                // Identifies the account via
                int accountID = transaction.AccountID;
                Account account = _context.Accounts.FirstOrDefault(m => m.AccountID == accountID);

                // Updates the account Balance in the context and updates the Transaction.
                account.Balance -= (Decimal)transaction.Amount;
                _bankService.Update(Transaction.Actions.withdraw, account, transaction);

                if (ModelState.IsValid)
                {
                    // Adds the modified tranaction parameter to the context and update the database
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }

                return RedirectToAction(nameof(Index), "Accounts", new { searchString = account.AccountID, enableFilter = false, customerID = account.CustomerID });
            }

            return NotFound();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddInterest(int accountID)
        {

            var account = await _context.Accounts
                         .FirstOrDefaultAsync(m => m.AccountID == accountID)
                         .ConfigureAwait(true);


            ViewData["CustomerID"] = account.CustomerID;
            ViewData["InterestRate"] = account.InterestRate.ToString("P");

            // transaction.Amount = transaction.Account.Balance * (Decimal)transaction.Account.InterestRate; // Don't change it !!!
            decimal transactionAmount = account.Balance * (Decimal)account.InterestRate; ;
            ViewData["PreviousBalance"] = ((double)account.Balance).ToString("C");
            ViewData["Interest"] = transactionAmount.ToString("C");
            ViewData["AccountID"] = accountID;
            ViewData["PostBalance"] = (((double)account.Balance) + ((double)transactionAmount)).ToString("C");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInterest([Bind("AccountID,Amount")] Transaction transaction)
        {
            // Identifies the account via the transaction parameter.
            if (transaction != null)
            {
                int accountID = transaction.AccountID;
                Account account = _context.Accounts.FirstOrDefault(m => m.AccountID == accountID);

                if (account == null)
                {
                    return NotFound();
                }

                // Assigns the transaction Amount
                transaction.Amount = account.Balance * account.InterestRate;

                // Updates the account Balance in the context and updates the Transaction.
                account.Balance += (Decimal)transaction.Amount;
                _bankService.Update(Transaction.Actions.interest, account, transaction);

                if (ModelState.IsValid)
                {
                    // Adds the modified tranaction parameter to the context and update the database
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Index), "Accounts", new { searchString = account.AccountID, enableFilter = false, customerID = account.CustomerID });
            }

            return NotFound();
        }
    }
}

