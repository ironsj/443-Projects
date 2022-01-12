#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyChecklist.Data;
using MyChecklist.Models;

namespace MyChecklist.Controllers
{
    public class ChecksController : Controller
    {
        private readonly MyChecklistContext _context;

        public ChecksController(MyChecklistContext context)
        {
            _context = context;
        }

        // GET: Checks
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Checks.ToListAsync());
        //}
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var checks = from s in _context.Checks
                         select s;
            //           if (!String.IsNullOrEmpty(searchString))
            //           {
            //               checks = checks.Where(s => s.Assignment.Contains(searchString));
            //           }
            if (!String.IsNullOrEmpty(searchString))
            {
                checks = checks.Where(s => s.Unit.ToString().Equals(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    //checks = checks.OrderByDescending(s => s.Assignment);
                    checks = checks.OrderByDescending(s => s.Unit);
                    break;
                case "Date":
                    checks = checks.OrderBy(s => s.DueDate);
                    break;
                case "date_desc":
                    checks = checks.OrderByDescending(s => s.DueDate);
                    break;
                default:
                    //checks = checks.OrderBy(s => s.Assignment);
                    checks = checks.OrderBy(s => s.Unit);
                    break;
            }
            return View(await checks.AsNoTracking().ToListAsync());
        }





        // GET: Checks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Checks
                .FirstOrDefaultAsync(m => m.CheckID == id);
            if (check == null)
            {
                return NotFound();
            }

            return View(check);
        }

        // GET: Checks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Checks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CheckID,IsDone,Unit,Topic,Task,DueDate,HoursInvested,Feature")] Check check)
        {
            if (ModelState.IsValid)
            {
                _context.Add(check);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(check);
        }

        // GET: Checks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Checks.FindAsync(id);
            if (check == null)
            {
                return NotFound();
            }
            return View(check);
        }

        // POST: Checks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CheckID,IsDone,Unit,Topic,Task,DueDate,HoursInvested,Feature")] Check check)
        {
            if (id != check.CheckID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(check);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckExists(check.CheckID))
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
            return View(check);
        }

        // GET: Checks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Checks
                .FirstOrDefaultAsync(m => m.CheckID == id);
            if (check == null)
            {
                return NotFound();
            }

            return View(check);
        }

        // POST: Checks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var check = await _context.Checks.FindAsync(id);
            _context.Checks.Remove(check);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckExists(int id)
        {
            return _context.Checks.Any(e => e.CheckID == id);
        }
    }
}
