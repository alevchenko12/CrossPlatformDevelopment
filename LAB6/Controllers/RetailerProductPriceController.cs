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
    public class RetailerProductPriceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RetailerProductPriceController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all retailer product prices with optional filtering and pagination.
        /// </summary>
        /// <param name="retailerId">Optional filter by retailer ID.</param>
        /// <param name="productId">Optional filter by product ID.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A list of retailer product prices.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetailerProductPrice>>> GetRetailerProductPrices(
            int? retailerId = null,
            int? productId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<RetailerProductPrice> query = _context.RetailerProductPrices;

            // Apply filters if provided
            if (retailerId.HasValue)
            {
                query = query.Where(rpp => rpp.RetailerId == retailerId.Value);
            }

            if (productId.HasValue)
            {
                query = query.Where(rpp => rpp.ProductId == productId.Value);
            }

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Include related entities for eager loading
            var result = await query.Include(rpp => rpp.Product)
                                     .Include(rpp => rpp.Retailer)
                                     .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get a specific retailer product price by product ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>The retailer product price if found, otherwise a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RetailerProductPrice>> GetRetailerProductPrice(int id)
        {
            var retailerProductPrice = await _context.RetailerProductPrices
                                                     .Include(rpp => rpp.Product)
                                                     .Include(rpp => rpp.Retailer)
                                                     .FirstOrDefaultAsync(rpp => rpp.ProductId == id);

            if (retailerProductPrice == null)
            {
                return NotFound();
            }

            return retailerProductPrice;
        }

        /// <summary>
        /// Update a retailer product price.
        /// </summary>
        /// <param name="id">The product ID of the retailer product price to update.</param>
        /// <param name="retailerProductPrice">The updated retailer product price object.</param>
        /// <returns>No content if successful, or BadRequest if the request is invalid.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRetailerProductPrice(int id, RetailerProductPrice retailerProductPrice)
        {
            if (id != retailerProductPrice.ProductId)
            {
                return BadRequest("ProductId mismatch.");
            }

            // Validate the price range (e.g., MinPrice cannot be greater than MaxPrice)
            if (retailerProductPrice.MinPrice > retailerProductPrice.MaxPrice)
            {
                return BadRequest("MinPrice cannot be greater than MaxPrice.");
            }

            _context.Entry(retailerProductPrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RetailerProductPriceExists(id))
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
        /// Create a new retailer product price.
        /// </summary>
        /// <param name="retailerProductPrice">The retailer product price object to create.</param>
        /// <returns>The created retailer product price object.</returns>
        [HttpPost]
        public async Task<ActionResult<RetailerProductPrice>> PostRetailerProductPrice(RetailerProductPrice retailerProductPrice)
        {
            // Validate that the price range is correct
            if (retailerProductPrice.MinPrice > retailerProductPrice.MaxPrice)
            {
                return BadRequest("MinPrice cannot be greater than MaxPrice.");
            }

            // Check if the combination of ProductId and RetailerId already exists
            var existingPrice = await _context.RetailerProductPrices
                                              .FirstOrDefaultAsync(rpp => rpp.ProductId == retailerProductPrice.ProductId &&
                                                                          rpp.RetailerId == retailerProductPrice.RetailerId);

            if (existingPrice != null)
            {
                return Conflict("The price for this product and retailer already exists.");
            }

            _context.RetailerProductPrices.Add(retailerProductPrice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRetailerProductPrice", new { id = retailerProductPrice.ProductId }, retailerProductPrice);
        }

        /// <summary>
        /// Delete a retailer product price.
        /// </summary>
        /// <param name="id">The product ID of the retailer product price to delete.</param>
        /// <returns>No content if successful, or NotFound if the product price does not exist.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRetailerProductPrice(int id)
        {
            var retailerProductPrice = await _context.RetailerProductPrices.FindAsync(id);
            if (retailerProductPrice == null)
            {
                return NotFound();
            }

            _context.RetailerProductPrices.Remove(retailerProductPrice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if a retailer product price exists by product ID.
        /// </summary>
        /// <param name="id">The product ID of the retailer product price.</param>
        /// <returns>True if the retailer product price exists, otherwise false.</returns>
        private bool RetailerProductPriceExists(int id)
        {
            return _context.RetailerProductPrices.Any(e => e.ProductId == id);
        }
    }
}
