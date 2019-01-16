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
    public class FoodMenusController : Controller
    {
        private readonly EventsDbContext _context;
        private readonly IConfiguration _config;

        public FoodMenusController(EventsDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Gets HttpClient connection for the FoodMenus API
        /// </summary>
        /// <returns>
        /// HttpClient connection for FoodMenus API
        /// </returns>
        private HttpClient getConnection()
        {
            HttpClient c = new HttpClient();
            c.BaseAddress = new System.Uri(_config["FoodMenusAPI"]);
            c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            return c;
        }

        /// <summary>
        /// Gets a list of food menus from the FoodMenus API
        /// </summary>
        /// <returns>
        /// A list of food menus as a FoodMenuDTO IEnumerable
        /// </returns>
        private async Task<IEnumerable<FoodMenuDTO>> GetFoodMenuData()
        {
            var menus = new List<FoodMenuDTO>().AsEnumerable();

            try
            {
                HttpResponseMessage response = await getConnection().GetAsync("/api/values/");
                if (response.IsSuccessStatusCode)
                {
                    menus = await response.Content.ReadAsAsync<IEnumerable<FoodMenuDTO>>();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return menus;
        }

        /// <summary>
        /// Gets a specific food menu from the FoodMenus API
        /// </summary>
        /// <returns>
        /// A food menu item as a FoodMenuDTO
        /// </returns>
        private async Task<FoodMenuDTO> GetFoodMenuData(int id)
        {
            try
            {
                HttpResponseMessage response = await getConnection().GetAsync("/api/values/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var menu = await response.Content.ReadAsAsync<FoodMenuDTO>();

                    if (menu != null)
                    {
                        return menu;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        /// <summary>
        /// Gets a list of food menus from the FoodMenus API that are not booked onto an event already
        /// </summary>
        /// <returns>
        /// A list of food menus not booked for an event as a FoodMenuDTO IEnumerable
        /// </returns>
        private async Task<List<FoodMenuDTO>> GetFoodMenuDataForEvent(int EventId)
        {
            var menus = new List<FoodMenuDTO>();

            try
            {
                HttpResponseMessage response = await getConnection().GetAsync("/api/values/?EventId=" + EventId);
                if (response.IsSuccessStatusCode)
                {
                    menus = await response.Content.ReadAsAsync<List<FoodMenuDTO>>();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return menus;
        }

        // GET: FoodMenus
        public async Task<IActionResult> Index()
        {
            return View(await GetFoodMenuData());
        }

        // GET: FoodMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await GetFoodMenuData(id.Value);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        [Route("FoodMenus/UnbookFood/Event/{EventId?}/Food/{FoodId?}")]
        // GET: FoodMenus/UnbookFood/Event/1/Food/1
        public async Task<IActionResult> UnbookFood(int? EventId, int? FoodId)
        {
            if (EventId == null || FoodId == null)
            {
                return NotFound();
            }

            try
            {
                HttpResponseMessage response = await getConnection().DeleteAsync("/api/bookings?eventid=" + EventId + "&foodid=" + FoodId);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Events", new { id = EventId });
                }
            }
            catch (Exception)
            {
                return BadRequest("Booking could not be deleted.");
            }

            return BadRequest("Booking could not be deleted.");
        }

        // GET: FoodMenus/BookFood
        public async Task<IActionResult> BookFood(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Where(o => o.Disabled == null)
                .Select(e => new EventViewModel() {
                    Id = e.Id,
                    Title = e.Title,
                    Date = e.Date
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            List<FoodMenuDTO> menus = await GetFoodMenuDataForEvent(id.Value);
            // Do this so in the view a null check can be used to see if it is empty
            if (menus.Count() == 0)
            {
                ViewData["FoodMenu"] = null;
            }
            else
            {
                ViewData["FoodMenu"] = new SelectList(menus, "Id", "MenuName");
            }

            return View(@event);
        }

        // POST: FoodMenus/BookFood
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookFood(int id, [Bind("Id,Date,FoodMenu")] EventViewModel @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (@event.Date != null && @event.FoodMenu != null)
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

                    //Add new booking
                    var booking = new FoodBookingPostDTO()
                    {
                        EventId = @event.Id,
                        EventDate = @event.Date,
                        FoodMenuId = @event.FoodMenu.Value
                    };

                    try
                    {
                        HttpResponseMessage response = await getConnection().PostAsJsonAsync("/api/bookings", booking);
                        if (response.IsSuccessStatusCode)
                        {
                            var bookingResponse = await response.Content.ReadAsAsync<FoodBookingPostDTO>();
                        }
                    }
                    catch(Exception)
                    {
                        return BadRequest("Unable to connect to Food Menu API");
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
                return RedirectToAction("Details", "Events", new { id = @event.Id });
            }

            ViewData["FoodMenu"] = new SelectList(await GetFoodMenuDataForEvent(id), "Id", "MenuName");
            return View(@event);
        }

        // GET: FoodMenus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuName,MenuDescription,People,MenuCost")] FoodMenuDTO foodMenuDTO)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    // Send to api
                    HttpResponseMessage response = await getConnection().PostAsJsonAsync("/api/values/", foodMenuDTO);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    return BadRequest("Unable to connect to Food Menu API");
                }
            }
            return View(foodMenuDTO);
        }

        // GET: FoodMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await GetFoodMenuData(id.Value);

            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: FoodMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuName,MenuDescription,People,MenuCost")] FoodMenuDTO foodMenuDTO)
        {
            if (id != foodMenuDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Send to api
                    HttpResponseMessage response = await getConnection().PutAsJsonAsync("/api/values/" + id, foodMenuDTO);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    return BadRequest("Unable to connect to Food Menu API");
                }
            }
            return View(foodMenuDTO);
        }

        // GET: FoodMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodMenuGetDTO = await GetFoodMenuData(id.Value);
            if (foodMenuGetDTO == null)
            {
                return NotFound();
            }

            return View(foodMenuGetDTO);
        }

        // POST: FoodMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Send to api
                HttpResponseMessage response = await getConnection().DeleteAsync("/api/values/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                return BadRequest("Unable to connect to Food Menu API");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
