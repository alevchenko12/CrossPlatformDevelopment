using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAB6.Data;
using LAB6.Models;

namespace LAB6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RetailerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Retailer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Retailer>>> GetRetailers()
        {
            return await _context.Retailers.ToListAsync();
        }

        // GET: api/Retailer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Retailer>> GetRetailer(int id)
        {
            var retailer = await _context.Retailers.FindAsync(id);

            if (retailer == null)
            {
                return NotFound();
            }

            return retailer;
        }

        // PUT: api/Retailer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRetailer(int id, Retailer retailer)
        {
            if (id != retailer.RetailerId)
            {
                return BadRequest();
            }

            _context.Entry(retailer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RetailerExists(id))
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

        // POST: api/Retailer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Retailer>> PostRetailer(Retailer retailer)
        {
            _context.Retailers.Add(retailer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRetailer", new { id = retailer.RetailerId }, retailer);
        }

        // DELETE: api/Retailer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRetailer(int id)
        {
            var retailer = await _context.Retailers.FindAsync(id);
            if (retailer == null)
            {
                return NotFound();
            }

            _context.Retailers.Remove(retailer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RetailerExists(int id)
        {
            return _context.Retailers.Any(e => e.RetailerId == id);
        }
    }
}
