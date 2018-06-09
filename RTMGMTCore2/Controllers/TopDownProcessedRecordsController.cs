using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RTMGMTCore2.Models;
using Microsoft.AspNetCore.Hosting;

namespace RTMGMTCore2.Controllers
{
    public class TopDownProcessedRecordsController : Controller
    {
        private readonly RTMGMTContext _context;

        public TopDownProcessedRecordsController(RTMGMTContext context)
        {
            _context = context;
        }

        //// GET: TopDownProcessedRecords
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.TopDownProcessedRecords.ToListAsync());
        //}

        // GET: ReportsToRecords
        public IActionResult Index()
        {
            //return View(await _context.ReportsToRecords.ToListAsync());
            return View();
        }


        [HttpPost]
        public JsonResult TDPRList(DTParameters param)
        {

            int TotalCount = 0;
            var filtered = this.GetTDPRFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out TotalCount);

            var TDPRList = filtered.Select(o => new TopDownProcessedRecords()
            {

                Id = o.Id,
                ReportingId = o.ReportingId,
                Title = o.Title,
                Name = o.Name,
                ReportLevel=o.ReportLevel,
                ReportsToId = o.ReportsToId,
                RtString=o.RtString,
                EmployeeId = o.EmployeeId
            });



            DTResult<TopDownProcessedRecords> finalresult = new DTResult<TopDownProcessedRecords>
            {
                draw = param.Draw,
                data = TDPRList.ToList(),
                recordsFiltered = TotalCount,
                recordsTotal = filtered.Count

            };
            return Json(finalresult);

        }



        // GET: TopDownProcessedRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topDownProcessedRecords = await _context.TopDownProcessedRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (topDownProcessedRecords == null)
            {
                return NotFound();
            }

            return View(topDownProcessedRecords);
        }

        // GET: TopDownProcessedRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TopDownProcessedRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportingId,Title,Name,ReportLevel,ReportsToId,RtString,EmployeeId")] TopDownProcessedRecords topDownProcessedRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topDownProcessedRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topDownProcessedRecords);
        }

        // GET: TopDownProcessedRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topDownProcessedRecords = await _context.TopDownProcessedRecords.SingleOrDefaultAsync(m => m.Id == id);
            if (topDownProcessedRecords == null)
            {
                return NotFound();
            }
            return View(topDownProcessedRecords);
        }

        // POST: TopDownProcessedRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportingId,Title,Name,ReportLevel,ReportsToId,RtString,EmployeeId")] TopDownProcessedRecords topDownProcessedRecords)
        {
            if (id != topDownProcessedRecords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topDownProcessedRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopDownProcessedRecordsExists(topDownProcessedRecords.Id))
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
            return View(topDownProcessedRecords);
        }

        // GET: TopDownProcessedRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topDownProcessedRecords = await _context.TopDownProcessedRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (topDownProcessedRecords == null)
            {
                return NotFound();
            }

            return View(topDownProcessedRecords);
        }

        // POST: TopDownProcessedRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topDownProcessedRecords = await _context.TopDownProcessedRecords.SingleOrDefaultAsync(m => m.Id == id);
            _context.TopDownProcessedRecords.Remove(topDownProcessedRecords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopDownProcessedRecordsExists(int id)
        {
            return _context.TopDownProcessedRecords.Any(e => e.Id == id);
        }

        //for server side processing

        public List<TopDownProcessedRecords> GetTDPRFiltered(string search, string sortOrder, int start, int length, out int TotalCount)
        {


            var result = _context.TopDownProcessedRecords.Where(p => (search == null
                || (p.ReportingId != null && p.ReportingId.ToLower().Contains(search.ToLower())
                || p.Title != null && p.Title.ToLower().Contains(search.ToLower())
                || p.Name != null && p.Name.ToLower().Contains(search.ToLower())
                || p.ReportLevel != null && p.ReportLevel.ToString().Contains(search.ToString())
                || p.ReportsToId != null && p.ReportsToId.ToLower().Contains(search.ToLower())
                || p.RtString != null && p.RtString.ToLower().Contains(search.ToLower())
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
                case "ReportLevel":
                    result = result.OrderBy(a => a.ReportLevel).ToList();
                    break;
                case "ReportsToId":
                    result = result.OrderBy(a => a.ReportsToId).ToList();
                    break;
                case "RtString":
                    result = result.OrderBy(a => a.RtString).ToList();
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
