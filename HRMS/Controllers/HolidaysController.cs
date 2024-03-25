using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin, Superadmin")]
    public class HolidaysController : Controller
    {
        private ApplicationDbContext _db;
        public HolidaysController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Models.Holiday> holidayes = _db.Holidays.Where(x => x.Date.Year == DateTime.Now.Year).ToList();
            return View(holidayes);
        }
        public IActionResult Create()
        {
            Holiday holidatay = new() { HolidayId=0,HolidayName="", Date=DateTime.Now};
            return View(holidatay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HolidayName,Date")] Holiday holidatay)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Holidays.Add(holidatay);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _db.Holidays.Where(x => x.HolidayId == id).FirstOrDefaultAsync();
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HolidayId,HolidayName,Date")] Holiday holidatay)
        {
            if (id != holidatay.HolidayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existHoliday = await _db.Holidays.Where(x => x.HolidayId == holidatay.HolidayId).FirstOrDefaultAsync();
                    if (existHoliday != null)
                    {
                        existHoliday.HolidayName = holidatay.HolidayName;
                        existHoliday.Date = holidatay.Date;
                        _db.Holidays.Update(existHoliday);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _db.Holidays
                .FirstOrDefaultAsync(m => m.HolidayId == id);
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var holiday = await _db.Holidays.Where(x => x.HolidayId == id).FirstOrDefaultAsync();
            if (holiday != null)
            {
                _db.Holidays.Remove(holiday);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _db.Holidays
                .FirstOrDefaultAsync(m => m.HolidayId == id);
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }

    }
}
