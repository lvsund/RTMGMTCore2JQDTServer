using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RTMGMTCore2.Models;
using System.IO;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Web;
using Microsoft.Extensions.FileProviders;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

namespace RTMGMTCore2.Controllers
{
    public class ReportsToRecordsController : Controller
    {
        private IHostingEnvironment _environment;
        private readonly RTMGMTContext _context;

        public ReportsToRecordsController(RTMGMTContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //// GET: ReportsToRecords
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.ReportsToRecords.ToListAsync());
        //}

        // GET: ReportsToRecords
        public IActionResult Index()
        {
            //return View(await _context.ReportsToRecords.ToListAsync());
            return View();
        }


        [HttpPost]
        public JsonResult RTRList(DTParameters param)
        {

            int TotalCount = 0;
            var filtered = this.GetRTRFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out TotalCount);

            var RTRList = filtered.Select(o => new ReportsToRecords()
            {

                Id = o.Id,
                ReportingId = o.ReportingId,
                Title = o.Title,
                Name = o.Name,
                ReportsToId = o.ReportsToId,
                EmployeeId = o.EmployeeId
            });



            DTResult<ReportsToRecords> finalresult = new DTResult<ReportsToRecords>
            {
                draw = param.Draw,
                data = RTRList.ToList(),
                recordsFiltered = TotalCount,
                recordsTotal = filtered.Count

            };
            return Json(finalresult);

        }

        // GET: ReportsToRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsToRecords = await _context.ReportsToRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reportsToRecords == null)
            {
                return NotFound();
            }

            return View(reportsToRecords);
        }

        // GET: ReportsToRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReportsToRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportingId,Title,Name,ReportsToId,EmployeeId")] ReportsToRecords reportsToRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportsToRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reportsToRecords);
        }

        // GET: ReportsToRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsToRecords = await _context.ReportsToRecords.SingleOrDefaultAsync(m => m.Id == id);
            if (reportsToRecords == null)
            {
                return NotFound();
            }
            return View(reportsToRecords);
        }

        // POST: ReportsToRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportingId,Title,Name,ReportsToId,EmployeeId")] ReportsToRecords reportsToRecords)
        {
            if (id != reportsToRecords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportsToRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportsToRecordsExists(reportsToRecords.Id))
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
            return View(reportsToRecords);
        }

        // GET: ReportsToRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportsToRecords = await _context.ReportsToRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reportsToRecords == null)
            {
                return NotFound();
            }

            return View(reportsToRecords);
        }

        // POST: ReportsToRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportsToRecords = await _context.ReportsToRecords.SingleOrDefaultAsync(m => m.Id == id);
            _context.ReportsToRecords.Remove(reportsToRecords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportsToRecordsExists(int id)
        {
            return _context.ReportsToRecords.Any(e => e.Id == id);
        }


        public ActionResult CreateReportingHierarchy()
        {
            var affectedtables = _context.Database.ExecuteSqlCommand("ProcessReportsToRecords");

            ViewBag.Message = " Reporting Hierarchy Created";

            return View();
        }

        public ActionResult DeleteAllData()
        {
            var affectedtables = _context.Database.ExecuteSqlCommand("DeleteAllRecords");

            ViewBag.Message = "Delete Confirmed";

            return View();
        }

        public ActionResult ClearProcessingTables()
        {
            var affectedtables = _context.Database.ExecuteSqlCommand("TruncateProcessedRecordsExceptCorrections");

            ViewBag.Message = " Processing Tables Cleared";

            return View();
        }

        public IActionResult ImportReportsToRecords()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportReportsToRecords(ICollection<IFormFile> files)
        {
            string path = null;

            List<ReportsToRecords> RTRs = new List<ReportsToRecords>();

            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
                try
            {
                if (file == null || file.Length == 0)

                    return Content("file not selected");
                else
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var fileName = Path.GetFileName(file.FileName);
                    path = uploads + "\\" + fileName;
                    //file.SaveAs(path);
                    var csv = new CsvReader(new StreamReader(path));
                    var RTRecordlist = csv.GetRecords<ReportsToRecords>();

                    foreach (var r in RTRecordlist)
                    {
                        ReportsToRecords RTR = new ReportsToRecords();
                       // RTR.Id = r.Id;
                       //comment the above out because that field in SQL Server is auto increment identity column and therefore doesnt need to be imported
                        RTR.ReportingId = r.ReportingId;
                        RTR.Title = r.Title;
                        RTR.Name = r.Name;
                        RTR.ReportsToId = r.ReportsToId;
                        RTR.EmployeeId = r.EmployeeId;
                        RTRs.Add(RTR);
                        _context.ReportsToRecords.Add(RTR);
                    }

                  await  _context.SaveChangesAsync();
                  ViewBag.Message = " ReportsToRecords Imported";
                  return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewBag.Message   = "Upload Failed";
            }


            return View();
        }


       

        public void ExportReportsToRecords()
        {

            Response.ContentType = "text/csv";

            Response.ContentType = "application/octet-stream";
            StringWriter sw = new StringWriter();

            var writer = new CsvWriter(sw);
            IEnumerable records = (_context.ReportsToRecords.ToList());
            writer.WriteRecords(records);
            foreach (ReportsToRecords record in records)
            {
                writer.WriteRecord(record);

            }
            Response.WriteAsync(sw.ToString());

        }

        //for server side processing

        public List<ReportsToRecords> GetRTRFiltered(string search, string sortOrder, int start, int length, out int TotalCount)
        {


            var result = _context.ReportsToRecords.Where(p => (search == null
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



