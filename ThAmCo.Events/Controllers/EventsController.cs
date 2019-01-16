using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;
        private readonly IConfiguration _config;

        public EventsController(EventsDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Gets HttpClient connection for the Venues API
        /// </summary>
        /// <returns>
        /// HttpClient connection for Venues API
        /// </returns>
        private HttpClient getConnection()
        {
            HttpClient c = new HttpClient();
            c.BaseAddress = new System.Uri(_config["VenuesAPI"]);
            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            return c;
        }

        /// <summary>
        /// Gets HttpClient connection for the FoodMenus API
        /// </summary>
        /// <returns>
        /// HttpClient connection for FoodMenus API
        /// </returns>
        private HttpClient getConnectionFoodAPI()
        {
            HttpClient c = new HttpClient();
            c.BaseAddress = new System.Uri(_config["FoodMenusAPI"]);
            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            return c;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = _context.Events.Include(g => g.Bookings)
                .Where(o => o.Disabled == null)
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Date = e.Date,
                    Duration = e.Duration,
                    Type = e.Type,
                    Venue = e.Venue ?? "No Venue",
                    TotalGuests = e.Bookings.Count,
                    TotalStaff = e.StaffBookings.Count,
                    HasFirstAider = _context.StaffBooking.Include(s => s.Staff)
                        .Include(g => g.Event)
                        .Where(g => g.EventId == e.Id && g.Staff.FirstAider).Count() >= 1
                        ? "Yes" : "No",
                    HasEnoughStaff = e.StaffBookings.Count != 0 && e.StaffBookings.Count >= Math.Ceiling((double) e.Bookings.Count / 10) ? "Yes" : "No"
                }).ToListAsync();

            return View(await events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Where(o => o.Disabled == null)
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Date = e.Date,
                    Duration = e.Duration,
                    Type = e.Type,
                    Venue = e.Venue ?? "No Venue",
                    VenueReference = e.VenueReference,
                    TotalGuests = e.Bookings.Count,
                    AllGuests = _context.Guests.Include(c => c.Customer).Include(y => y.Event).Where(s => s.EventId == e.Id)
                        .Select(s => new GuestBookingViewModel()
                        {
                            CustomerId = s.CustomerId,
                            Customer = s.Customer,
                            EventId = s.EventId,
                            Event = s.Event,
                            EventName = s.Event.Title,
                            Attended = s.Attended
                        })
                        .ToList(),
                    TotalStaff = e.StaffBookings.Count,
                    AllStaff = _context.StaffBooking.Include(g => g.Staff).Include(g => g.Event)
                        .Select(s => new StaffBookingViewModel()
                        {
                            StaffId = s.StaffId,
                            Staff = s.Staff,
                            StaffName = s.Staff.FirstName + " " + s.Staff.Surname,
                            FirstAider = s.Staff.FirstAider ? "Yes" : "No",
                            EventId = s.EventId,
                            Event = s.Event,
                        })
                        .Where(g => g.EventId == id).ToList(),
                    HasFirstAider = _context.StaffBooking.Include(s => s.Staff)
                        .Include(g => g.Event)
                        .Where(g => g.EventId == e.Id && g.Staff.FirstAider).Count() >= 1
                        ? "Yes" : "No",
                    HasEnoughStaff = e.StaffBookings.Count != 0 && e.StaffBookings.Count >= Math.Ceiling((double)e.Bookings.Count / 10) ? "Yes" : "No"
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            //use api to get venue info
            if (@event.Venue != null && @event.VenueReference != null)
            {
                try
                {
                    HttpResponseMessage response = await getConnection().GetAsync("/api/reservations/" + @event.VenueReference);
                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest("Couldn't get reservation for old venue");
                    }

                    @event.VenueData = await response.Content.ReadAsAsync<ReservationGetDTO>();
                }
                catch (Exception)
                {
                    return BadRequest("Unable to connect to Venue API");
                }

                // Set total cost for venue
                TimeSpan timeSpan = new TimeSpan();
                timeSpan = @event.Date.Add(@event.Duration.Value).Subtract(@event.Date);
                double venueCost = timeSpan.Hours * @event.VenueData.VenueCostPerHour;
                @event.TotalVenueCost = "£" + venueCost.ToString() + " (" + timeSpan.Hours + " hours at £" + @event.VenueData.VenueCostPerHour.ToString() + " per hour)";

                // Add to total event cost
                @event.TotalEventCost += venueCost;
            }
            else
            {
                @event.TotalVenueCost = "£0";
            }

            //use api to get food info
            try
            {
                HttpResponseMessage response = await getConnectionFoodAPI().GetAsync("/api/bookings/" + @event.Id);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest("Couldn't get food menu data for the event");
                }

                @event.FoodMenuData = await response.Content.ReadAsAsync<List<FoodMenuDTO>>();
                @event.HasFoodBooked = @event.FoodMenuData.Count() == 0 ? "No" : "Yes";
            }
            catch (Exception)
            {
                return BadRequest("Unable to connect to Food Menu API");
            }

            // Set total cost for food menu
            double totalFoodCost = 0;
            @event.FoodMenuData.ForEach(s => totalFoodCost += (s.MenuCost / s.People) * @event.TotalGuests);
            @event.TotalFoodMenuCost = "£" + totalFoodCost.ToString();

            // Add to total event cost
            @event.TotalEventCost += totalFoodCost;

            return View(@event);
        }

        /// <summary>
        /// Gets a list of available venues for an event
        /// </summary>
        /// <returns>
        /// A list of available venues as VenueDTO IEnumerable
        /// </returns>
        /// <param name="event">The event to find venues for</param>
        private async Task<IEnumerable<VenueDTO>> GetVenueData(EventViewModel @event) {
            var venues = new List<VenueDTO>().AsEnumerable();

            try
            {
                HttpResponseMessage response = await getConnection().GetAsync("/api/availability?eventType=" + @event.Type 
                    + "&beginDate=" + @event.Date.ToString("MM/dd/yyyy") 
                    + "&endDate=" + @event.Date.Add(@event.Duration.Value).ToString("MM/dd/yyyy"));
                if (response.IsSuccessStatusCode)
                {
                    venues = await response.Content.ReadAsAsync<IEnumerable<VenueDTO>>();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return venues;
        }

        // GET: Events/SetVenue
        public async Task<IActionResult> SetVenue(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Where(o => o.Disabled == null)
                .Select(e => new EventViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Date = e.Date,
                    Duration = e.Duration,
                    Type = e.Type

                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            var venues = await GetVenueData(@event);
            ViewData["Venue"] = new SelectList(venues, "Code", "Name");
            ViewData["NumVenues"] = venues.Count();

            return View(@event);
        }

        // POST: Events/SetVenue
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetVenue(int id, [Bind("Id,Date,Venue")] EventViewModel @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (@event.Venue != null)
            {
                try
                {
                    var current = _context.Events
                        .Where(o => o.Disabled == null)
                        .FirstOrDefault(e => e.Id == id);
                    if (current == null)
                    {
                        return NotFound();
                    }

                    //Remove old reservation
                    if (current.Venue != null && current.VenueReference != null)
                    {
                        try
                        {
                            HttpResponseMessage res = await getConnection().DeleteAsync("/api/reservations/" + current.VenueReference);
                            if (!res.IsSuccessStatusCode)
                            {
                                return BadRequest("Couldn't remove reservation for old venue");
                            }
                        }
                        catch (Exception)
                        {
                            return BadRequest("Unable to connect to Venue API");
                        }
                    }

                    //Add new reservation
                    var reservation = new ReservationPostDTO()
                    {
                        EventDate = @event.Date,
                        VenueCode = @event.Venue,
                        StaffId = "1"
                    };

                    try
                    {
                        HttpResponseMessage response = await getConnection().PostAsJsonAsync("/api/reservations", reservation);
                        if (response.IsSuccessStatusCode)
                        {
                            var reservationResponse = await response.Content.ReadAsAsync<ReservationGetDTO>();

                            //Save reservation info to event in db
                            current.Venue = @event.Venue;
                            current.VenueReference = reservationResponse.Reference;
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception)
                    {
                        return BadRequest("Unable to connect to Venue API");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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

            var venues = await GetVenueData(@event);
            ViewData["Venue"] = new SelectList(venues, "Code", "Name");
            ViewData["NumVenues"] = venues.Count();

            return View(@event);
        }

        /// <summary>
        /// Gets event types from the Venues API
        /// </summary>
        /// <returns>
        /// IEnumberable of TypeDto holding all event types from the Venues API
        /// </returns>
        private async Task<IEnumerable<TypeDto>> GetEventTypes()
        {
            var types = new List<TypeDto>().AsEnumerable();

            try
            {
                HttpResponseMessage response = await getConnection().GetAsync("/api/eventtypes");
                if (response.IsSuccessStatusCode)
                {
                    types = await response.Content.ReadAsAsync<IEnumerable<TypeDto>>();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return types;
        }

        // GET: Events/Create
        public async Task<IActionResult> Create(string code, string type, string date)
        {
            if (code == null || type == null || date == null)
            {
                ViewData["Type"] = new SelectList(await GetEventTypes(), "Id", "Title");
            }
            else
            {
                ViewData["Type"] = type;
                ViewData["Venue"] = code;
                ViewData["Date"] = date;
            }
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Duration,Type,Venue")] EventViewModel @event)
        {
            if (ModelState.IsValid)
            {
                if(@event.Venue != null)
                {
                    //Make sure venue is fine for this event and make reservation
                    var reservation = new ReservationPostDTO()
                    {
                        EventDate = @event.Date,
                        VenueCode = @event.Venue,
                        StaffId = "1"
                    };

                    try
                    {
                        HttpResponseMessage response = await getConnection().PostAsJsonAsync("/api/reservations", reservation);
                        if (response.IsSuccessStatusCode)
                        {
                            var reservationResponse = await response.Content.ReadAsAsync<ReservationGetDTO>();

                            //Save reservation info to event
                            Event EventToAdd = new Event()
                            {
                                Id = @event.Id,
                                Title = @event.Title,
                                Date = @event.Date,
                                Duration = @event.Duration,
                                Type = @event.Type,
                                Venue = @event.Venue,
                                VenueReference = reservationResponse.Reference
                            };


                            try
                            {
                                _context.Add(EventToAdd);
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("Duration", "Please use a valid duration in the format: 00:00:00");
                                return View(@event);
                            }
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    catch (Exception)
                    {
                        return BadRequest("Unable to connect to Venue API");
                    }

                }
                else
                {
                    Event EventToAdd = new Event()
                    {
                        Id = @event.Id,
                        Title = @event.Title,
                        Date = @event.Date,
                        Duration = @event.Duration,
                        Type = @event.Type
                    };

                    try
                    {
                        _context.Add(EventToAdd);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("Duration", "Please use a valid duration in the format: 00:00:00");
                        return View(@event);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Type"] = new SelectList(await GetEventTypes(), "Id", "Title");

            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Where(o => o.Disabled == null)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Date,Duration,Type")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var current = _context.Events
                        .Where(o => o.Disabled == null)
                        .FirstOrDefault(e => e.Id == id);
                    if (current == null)
                    {
                        return NotFound();
                    }

                    try
                    {
                        current.Title = @event.Title;
                        current.Duration = @event.Duration;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("Duration", "Please use a valid duration in the format: 00:00:00");
                        return View(@event);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Where(o => o.Disabled == null)
                .Select(e => new EventViewModel() {
                    Id = e.Id,
                    Title = e.Title,
                    Date = e.Date,
                    Duration = e.Duration,
                    Type = e.Type
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events
                .Where(o => o.Disabled == null)
                .FirstOrDefaultAsync(m => m.Id == id);

            if(@event == null)
            {
                return NotFound();
            }

            //Remove old reservation
            if (@event.Venue != null && @event.VenueReference != null)
            {
                try
                {
                    HttpResponseMessage res = await getConnection().DeleteAsync("/api/reservations/" + @event.VenueReference);
                    if (!res.IsSuccessStatusCode)
                    {
                        return BadRequest("Couldn't remove reservation for old venue");
                    }
                }
                catch (Exception)
                {
                    return BadRequest("Unable to connect to Venue API");
                }
            }

            //Remove all staff
            _context.StaffBooking.Where(s => s.EventId == @event.Id).ToList()
                .ForEach(s => _context.StaffBooking.Remove(s));

            @event.Disabled = DateTime.Now;
            @event.Venue = null;
            @event.VenueReference = null;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
