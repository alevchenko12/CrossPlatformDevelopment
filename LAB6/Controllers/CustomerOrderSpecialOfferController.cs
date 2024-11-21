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
    public class CustomerOrderSpecialOfferController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerOrderSpecialOfferController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all customer order special offers with optional pagination and filtering.
        /// </summary>
        /// <param name="orderId">Optional filter by OrderId.</param>
        /// <param name="specialOfferId">Optional filter by SpecialOfferId.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A list of customer order special offers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrderSpecialOffer>>> GetCustomerOrderSpecialOffers(
            int? orderId = null,
            int? specialOfferId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<CustomerOrderSpecialOffer> query = _context.CustomerOrderSpecialOffers;

            // Apply filters if provided
            if (orderId.HasValue)
            {
                query = query.Where(co => co.OrderId == orderId.Value);
            }

            if (specialOfferId.HasValue)
            {
                query = query.Where(co => co.SpecialOfferId == specialOfferId.Value);
            }

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Include related entities for eager loading
            var result = await query.Include(co => co.CustomerOrder)
                                     .Include(co => co.SpecialOffer)
                                     .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get a specific customer order special offer by OrderId.
        /// </summary>
        /// <param name="id">The OrderId of the customer order special offer.</param>
        /// <returns>The customer order special offer if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrderSpecialOffer>> GetCustomerOrderSpecialOffer(int id)
        {
            var customerOrderSpecialOffer = await _context.CustomerOrderSpecialOffers
                                                         .Include(co => co.CustomerOrder)
                                                         .Include(co => co.SpecialOffer)
                                                         .FirstOrDefaultAsync(co => co.OrderId == id);

            if (customerOrderSpecialOffer == null)
            {
                return NotFound();
            }

            return customerOrderSpecialOffer;
        }

        /// <summary>
        /// Update a customer order special offer.
        /// </summary>
        /// <param name="id">The OrderId of the customer order special offer to update.</param>
        /// <param name="customerOrderSpecialOffer">The updated customer order special offer object.</param>
        /// <returns>No content if successful, or BadRequest if the request is invalid.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerOrderSpecialOffer(int id, CustomerOrderSpecialOffer customerOrderSpecialOffer)
        {
            if (id != customerOrderSpecialOffer.OrderId)
            {
                return BadRequest("OrderId mismatch.");
            }

            // Validate the existence of related entities (CustomerOrder and SpecialOffer)
            var customerOrderExists = await _context.CustomerOrders.AnyAsync(co => co.OrderId == customerOrderSpecialOffer.OrderId);
            var specialOfferExists = await _context.SpecialOffers.AnyAsync(so => so.SpecialOfferId == customerOrderSpecialOffer.SpecialOfferId);

            if (!customerOrderExists || !specialOfferExists)
            {
                return NotFound("Either the CustomerOrder or SpecialOffer does not exist.");
            }

            _context.Entry(customerOrderSpecialOffer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerOrderSpecialOfferExists(id))
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
        /// Create a new customer order special offer.
        /// </summary>
        /// <param name="customerOrderSpecialOffer">The customer order special offer object to create.</param>
        /// <returns>The created customer order special offer object.</returns>
        [HttpPost]
        public async Task<ActionResult<CustomerOrderSpecialOffer>> PostCustomerOrderSpecialOffer(CustomerOrderSpecialOffer customerOrderSpecialOffer)
        {
            // Validate that the related entities (CustomerOrder and SpecialOffer) exist
            var customerOrderExists = await _context.CustomerOrders.AnyAsync(co => co.OrderId == customerOrderSpecialOffer.OrderId);
            var specialOfferExists = await _context.SpecialOffers.AnyAsync(so => so.SpecialOfferId == customerOrderSpecialOffer.SpecialOfferId);

            if (!customerOrderExists || !specialOfferExists)
            {
                return NotFound("Either the CustomerOrder or SpecialOffer does not exist.");
            }

            // Check if the combination of OrderId and SpecialOfferId already exists
            var existingOffer = await _context.CustomerOrderSpecialOffers
                                              .FirstOrDefaultAsync(co => co.OrderId == customerOrderSpecialOffer.OrderId &&
                                                                          co.SpecialOfferId == customerOrderSpecialOffer.SpecialOfferId);

            if (existingOffer != null)
            {
                return Conflict("This special offer is already applied to the specified order.");
            }

            _context.CustomerOrderSpecialOffers.Add(customerOrderSpecialOffer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerOrderSpecialOffer", new { id = customerOrderSpecialOffer.OrderId }, customerOrderSpecialOffer);
        }

        /// <summary>
        /// Delete a customer order special offer by OrderId.
        /// </summary>
        /// <param name="id">The OrderId of the customer order special offer to delete.</param>
        /// <returns>No content if successful, or NotFound if the customer order special offer does not exist.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerOrderSpecialOffer(int id)
        {
            var customerOrderSpecialOffer = await _context.CustomerOrderSpecialOffers.FindAsync(id);
            if (customerOrderSpecialOffer == null)
            {
                return NotFound();
            }

            _context.CustomerOrderSpecialOffers.Remove(customerOrderSpecialOffer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if a customer order special offer exists by OrderId.
        /// </summary>
        /// <param name="id">The OrderId of the customer order special offer.</param>
        /// <returns>True if the customer order special offer exists, otherwise false.</returns>
        private bool CustomerOrderSpecialOfferExists(int id)
        {
            return _context.CustomerOrderSpecialOffers.Any(e => e.OrderId == id);
        }
    }
}
