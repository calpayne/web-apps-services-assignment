using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class StaffBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: StaffBookings
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null || !EventExists(id.Value))
            {
                return NotFound();
            }

            var index = new StaffBookingIndexViewModel()
            {
                EventId = id.Value,
                TotalStaff = _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event)
                    .Where(g => id == null || g.EventId == id).Count(),
                HasFirstAider = _context.StaffBooking.Include(s => s.Staff)
                    .Include(g => g.Event)
                    .Where(g => g.EventId == id && g.Staff.FirstAider).Count() >= 1
                    ? "Yes" : "No",
                HasEnoughStaff = _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event)
                                                                            .Where(g => id == null || g.EventId == id).Count() != 0 
                    && _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event)
                                                                  .Where(g => id == null || g.EventId == id).Count() 
                    >= Math.Ceiling(
                        (double) _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event)
                                                                            .Where(g => id == null || g.EventId == id).Count() / 10)
                    ? "Yes" : "No",
                StaffMembers = await _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event)
                    .Select(s => new StaffBookingViewModel()
                    {
                        StaffId = s.StaffId,
                        Staff = s.Staff,
                        StaffName = s.Staff.FirstName + " " + s.Staff.Surname,
                        FirstAider = s.Staff.FirstAider ? "Yes" : "No",
                        EventId = s.EventId,
                        Event = s.Event,
                    })
                    .Where(g => g.EventId == id && g.Event.Disabled == null).ToListAsync()
            };

            return View(index);
        }

        // GET: StaffBookings/Create
        public IActionResult Create(int? id)
        {
            var OnEvent = _context.StaffBooking.Include(c => c.Staff).Where(e => e.EventId == id).Select(c => new Staff()
            {
                Id = c.StaffId,
                Surname = c.Staff.Surname,
                FirstName = c.Staff.FirstName,
                FirstAider = c.Staff.FirstAider,
                Bookings = c.Staff.Bookings
            }).ToList();

            var ToShow = _context.Staff.Where(x => !OnEvent.Contains(x)).ToList();

            ViewData["StaffCheck"] = ToShow.Count == 0;

            ViewData["StaffId"] = new SelectList(ToShow, "Id", "FirstName");
            ViewData["EventId"] = id;
            return View();
        }

        // POST: StaffBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,EventId")] StaffBooking staffBooking)
        {
            if (ModelState.IsValid)
            {
                if (StaffBookingExists(staffBooking.StaffId, staffBooking.EventId))
                {
                    ModelState.AddModelError(String.Empty, "This staff member is already at the event.");
                    return View(staffBooking);
                }

                _context.Add(staffBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = staffBooking.EventId });
            }

            var OnEvent = _context.StaffBooking.Include(c => c.Staff).Where(e => e.EventId == staffBooking.EventId).Select(c => new Staff()
            {
                Id = c.StaffId,
                Surname = c.Staff.Surname,
                FirstName = c.Staff.FirstName,
                FirstAider = c.Staff.FirstAider,
                Bookings = c.Staff.Bookings
            }).ToList();

            var ToShow = _context.Staff.Where(x => !OnEvent.Contains(x)).ToList();

            ViewData["StaffCheck"] = ToShow == null || ToShow.Count == 0;

            ViewData["StaffId"] = new SelectList(ToShow, "Id", "FirstName");
            ViewData["EventId"] = staffBooking.EventId;

            return View(staffBooking);
        }

        [Route("StaffBookings/Delete/Staff/{staffId?}/Event/{eventId?}")]
        // GET: GuestBookings/Delete/Staff/5/Event/5
        public async Task<IActionResult> Delete(int? staffId, int? eventId)
        {
            if (staffId == null || eventId == null || !StaffBookingExists(staffId, eventId))
            {
                return NotFound();
            }

            var staffBooking = await _context.StaffBooking
                .Include(g => g.Staff)
                .Include(g => g.Event)
                .FirstOrDefaultAsync(m => m.StaffId == staffId && m.EventId == eventId);

            if (staffBooking == null)
            {
                return NotFound();
            }

            _context.StaffBooking.Remove(staffBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = staffBooking.EventId });
        }

        private bool StaffBookingExists(int? staffId, int? eventId)
        {
            return _context.StaffBooking.Any(e => e.StaffId == staffId && e.EventId == eventId);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
