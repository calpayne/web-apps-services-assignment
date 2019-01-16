using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.Models;

namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public BookingsController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public IEnumerable<Booking> GetBookings()
        {
            return _context.Bookings;
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _context.Bookings
                .Include(e => e.FoodMenu)
                .Where(e => e.EventId == id)
                .Select(e => new FoodMenuGetDTO()
                {
                    Id = e.FoodMenu.Id,
                    MenuName = e.FoodMenu.MenuName,
                    MenuDescription = e.FoodMenu.MenuDescription,
                    MenuCost = e.FoodMenu.MenuCost,
                    People = e.FoodMenu.People
                }).ToListAsync();

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking([FromRoute] int id, [FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.FoodMenuId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> PostBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodMenu = await _context.FoodMenus
                .FirstOrDefaultAsync(e => e.Id == booking.FoodMenuId);

            if (foodMenu == null)
            {
                return NotFound();
            }

            var BookingToAdd = new Booking()
            {
                Id = $"{foodMenu.Id}{booking.EventDate:yyyyMMdd}{DateTime.Now:yyyyMMdd}",
                EventId = booking.EventId,
                FoodMenuId = booking.FoodMenuId,
                EventDate = booking.EventDate,
                WhenMade = DateTime.Now
            };

            _context.Bookings.Add(BookingToAdd);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.FoodMenuId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBooking", new { id = BookingToAdd.FoodMenuId }, BookingToAdd);
        }

        // DELETE: api/Bookings/5
        [HttpDelete]
        public async Task<IActionResult> DeleteBooking([FromQuery] int EventId, [FromQuery] int FoodId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _context.Bookings.FirstOrDefaultAsync(e => e.EventId == EventId && e.FoodMenuId == FoodId);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.FoodMenuId == id);
        }
    }
}