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

        /// <summary>
        /// Get all retailers with optional pagination and filtering.
        /// </summary>
        /// <param name="name">Optional filter by retailer name.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A list of retailers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Retailer>>> GetRetailers(
            string name = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<Retailer> query = _context.Retailers;

            // Apply filter for RetailerName if provided
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(r => r.RetailerName.Contains(name));
            }

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Return the list of retailers with eager loading for related entities
            return await query.Include(r => r.RetailerProductPrices)
                              .Include(r => r.SpecialOffers)
                              .Include(r => r.CustomerOrderProducts)
                              .ToListAsync();
        }

        /// <summary>
        /// Get a specific retailer by id.
        /// </summary>
        /// <param name="id">The id of the retailer.</param>
        /// <returns>The retailer if found, otherwise a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Retailer>> GetRetailer(int id)
        {
            var retailer = await _context.Retailers
                                         .Include(r => r.RetailerProductPrices)
                                         .Include(r => r.SpecialOffers)
                                         .Include(r => r.CustomerOrderProducts)
                                         .FirstOrDefaultAsync(r => r.RetailerId == id);

            if (retailer == null)
            {
                return NotFound();
            }

            return retailer;
        }

        /// <summary>
        /// Update a retailer's details.
        /// </summary>
        /// <param name="id">The id of the retailer to update.</param>
        /// <param name="retailer">The updated retailer object.</param>
        /// <returns>No content if successful, or BadRequest if there are errors.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRetailer(int id, Retailer retailer)
        {
            if (id != retailer.RetailerId)
            {
                return BadRequest("Retailer ID mismatch.");
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

        /// <summary>
        /// Create a new retailer.
        /// </summary>
        /// <param name="retailer">The retailer object to create.</param>
        /// <returns>The created retailer object.</returns>
        [HttpPost]
        public async Task<ActionResult<Retailer>> PostRetailer(Retailer retailer)
        {
            _context.Retailers.Add(retailer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRetailer", new { id = retailer.RetailerId }, retailer);
        }

        /// <summary>
        /// Delete a retailer by id.
        /// </summary>
        /// <param name="id">The id of the retailer to delete.</param>
        /// <returns>No content if successful, or NotFound if retailer does not exist.</returns>
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

        /// <summary>
        /// Check if a retailer exists by id.
        /// </summary>
        /// <param name="id">The id of the retailer to check.</param>
        /// <returns>True if the retailer exists, otherwise false.</returns>
        private bool RetailerExists(int id)
        {
            return _context.Retailers.Any(e => e.RetailerId == id);
        }
    }
}
