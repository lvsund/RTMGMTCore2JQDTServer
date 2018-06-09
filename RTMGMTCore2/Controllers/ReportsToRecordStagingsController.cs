using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RTMGMTCore2.Models;

namespace RTMGMTCore2.Controllers
{
    public class ReportsToRecordStagingsController : Controller
    {
        private readonly RTMGMTContext _context;

        public ReportsToRecordStagingsController(RTMGMTContext context)
        {
            _context = context;
        }

        // GET: ReportsToRecordStagings
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReportsToRecordStagings.ToListAsync());
        }

        // GET: ReportsToRecordStagings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsToRecordStagings = await _context.ReportsToRecordStagings
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reportsToRecordStagings == null)
            {
                return NotFound();
            }

            return View(reportsToRecordStagings);
        }

        // GET: ReportsToRecordStagings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReportsToRecordStagings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportingId,Title,Name,ReportsToId,EmployeeId")] ReportsToRecordStagings reportsToRecordStagings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportsToRecordStagings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reportsToRecordStagings);
        }

        // GET: ReportsToRecordStagings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsToRecordStagings = await _context.ReportsToRecordStagings.SingleOrDefaultAsync(m => m.Id == id);
            if (reportsToRecordStagings == null)
            {
                return NotFound();
            }
            return View(reportsToRecordStagings);
        }

        // POST: ReportsToRecordStagings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportingId,Title,Name,ReportsToId,EmployeeId")] ReportsToRecordStagings reportsToRecordStagings)
        {
            if (id != reportsToRecordStagings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportsToRecordStagings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportsToRecordStagingsExists(reportsToRecordStagings.Id))
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
            return View(reportsToRecordStagings);
        }

        // GET: ReportsToRecordStagings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsToRecordStagings = await _context.ReportsToRecordStagings
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reportsToRecordStagings == null)
            {
                return NotFound();
            }

            return View(reportsToRecordStagings);
        }

        // POST: ReportsToRecordStagings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportsToRecordStagings = await _context.ReportsToRecordStagings.SingleOrDefaultAsync(m => m.Id == id);
            _context.ReportsToRecordStagings.Remove(reportsToRecordStagings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportsToRecordStagingsExists(int id)
        {
            return _context.ReportsToRecordStagings.Any(e => e.Id == id);
        }
    }
}
