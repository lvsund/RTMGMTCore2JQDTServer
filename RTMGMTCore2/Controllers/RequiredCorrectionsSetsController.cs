using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RTMGMTCore2.Models;
using CsvHelper;
using System.IO;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace RTMGMTCore2.Controllers
{
    public class RequiredCorrectionsSetsController : Controller
    {
        private readonly RTMGMTContext _context;

        public RequiredCorrectionsSetsController(RTMGMTContext context)
        {
            _context = context;
        }

        //// GET: RequiredCorrectionsSets
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.RequiredCorrectionsSet.ToListAsync());
        //}

        // GET: ReportsToRecords
        public IActionResult Index()
        {
            //return View(await _context.ReportsToRecords.ToListAsync());
            return View();
        }


        [HttpPost]
        public JsonResult RCList(DTParameters param)
        {

            int TotalCount = 0;
            var filtered = this.GetRCFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out TotalCount);

            var RCList = filtered.Select(o => new RequiredCorrectionsSet()
            {

                Id = o.Id,
                ReportingId = o.ReportingId,

                ReportsToId = o.ReportsToId,

            });



            DTResult<RequiredCorrectionsSet> finalresult = new DTResult<RequiredCorrectionsSet>
            {
                draw = param.Draw,
                data = RCList.ToList(),
                recordsFiltered = TotalCount,
                recordsTotal = filtered.Count

            };
            return Json(finalresult);

        }


        // GET: RequiredCorrectionsSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requiredCorrectionsSet = await _context.RequiredCorrectionsSet
                .SingleOrDefaultAsync(m => m.Id == id);
            if (requiredCorrectionsSet == null)
            {
                return NotFound();
            }

            return View(requiredCorrectionsSet);
        }

        // GET: RequiredCorrectionsSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RequiredCorrectionsSets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportingId,ReportsToId")] RequiredCorrectionsSet requiredCorrectionsSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requiredCorrectionsSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requiredCorrectionsSet);
        }

        // GET: RequiredCorrectionsSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requiredCorrectionsSet = await _context.RequiredCorrectionsSet.SingleOrDefaultAsync(m => m.Id == id);
            if (requiredCorrectionsSet == null)
            {
                return NotFound();
            }
            return View(requiredCorrectionsSet);
        }

        // POST: RequiredCorrectionsSets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportingId,ReportsToId")] RequiredCorrectionsSet requiredCorrectionsSet)
        {
            if (id != requiredCorrectionsSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requiredCorrectionsSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequiredCorrectionsSetExists(requiredCorrectionsSet.Id))
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
            return View(requiredCorrectionsSet);
        }

        // GET: RequiredCorrectionsSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requiredCorrectionsSet = await _context.RequiredCorrectionsSet
                .SingleOrDefaultAsync(m => m.Id == id);
            if (requiredCorrectionsSet == null)
            {
                return NotFound();
            }

            return View(requiredCorrectionsSet);
        }

        // POST: RequiredCorrectionsSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requiredCorrectionsSet = await _context.RequiredCorrectionsSet.SingleOrDefaultAsync(m => m.Id == id);
            _context.RequiredCorrectionsSet.Remove(requiredCorrectionsSet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequiredCorrectionsSetExists(int id)
        {
            return _context.RequiredCorrectionsSet.Any(e => e.Id == id);
        }

        public void ExportCorrections()
        {

            Response.ContentType = "text/csv";

            Response.ContentType = "application/octet-stream";
            StringWriter sw = new StringWriter();

            var writer = new CsvWriter(sw);
            IEnumerable records = (_context.RequiredCorrectionsSet.ToList());
            writer.WriteRecords(records);
            foreach (RequiredCorrectionsSet record in records)
            {
                writer.WriteRecord(record);
            }
            Response.WriteAsync(sw.ToString());

        }

        public ActionResult ClearCorrections()
        {
            var affectedtables = _context.Database.ExecuteSqlCommand("DeleteCorrections");

            ViewBag.Message = "Corrections Cleared";

            return View();
        }

        public ActionResult TestYourCorrections()
        {
            var affectedtables = _context.Database.ExecuteSqlCommand("UpdateReportsToWithCorrections");

            ViewBag.Message = " Reports To updated with Corrections - please retest by regeneration the hierarchy";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }


        //for server side processing

        public List<RequiredCorrectionsSet> GetRCFiltered(string search, string sortOrder, int start, int length, out int TotalCount)
        {


            var result = _context.RequiredCorrectionsSet.Where(p => (search == null
                || (p.ReportingId != null && p.ReportingId.ToLower().Contains(search.ToLower())
                || p.ReportsToId != null && p.ReportsToId.ToLower().Contains(search.ToLower())
                ))).ToList();

            TotalCount = result.Count;

            result = result.Skip(start).Take(length).ToList();


            switch (sortOrder)
            {
                case "ReportingId":
                    result = result.OrderBy(a => a.ReportingId).ToList();
                    break;


                case "ReportsToId":
                    result = result.OrderBy(a => a.ReportsToId).ToList();
                    break;
                default:
                    result = result.AsQueryable().ToList();
                    break;
            }
            return result.ToList();
        }

    }
}
