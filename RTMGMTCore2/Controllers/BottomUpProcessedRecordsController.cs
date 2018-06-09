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
    public class BottomUpProcessedRecordsController : Controller
    {
        private readonly RTMGMTContext _context;

        public BottomUpProcessedRecordsController(RTMGMTContext context)
        {
            _context = context;
        }

        //// GET: BottomUpProcessedRecords
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.BottomUpProcessedRecords.ToListAsync());
        //}


        // GET: ReportsToRecords
        public IActionResult Index()
        {
            //return View(await _context.ReportsToRecords.ToListAsync());
            return View();
        }

        [HttpPost]
        public JsonResult BUPRList(DTParameters param)
        {

            int TotalCount = 0;
            var filtered = this.GetBUPRFiltered(param.Search.Value, param.SortOrder, param.Start, param.Length, out TotalCount);

            var TDPRList = filtered.Select(o => new BottomUpProcessedRecords()
            {

                Id = o.Id,
                RtString = o.RtString
            });



            DTResult<BottomUpProcessedRecords> finalresult = new DTResult<BottomUpProcessedRecords>
            {
                draw = param.Draw,
                data = TDPRList.ToList(),
                recordsFiltered = TotalCount,
                recordsTotal = filtered.Count

            };
            return Json(finalresult);

        }


        // GET: BottomUpProcessedRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bottomUpProcessedRecords = await _context.BottomUpProcessedRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bottomUpProcessedRecords == null)
            {
                return NotFound();
            }

            return View(bottomUpProcessedRecords);
        }

        // GET: BottomUpProcessedRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BottomUpProcessedRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RtString")] BottomUpProcessedRecords bottomUpProcessedRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bottomUpProcessedRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bottomUpProcessedRecords);
        }

        // GET: BottomUpProcessedRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bottomUpProcessedRecords = await _context.BottomUpProcessedRecords.SingleOrDefaultAsync(m => m.Id == id);
            if (bottomUpProcessedRecords == null)
            {
                return NotFound();
            }
            return View(bottomUpProcessedRecords);
        }

        // POST: BottomUpProcessedRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RtString")] BottomUpProcessedRecords bottomUpProcessedRecords)
        {
            if (id != bottomUpProcessedRecords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bottomUpProcessedRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BottomUpProcessedRecordsExists(bottomUpProcessedRecords.Id))
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
            return View(bottomUpProcessedRecords);
        }

        // GET: BottomUpProcessedRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bottomUpProcessedRecords = await _context.BottomUpProcessedRecords
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bottomUpProcessedRecords == null)
            {
                return NotFound();
            }

            return View(bottomUpProcessedRecords);
        }

        // POST: BottomUpProcessedRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bottomUpProcessedRecords = await _context.BottomUpProcessedRecords.SingleOrDefaultAsync(m => m.Id == id);
            _context.BottomUpProcessedRecords.Remove(bottomUpProcessedRecords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BottomUpProcessedRecordsExists(int id)
        {
            return _context.BottomUpProcessedRecords.Any(e => e.Id == id);
        }

        //for server side processing

        public List<BottomUpProcessedRecords> GetBUPRFiltered(string search, string sortOrder, int start, int length, out int TotalCount)
        {


            var result = _context.BottomUpProcessedRecords.Where(p => (search == null

                ||( p.RtString != null && p.RtString.ToLower().Contains(search.ToLower())
                ))).ToList();

            TotalCount = result.Count;

            result = result.Skip(start).Take(length).ToList();


            switch (sortOrder)
            {

                case "RtString":
                    result = result.OrderBy(a => a.RtString).ToList();
                    break;

                default:
                    result = result.AsQueryable().ToList();
                    break;
            }
            return result.ToList();
        }
    }
}
