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

        // GET: Transfers
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
            //ViewData["CurrentSort"] = sortOrder;
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransferID == id);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("TransferID,AccountID,ToAccountID,TimeSlot,Amount")] Transfer transfer)
        {
            if (id != transfer.TransferID)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.TransferID == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // POST: Transfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transfer = await _context.Transfers.FindAsync(id);
            _context.Transfers.Remove(transfer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferExists(int id)
        {
            return _context.Transfers.Any(e => e.TransferID == id);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Move(int accountID, int? toAccountID, decimal? amount)
        {
            Account account = _context.Accounts.Find(accountID);
            //int custId = account.CustomerID;
            ViewData["CustomerId"] = account.CustomerID;
            ViewData["AccountID"] = accountID;
            ViewData["ToAccountID"] = toAccountID;
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move([Bind("AccountID,ToAccountID,Amount,AmountPaid,Paid,DatePaid,PayerID")] Transfer transfer)
        {
            if (transfer != null)
            {
                int fromAccountID = transfer.AccountID;
                Account fromAccount = _context.Accounts.FirstOrDefault(m => m.AccountID == fromAccountID);
                //if (fromAccount.Balance > transfer.Amount)
                //{
                //    fromAccount.Balance -= transfer.Amount;
                //}
                //else
                //{
                //    return RedirectToAction(nameof(Details), "Customers", new { customerID = fromAccount.CustomerID });
                //}

                fromAccount.Balance -= transfer.Amount;

                int toAccountID = (int)transfer.ToAccountID;


                Transaction from = new Transaction()
                {
                    AccountID = transfer.AccountID,
                    TimeSlot = System.DateTime.Now,
                    Action = Transaction.Actions.withdraw,
                    Amount = transfer.Amount,
                    NewBalance = fromAccount.Balance
                };
                _context.Transactions.Add(from);

                // Paying a bill with a credit card creates a credit card bill.

                if (from.Account.Kind == Account.Kinds.credit)
                {
                    Bill bill = new Bill
                    {
                        CustomerID = from.Account.CustomerID,
                        AccountID = from.AccountID,
                        AmountDue = transfer.Amount,
                        DueDate = System.DateTime.Now,
                        Creditor = from.Account.Name
                    };
                    _context.Bills.Add(bill);
                }

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


                if (toAccount.Kind == Account.Kinds.credit || toAccount.Kind == Account.Kinds.bill)
                {
                    Bill bill = _context.Bills.FirstOrDefault(b => b.AccountID == toAccountID && !b.Paid);
                    if (bill != null)
                    {
                        if (toAccount.Balance >= 0 || transfer.Amount >= bill.AmountDue)
                        {
                            bill.Paid = true;
                            bill.DatePaid = DateTime.Now;
                            bill.PayerID = from.AccountID;
                            bill.AmountPaid = transfer.Amount;

                            _context.Bills.Update(bill);
                        }
                    }
                }


                if (ModelState.IsValid)
                {
                    transfer.TimeSlot = System.DateTime.Now;

                    _context.Add(transfer);
                    await _context.SaveChangesAsync()
                        .ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Details), "Customers", new { customerID = fromAccount.CustomerID });
            }
            return NotFound();
        }


    }
}

