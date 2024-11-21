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
    public class SpecialOfferController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpecialOfferController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all special offers with optional filtering and pagination.
        /// </summary>
        /// <param name="retailerId">Optional filter by retailer ID.</param>
        /// <param name="productId">Optional filter by product ID.</param>
        /// <param name="startDate">Optional filter by start date.</param>
        /// <param name="endDate">Optional filter by end date.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A list of special offers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialOffer>>> GetSpecialOffers(
            int? retailerId = null,
            int? productId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<SpecialOffer> query = _context.SpecialOffers;

            // Apply filters if provided
            if (retailerId.HasValue)
            {
                query = query.Where(s => s.RetailerId == retailerId.Value);
            }

            if (productId.HasValue)
            {
                query = query.Where(s => s.ProductId == productId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(s => s.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(s => s.EndDate <= endDate.Value);
            }

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Include related entities for eager loading
            var result = await query.Include(s => s.Product)
                                     .Include(s => s.Retailer)
                                     .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get a specific special offer by id.
        /// </summary>
        /// <param name="id">The id of the special offer.</param>
        /// <returns>The special offer if found, otherwise a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialOffer>> GetSpecialOffer(int id)
        {
            var specialOffer = await _context.SpecialOffers
                                             .Include(s => s.Product)
                                             .Include(s => s.Retailer)
                                             .FirstOrDefaultAsync(s => s.SpecialOfferId == id);

            if (specialOffer == null)
            {
                return NotFound();
            }

            return specialOffer;
        }

        /// <summary>
        /// Update a special offer.
        /// </summary>
        /// <param name="id">The id of the special offer to update.</param>
        /// <param name="specialOffer">The updated special offer object.</param>
        /// <returns>No content if successful, or BadRequest if there are errors.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialOffer(int id, SpecialOffer specialOffer)
        {
            if (id != specialOffer.SpecialOfferId)
            {
                return BadRequest("SpecialOffer ID mismatch.");
            }

            // Validate the date range
            if (specialOffer.StartDate >= specialOffer.EndDate)
            {
                return BadRequest("Start Date must be earlier than End Date.");
            }

            _context.Entry(specialOffer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialOfferExists(id))
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
        /// Create a new special offer.
        /// </summary>
        /// <param name="specialOffer">The special offer object to create.</param>
        /// <returns>The created special offer object.</returns>
        [HttpPost]
        public async Task<ActionResult<SpecialOffer>> PostSpecialOffer(SpecialOffer specialOffer)
        {
            // Validate the date range
            if (specialOffer.StartDate >= specialOffer.EndDate)
            {
                return BadRequest("Start Date must be earlier than End Date.");
            }

            // Check for existing special offer with the same product and retailer during the same period
            var existingOffer = await _context.SpecialOffers
                                              .FirstOrDefaultAsync(s => s.ProductId == specialOffer.ProductId &&
                                                                        s.RetailerId == specialOffer.RetailerId &&
                                                                        s.StartDate <= specialOffer.EndDate &&
                                                                        s.EndDate >= specialOffer.StartDate);
            if (existingOffer != null)
            {
                return Conflict("A special offer already exists for this product and retailer in the specified period.");
            }

            _context.SpecialOffers.Add(specialOffer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecialOffer", new { id = specialOffer.SpecialOfferId }, specialOffer);
        }

        /// <summary>
        /// Delete a special offer.
        /// </summary>
        /// <param name="id">The id of the special offer to delete.</param>
        /// <returns>No content if successful, or NotFound if the offer does not exist.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialOffer(int id)
        {
            var specialOffer = await _context.SpecialOffers.FindAsync(id);
            if (specialOffer == null)
            {
                return NotFound();
            }

            _context.SpecialOffers.Remove(specialOffer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if a special offer exists by id.
        /// </summary>
        /// <param name="id">The id of the special offer to check.</param>
        /// <returns>True if the special offer exists, otherwise false.</returns>
        private bool SpecialOfferExists(int id)
        {
            return _context.SpecialOffers.Any(e => e.SpecialOfferId == id);
        }
    }
}
