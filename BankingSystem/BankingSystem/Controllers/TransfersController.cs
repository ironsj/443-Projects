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
    public class TransfersController : Controller
    {
        private readonly BankingSystemContext _context;

        public TransfersController(BankingSystemContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="enableFilter"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
             bool? enableFilter)
        {
            ViewData["SsortByAccount"] = String.IsNullOrEmpty(sortOrder) ? "account_descinding" : "";
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


            var transfers = from s in _context.Transfers
                            .Include(t => t.Account)
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Int32 num;
                if (Int32.TryParse(searchString, out num))
                {
                    transfers = transfers.Where(s => s.AccountID.Equals(num));
                }
            }


            switch (sortOrder)
            {
                case "account_descinding":
                    transfers = transfers.OrderByDescending(s => s.AccountID);
                    break;
                case "Customer":
                    transfers = transfers.OrderBy(s => s.Account.CustomerID);
                    break;
                case "customer_descinding":
                    transfers = transfers.OrderByDescending(s => s.Account.CustomerID);
                    break;
                default:
                    transfers = transfers.OrderBy(s => s.AccountID);
                    break;
            }

            return View(await transfers.AsNoTracking().ToListAsync());
        }




        // GET: Transfers/Details/5
        public async Task<IActionResult> Details(int? transferID)
        {
            if (transferID == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransferID == transferID);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // GET: Transfers/Create
        public IActionResult Create()
        {
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID");
            return View();
        }

        // POST: Transfers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransferID,AccountID,ToAccountID,TimeSlot,Amount")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", transfer.AccountID);
            return View(transfer);
        }

        // GET: Transfers/Edit/5
        public async Task<IActionResult> Edit(int? transferID)
        {
            if (transferID == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers.FindAsync(transferID);
            if (transfer == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", transfer.AccountID);
            return View(transfer);
        }

        // POST: Transfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int transferID, [Bind("TransferID,AccountID,ToAccountID,TimeSlot,Amount")] Transfer transfer)
        {
            if (transferID != transfer.TransferID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferExists(transfer.TransferID))
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", transfer.AccountID);
            return View(transfer);
        }

        // GET: Transfers/Delete/5
        public async Task<IActionResult> Delete(int? transferID)
        {
            if (transferID == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransferID == transferID);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // POST: Transfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int transferID)
        {
            var transfer = await _context.Transfers.FindAsync(transferID);
            _context.Transfers.Remove(transfer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferExists(int transferID)
        {
            return _context.Transfers.Any(e => e.TransferID == transferID);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Move(int accountID)
        {
            var transaction = await _context.Accounts   //Debit
                         .FirstOrDefaultAsync(m => m.AccountID == accountID)
                         .ConfigureAwait(true);

            Account account = _context.Accounts.Find(accountID);
            int custId = account.CustomerID;

            var accounts = from s in _context.Accounts
                           .Where(a => a.CustomerID == custId)
                           select s;

            ViewData["Balance"] = account.Balance;
            ViewData["CustomerId"] = account.CustomerID;
            ViewData["AccountID"] = accountID;
            ViewBag.MyAccounts = new SelectList(accounts, "AccountID", "AccountID");
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move([Bind("AccountID,ToAccountID,Amount")] Transfer transfer)
        {
            if (transfer != null)
            {
                // Identifies the account from which to transfer the transfer amount and handles the transaction with the from account
                int accountID = transfer.AccountID;
                Account account = _context.Accounts.FirstOrDefault(m => m.AccountID == accountID);
                if (account.Balance < transfer.Amount)
                {
                    return RedirectToAction(nameof(Details), "Customers", new { customerID = account.CustomerID });
                }
                else
                {
                    account.Balance -= transfer.Amount;
                }
                Transaction from = new Transaction()
                {
                    AccountID = transfer.AccountID,
                    TimeSlot = System.DateTime.Now,
                    Action = Transaction.Actions.withdraw,
                    Amount = transfer.Amount,
                    NewBalance = account.Balance
                };
                _context.Transactions.Add(from);


                // Identifies the account to which to transfer the transfer amount and handles the transaction with the to account
                int toAccountID = (int)transfer.ToAccountID;
                Account toAccount = _context.Accounts.FirstOrDefault(m => m.AccountID == toAccountID);
                toAccount.Balance += transfer.Amount;
                Transaction to = new Transaction()
                {
                    AccountID = (int)transfer.ToAccountID,
                    TimeSlot = System.DateTime.Now,
                    Action = Transaction.Actions.deposit,
                    Amount = transfer.Amount,
                    NewBalance = toAccount.Balance
                };
                _context.Transactions.Add(to);


                // Update the database
                if (ModelState.IsValid)
                {
                    transfer.TimeSlot = System.DateTime.Now;

                    _context.Add(transfer);
                    await _context.SaveChangesAsync()
                        .ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Details), "Customers", new { customerID = account.CustomerID });
            }
            return NotFound();
        }

    }
}
