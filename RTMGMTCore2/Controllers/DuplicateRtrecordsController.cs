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
    public class DuplicateRtrecordsController : Controller
    {
        private readonly RTMGMTContext _context;

        public DuplicateRtrecordsController(RTMGMTContext context)
        {
            _context = context;
        }

        //// GET: DuplicateRtrecords
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.DuplicateRtrecords.ToListAsync());
        //}

        // GET: ReportsToRecords
        public IActionResult Index()
        {
            //return View(await _context.ReportsToRecords.ToListAsync());
            return View();
        }

        [HttpPost]
        public JsonResult DRList(DTParameters param)
        {

            int TotalCount = 0;
            var filtered = this.GetDRFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out TotalCount);

            var DRList = filtered.Select(o => new DuplicateRtrecords()
            {

                Id = o.Id,
                ReportingId = o.ReportingId,
                Title = o.Title,
                Name = o.Name,
                ReportsToId = o.ReportsToId,
                EmployeeId = o.EmployeeId
            });



            DTResult<DuplicateRtrecords> finalresult = new DTResult<DuplicateRtrecords>
            {
                draw = param.Draw,
                data = DRList.ToList(),
                recordsFiltered = TotalCount,
                recordsTotal = filtered.Count

            };
            return Json(finalresult);

        }



        // GET: DuplicateRtrecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duplicateRtrecords = await _context.DuplicateRtrecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (duplicateRtrecords == null)
            {
                return NotFound();
            }

            return View(duplicateRtrecords);
        }

        // GET: DuplicateRtrecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DuplicateRtrecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportingId,Title,Name,ReportsToId,EmployeeId")] DuplicateRtrecords duplicateRtrecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(duplicateRtrecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(duplicateRtrecords);
        }

        // GET: DuplicateRtrecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duplicateRtrecords = await _context.DuplicateRtrecords.SingleOrDefaultAsync(m => m.Id == id);
            if (duplicateRtrecords == null)
            {
                return NotFound();
            }
            return View(duplicateRtrecords);
        }

        // POST: DuplicateRtrecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportingId,Title,Name,ReportsToId,EmployeeId")] DuplicateRtrecords duplicateRtrecords)
        {
            if (id != duplicateRtrecords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(duplicateRtrecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DuplicateRtrecordsExists(duplicateRtrecords.Id))
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
            return View(duplicateRtrecords);
        }

        // GET: DuplicateRtrecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duplicateRtrecords = await _context.DuplicateRtrecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (duplicateRtrecords == null)
            {
                return NotFound();
            }

            return View(duplicateRtrecords);
        }

        // POST: DuplicateRtrecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var duplicateRtrecords = await _context.DuplicateRtrecords.SingleOrDefaultAsync(m => m.Id == id);
            _context.DuplicateRtrecords.Remove(duplicateRtrecords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DuplicateRtrecordsExists(int id)
        {
            return _context.DuplicateRtrecords.Any(e => e.Id == id);
        }

        //for server side processing

        public List<DuplicateRtrecords> GetDRFiltered(string search, string sortOrder, int start, int length, out int TotalCount)
        {


            var result = _context.DuplicateRtrecords.Where(p => (search == null
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
