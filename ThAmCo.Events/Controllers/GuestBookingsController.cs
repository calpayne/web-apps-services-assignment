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
    public class GuestBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: GuestBookings
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null || !EventExists(id.Value))
            {
                return NotFound();
            }

            var index = new GuestBookingIndexViewModel()
            {
                EventId = id.Value,
                TotalGuests = _context.Guests.Include(g => g.Customer).Include(g => g.Event)
                    .Where(g => id == null || g.EventId == id).Count(),
                AllGuests = await _context.Guests.Include(c => c.Customer)
                    .Include(e => e.Event)
                    .Where(g => id == null || g.EventId == id && g.Event.Disabled == null)
                    .Select(s => new GuestBookingViewModel() {
                        CustomerId = s.CustomerId,
                        Customer = s.Customer,
                        EventId = s.EventId,
                        Event = s.Event,
                        EventName = s.Event.Title,
                        Attended = s.Attended
                    })
                    .ToListAsync()
            };

            return View(index);
        }

        // GET: GuestBookings/Create
        public IActionResult Create(int? id)
        {
            var OnEvent = _context.Guests.Include(c => c.Customer).Where(e => e.EventId == id).Select(c => new Customer()
            {
                Id = c.CustomerId,
                Surname = c.Customer.Surname,
                FirstName = c.Customer.FirstName,
                Email = c.Customer.Email,
                Bookings = c.Customer.Bookings
            }).ToList();

            var ToShow = _context.Customers.Where(x => !OnEvent.Contains(x)).ToList();

            ViewData["CustomerCheck"] = ToShow.Count == 0;

            ViewData["CustomerId"] = new SelectList(ToShow, "Id", "Email");
            ViewData["EventId"] = id;
            return View();
        }

        // POST: GuestBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,EventId,Attended")] GuestBooking guestBooking)
        {
            if (ModelState.IsValid)
            {
                if (GuestBookingExists(guestBooking.CustomerId, guestBooking.EventId))
                {
                    ModelState.AddModelError(String.Empty, "This customer is already a guest at the event.");
                    return View(guestBooking);
                }

                _context.Add(guestBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = guestBooking.EventId });
            }

            var OnEvent = _context.Guests.Include(c => c.Customer).Where(e => e.EventId == guestBooking.EventId).Select(c => new Customer()
            {
                Id = c.CustomerId,
                Surname = c.Customer.Surname,
                FirstName = c.Customer.FirstName,
                Email = c.Customer.Email,
                Bookings = c.Customer.Bookings
            }).ToList();

            var ToShow = _context.Customers.Where(x => !OnEvent.Contains(x)).ToList();

            ViewData["CustomerCheck"] = ToShow == null || ToShow.Count == 0;

            ViewData["CustomerId"] = new SelectList(ToShow, "Id", "Email");
            ViewData["EventId"] = guestBooking.EventId;

            return View(guestBooking);
        }

        [HttpPost]
        // GET: GuestBookings/UpdateAttendance/Customer/5/Event/5
        public async Task<IActionResult> UpdateAttendance(int? customerId, int? eventId)
        {
            if (customerId == null || eventId == null || !GuestBookingExists(customerId, eventId))
            {
                return NotFound();
            }

            var current = await _context.Guests
                .Include(g => g.Customer)
                .Include(g => g.Event)
                .FirstOrDefaultAsync(m => m.CustomerId == customerId && m.EventId == eventId);

            if (current == null)
            {
                return NotFound();
            }
            current.Attended = !current.Attended;
            _context.SaveChanges();

            return RedirectToAction("Index", new { id = eventId });
        }

        [Route("GuestBookings/Delete/Customer/{customerId?}/Event/{eventId?}")]
        // GET: GuestBookings/Delete/Customer/5/Event/5
        public async Task<IActionResult> Delete(int? customerId, int? eventId)
        {
            if (customerId == null || eventId == null || !GuestBookingExists(customerId, eventId))
            {
                return NotFound();
            }

            var guestBooking = await _context.Guests
                .Include(g => g.Customer)
                .Include(g => g.Event)
                .FirstOrDefaultAsync(m => m.CustomerId == customerId && m.EventId == eventId);

            if (guestBooking == null)
            {
                return NotFound();
            }

            _context.Guests.Remove(guestBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = guestBooking.EventId });
        }

        private bool GuestBookingExists(int? customerId, int? eventId)
        {
            return _context.Guests.Any(e => e.CustomerId == customerId && e.EventId == eventId);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
