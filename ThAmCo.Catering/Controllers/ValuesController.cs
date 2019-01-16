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
    public class ValuesController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public ValuesController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodMenus
        [HttpGet]
        public async Task<IActionResult> GetFoodMenus(int? EventId)
        {
            var booking = await _context.Bookings
                .Include(e => e.FoodMenu)
                .Where(e => e.EventId == EventId)
                .ToListAsync();

            var dto = await _context.FoodMenus
                .Where(e => EventId == null || !booking.Any(x => x.FoodMenuId == e.Id))
                .Select(e => new FoodMenuGetDTO() {
                    Id = e.Id,
                    MenuName = e.MenuName,
                    MenuDescription = e.MenuDescription,
                    People = e.People,
                    MenuCost = e.MenuCost
                }).ToListAsync();

            return Ok(dto);
        }

        // GET: api/FoodMenus/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodMenu = await _context.FoodMenus
                .Select(e => new FoodMenuGetDTO()
                {
                    Id = e.Id,
                    MenuName = e.MenuName,
                    MenuDescription = e.MenuDescription,
                    People = e.People,
                    MenuCost = e.MenuCost
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (foodMenu == null)
            {
                return NotFound();
            }

            return Ok(foodMenu);
        }

        // PUT: api/FoodMenus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodMenu([FromRoute] int id, [FromBody] FoodMenu foodMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != foodMenu.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodMenu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodMenuExists(id))
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

        // POST: api/FoodMenus
        [HttpPost]
        public async Task<IActionResult> PostFoodMenu([FromBody] FoodMenu foodMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FoodMenus.Add(foodMenu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodMenu", new { id = foodMenu.Id }, foodMenu);
        }

        // DELETE: api/FoodMenus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodMenu = await _context.FoodMenus.FindAsync(id);
            if (foodMenu == null)
            {
                return NotFound();
            }

            _context.FoodMenus.Remove(foodMenu);
            await _context.SaveChangesAsync();

            return Ok(foodMenu);
        }

        private bool FoodMenuExists(int id)
        {
            return _context.FoodMenus.Any(e => e.Id == id);
        }
    }
}