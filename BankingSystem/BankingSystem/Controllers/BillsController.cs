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
    public class BillsController : Controller
    {
        private readonly BankingSystemContext _context;

        public BillsController(BankingSystemContext context)
        {
            _context = context;
        }

        // GET: Bills
        public async Task<IActionResult> Index()
        {
            var bankingSystemContext = _context.Bills.Include(b => b.Account).Include(b => b.Customer);
            return View(await bankingSystemContext.ToListAsync());
        }

        // GET: Bills/Details/5
        public async Task<IActionResult> Details(int? billID)
        {
            if (billID == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .Include(b => b.Account)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BillID == billID);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }



        // GET: Bills/Create
        public IActionResult Create(int? accountID, int? customerID, string? name)
        {
            ViewData["AccountID"] = accountID;
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID");
            ViewData["Name"] = name;
            return View();
        }

        // POST: Bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillID,CustomerID,AccountID,Creditor,Contact,AmountDue,DueDate,Paid,DatePaid,PayerID,ConfirmationNumber")] Bill bill)
        {
            int accountID = (int)bill.AccountID;
            int customerID = (int)bill.CustomerID;
            Account account = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountID == accountID);
            account.Balance -= bill.AmountDue;

            Transaction transaction =
                         new Transaction
                         {
                             AccountID = accountID,
                             TimeSlot = System.DateTime.Now, //DateTime.Parse("2022-04-01"),
                             Action = Transaction.Actions.charge,
                             Amount = -bill.AmountDue,
                             NewBalance = account.Balance
                         };


            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Customers", new { customerID = account.CustomerID });
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", bill.AccountID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", bill.CustomerID);
            return View(bill);
        }



        public async Task<IActionResult> Edit(int? billID)
        {
            if (billID == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.FindAsync(billID);
            if (bill == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", bill.AccountID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", bill.CustomerID);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int billID, [Bind("BillID,CustomerID,AccountID,Creditor,Contact,Description,AmountDue,DueDate,AmountPaid,Paid,DatePaid,PayerID,ConfirmationNumber")] Bill bill)
        {
            if (billID != bill.BillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.BillID))
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", bill.AccountID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", bill.CustomerID);
            return View(bill);
        }


        // GET: Bills/Delete/5
        public async Task<IActionResult> Delete(int? billID)
        {
            if (billID == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .Include(b => b.Account)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BillID == billID);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int billID)
        {
            var bill = await _context.Bills.FindAsync(billID);
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillID == id);
        }


        // GET: Bills/Edit/5
        public async Task<IActionResult> Pay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", bill.AccountID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", bill.CustomerID);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int id, [Bind("BillID,CustomerID,AccountID,Creditor,Contact,AmountDue,DueDate,Paid,DatePaid,PayerID,ConfirmationNumber")] Bill bill)
        {
            if (id != bill.BillID)
            {
                return NotFound();
            }

            int customerID = (int)bill.CustomerID;

            int toAccountID = (int)bill.AccountID;
            Account toAccount = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountID == toAccountID);

            // transfer/pay from some other account
            int fromAccountID = (int)bill.AccountID;
            Account fromAccount = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountID == fromAccountID);

            toAccount.Balance += bill.AmountDue;
            fromAccount.Balance -= bill.AmountDue;
            _context.Accounts.Update(toAccount);
            _context.Accounts.Update(fromAccount);

            Transaction from =
                           new Transaction
                           {
                               AccountID = fromAccountID,
                               TimeSlot = System.DateTime.Now, //DateTime.Parse("2022-04-01"),
                               Action = Transaction.Actions.withdraw,
                               Amount = -bill.AmountDue,
                               NewBalance = fromAccount.Balance + bill.AmountDue
                           };

            Transaction to =
                         new Transaction
                         {
                             AccountID = toAccountID,
                             TimeSlot = System.DateTime.Now, //DateTime.Parse("2022-04-01"),
                             Action = Transaction.Actions.deposit,
                             Amount = -bill.AmountDue,
                             NewBalance = toAccount.Balance + bill.AmountDue
                         };

            _context.Transactions.Add(from);
            _context.Transactions.Add(to);
            _context.SaveChanges();

            Transfer transfer = new Transfer
            {
                Amount = bill.AmountDue,
                AccountID = (int)bill.PayerID,
                ToAccountID = toAccountID,
                TimeSlot = System.DateTime.Now,
            };
            _context.Transfers.Add(transfer);
            _context.SaveChanges();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.BillID))
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", bill.AccountID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", bill.CustomerID);
            return View(bill);
        }



    }
}


