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
    public class FixRecordsController : Controller
    {
        private readonly RTMGMTContext _context;

        public FixRecordsController(RTMGMTContext context)
        {
            _context = context;
        }

        //// GET: FixRecords
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.FixRecords.ToListAsync());
        //}

        // GET: ReportsToRecords
        public IActionResult Index()
        {
            //return View(await _context.ReportsToRecords.ToListAsync());
            return View();
        }

        [HttpPost]
        public JsonResult FRList(DTParameters param)
        {

            int TotalCount = 0;
            var filtered = this.GetFRFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out TotalCount);

            var RTRList = filtered.Select(o => new FixRecords()
            {

                Id = o.Id,
                ReportingId = o.ReportingId,
                Title = o.Title,
                Name = o.Name,
                ReportsToId = o.ReportsToId,
                EmployeeId = o.EmployeeId
            });



            DTResult<FixRecords> finalresult = new DTResult<FixRecords>
            {
                draw = param.Draw,
                data = RTRList.ToList(),
                recordsFiltered = TotalCount,
                recordsTotal = filtered.Count

            };
            return Json(finalresult);

        }


        // GET: FixRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixRecords = await _context.FixRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fixRecords == null)
            {
                return NotFound();
            }

            return View(fixRecords);
        }

        // GET: FixRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FixRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportingId,Title,Name,ReportsToId,RtString,EmployeeId")] FixRecords fixRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fixRecords);
        }

        // GET: FixRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixRecords = await _context.FixRecords.SingleOrDefaultAsync(m => m.Id == id);
            if (fixRecords == null)
            {
                return NotFound();
            }
            return View(fixRecords);
        }

        // POST: FixRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportingId,Title,Name,ReportsToId,RtString,EmployeeId")] FixRecords fixRecords)
        {
            if (id != fixRecords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixRecordsExists(fixRecords.Id))
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
            return View(fixRecords);
        }

        // GET: FixRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixRecords = await _context.FixRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fixRecords == null)
            {
                return NotFound();
            }

            return View(fixRecords);
        }

        // POST: FixRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fixRecords = await _context.FixRecords.SingleOrDefaultAsync(m => m.Id == id);
            _context.FixRecords.Remove(fixRecords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FixRecordsExists(int id)
        {
            return _context.FixRecords.Any(e => e.Id == id);
        }

        //for server side processing

        public List<FixRecords> GetFRFiltered(string search, string sortOrder, int start, int length, out int TotalCount)
        {


            var result = _context.FixRecords.Where(p => (search == null
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
