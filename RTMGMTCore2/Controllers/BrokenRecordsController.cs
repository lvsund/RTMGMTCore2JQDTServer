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
    public class BrokenRecordsController : Controller
    {
        private readonly RTMGMTContext _context;

        public BrokenRecordsController(RTMGMTContext context)
        {
            _context = context;
        }

        //// GET: BrokenRecords
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.BrokenRecords.ToListAsync());
        //}

        // GET: ReportsToRecords
        public IActionResult Index()
        {
            //return View(await _context.ReportsToRecords.ToListAsync());
            return View();
        }


        [HttpPost]
        public JsonResult BRList(DTParameters param)
        {

            int TotalCount = 0;
            var filtered = this.GetBRFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out TotalCount);

            var BRList = filtered.Select(o => new BrokenRecords()
            {

                Id = o.Id,
                ReportingId = o.ReportingId,
                Title = o.Title,
                Name = o.Name,
                ReportsToId = o.ReportsToId,
                EmployeeId = o.EmployeeId
            });



            DTResult<BrokenRecords> finalresult = new DTResult<BrokenRecords>
            {
                draw = param.Draw,
                data = BRList.ToList(),
                recordsFiltered = TotalCount,
                recordsTotal = filtered.Count

            };
            return Json(finalresult);

        }


        // GET: BrokenRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brokenRecords = await _context.BrokenRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (brokenRecords == null)
            {
                return NotFound();
            }

            return View(brokenRecords);
        }

        // GET: BrokenRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BrokenRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportingId,Title,Name,ReportsToId,RtString,EmployeeId")] BrokenRecords brokenRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brokenRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brokenRecords);
        }

        // GET: BrokenRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brokenRecords = await _context.BrokenRecords.SingleOrDefaultAsync(m => m.Id == id);
            if (brokenRecords == null)
            {
                return NotFound();
            }
            return View(brokenRecords);
        }

        // POST: BrokenRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportingId,Title,Name,ReportsToId,RtString,EmployeeId")] BrokenRecords brokenRecords)
        {
            if (id != brokenRecords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brokenRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrokenRecordsExists(brokenRecords.Id))
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
            return View(brokenRecords);
        }

        // GET: BrokenRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brokenRecords = await _context.BrokenRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (brokenRecords == null)
            {
                return NotFound();
            }

            return View(brokenRecords);
        }

        // POST: BrokenRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brokenRecords = await _context.BrokenRecords.SingleOrDefaultAsync(m => m.Id == id);
            _context.BrokenRecords.Remove(brokenRecords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrokenRecordsExists(int id)
        {
            return _context.BrokenRecords.Any(e => e.Id == id);
        }

        //for server side processing

        public List<BrokenRecords> GetBRFiltered(string search, string sortOrder, int start, int length, out int TotalCount)
        {


            var result = _context.BrokenRecords.Where(p => (search == null
                || (p.ReportingId != null && p.ReportingId.ToLower().Contains(search.ToLower())
                || p.Title != null && p.Title.ToLower().Contains(search.ToLower())
                || p.Name != null && p.Name.ToLower().Contains(search.ToLower())
                || p.ReportsToId != null && p.ReportsToId.ToLower().Contains(search.ToLower())
                || p.EmployeeId != null && p.EmployeeId.ToLower().Contains(search.ToLower())
                ))).ToList();

            TotalCount = result.Count;

            result = result.Skip(start).Take(length).ToList();


            switch (sortOrder)
            {
                case "ReportingId":
                    result = result.OrderBy(a => a.ReportingId).ToList();
                    break;
                case "Title":
                    result = result.OrderBy(a => a.Title).ToList();
                    break;
                case "Name":
                    result = result.OrderBy(a => a.Name).ToList();
                    break;
                case "ReportsToId":
                    result = result.OrderBy(a => a.ReportsToId).ToList();
                    break;
                case "EmployeeId":
                    result = result.OrderBy(a => a.EmployeeId).ToList();
                    break;
                default:
                    result = result.AsQueryable().ToList();
                    break;
            }
            return result.ToList();
        }

    }
}
