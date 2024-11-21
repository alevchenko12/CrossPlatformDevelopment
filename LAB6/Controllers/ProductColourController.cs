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
    public class ProductColourController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductColourController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all product colours with optional filtering and pagination.
        /// </summary>
        /// <param name="productId">Optional filter by ProductId.</param>
        /// <param name="colourCode">Optional filter by ColourCode.</param>
        /// <param name="availability">Optional filter by Availability.</param>
        /// <param name="pageNumber">Page number for pagination.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>A list of ProductColour objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductColour>>> GetProductColours(
            int? productId = null,
            string colourCode = null,
            bool? availability = null,
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            IQueryable<ProductColour> query = _context.ProductColours.Include(pc => pc.RefColour).Include(pc => pc.Product);

            // Filtering
            if (productId.HasValue)
            {
                query = query.Where(pc => pc.ProductId == productId.Value);
            }

            if (!string.IsNullOrEmpty(colourCode))
            {
                query = query.Where(pc => pc.ColourCode.Contains(colourCode));
            }

            if (availability.HasValue)
            {
                query = query.Where(pc => pc.Availability == availability.Value);
            }

            // Pagination
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Get a specific product colour by ProductId and ColourCode.
        /// </summary>
        /// <param name="productId">The ProductId of the product.</param>
        /// <param name="colourCode">The ColourCode of the product's colour.</param>
        /// <returns>The ProductColour object if found, or a NotFound result.</returns>
        [HttpGet("{productId}/{colourCode}")]
        public async Task<ActionResult<ProductColour>> GetProductColour(int productId, string colourCode)
        {
            var productColour = await _context.ProductColours
                                               .Include(pc => pc.RefColour)
                                               .Include(pc => pc.Product)
                                               .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.ColourCode == colourCode);

            if (productColour == null)
            {
                return NotFound();
            }

            return productColour;
        }

        /// <summary>
        /// Update a product colour's details.
        /// </summary>
        /// <param name="productId">The ProductId of the product colour to update.</param>
        /// <param name="colourCode">The ColourCode of the product colour to update.</param>
        /// <param name="productColour">The updated ProductColour object.</param>
        /// <returns>NoContent if update is successful, or BadRequest/NotFound for errors.</returns>
        [HttpPut("{productId}/{colourCode}")]
        public async Task<IActionResult> PutProductColour(int productId, string colourCode, ProductColour productColour)
        {
            if (productId != productColour.ProductId || colourCode != productColour.ColourCode)
            {
                return BadRequest("ProductId and ColourCode mismatch.");
            }

            _context.Entry(productColour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductColourExists(productId, colourCode))
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
        /// Add a new product colour.
        /// </summary>
        /// <param name="productColour">The new ProductColour object to add.</param>
        /// <returns>The created ProductColour object.</returns>
        [HttpPost]
        public async Task<ActionResult<ProductColour>> PostProductColour(ProductColour productColour)
        {
            _context.ProductColours.Add(productColour);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductColourExists(productColour.ProductId, productColour.ColourCode))
                {
                    return Conflict("ProductId and ColourCode combination already exists.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductColour", new { productId = productColour.ProductId, colourCode = productColour.ColourCode }, productColour);
        }

        /// <summary>
        /// Delete a product colour by ProductId and ColourCode.
        /// </summary>
        /// <param name="productId">The ProductId of the product colour to delete.</param>
        /// <param name="colourCode">The ColourCode of the product colour to delete.</param>
        /// <returns>NoContent if deletion is successful, or NotFound for errors.</returns>
        [HttpDelete("{productId}/{colourCode}")]
        public async Task<IActionResult> DeleteProductColour(int productId, string colourCode)
        {
            var productColour = await _context.ProductColours
                                               .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.ColourCode == colourCode);

            if (productColour == null)
            {
                return NotFound();
            }

            _context.ProductColours.Remove(productColour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if a product colour exists by ProductId and ColourCode.
        /// </summary>
        /// <param name="productId">The ProductId to check for existence.</param>
        /// <param name="colourCode">The ColourCode to check for existence.</param>
        /// <returns>True if exists, false otherwise.</returns>
        private bool ProductColourExists(int productId, string colourCode)
        {
            return _context.ProductColours.Any(e => e.ProductId == productId && e.ColourCode == colourCode);
        }
    }
}
