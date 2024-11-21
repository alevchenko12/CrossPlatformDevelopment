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
    public class RefProductTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RefProductTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all product types with optional filtering and pagination.
        /// </summary>
        /// <param name="search">Search term for ProductTypeCode or ProductTypeDescription.</param>
        /// <param name="pageNumber">Page number for pagination.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>A list of RefProductTypes.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefProductType>>> GetRefProductTypes(
            string search = null,
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            IQueryable<RefProductType> query = _context.RefProductTypes;

            // Optional search filter for ProductTypeCode or ProductTypeDescription
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(pt => pt.ProductTypeCode.Contains(search) || pt.ProductTypeDescription.Contains(search));
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
        /// Get a specific product type by ProductTypeCode.
        /// </summary>
        /// <param name="id">The ProductTypeCode of the reference product type.</param>
        /// <returns>The RefProductType object if found, or a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RefProductType>> GetRefProductType(string id)
        {
            var refProductType = await _context.RefProductTypes
                                                .Include(pt => pt.Products) // Eager loading the related Products
                                                .FirstOrDefaultAsync(pt => pt.ProductTypeCode == id);

            if (refProductType == null)
            {
                return NotFound();
            }

            return refProductType;
        }

        /// <summary>
        /// Update a product type's details.
        /// </summary>
        /// <param name="id">The ProductTypeCode of the product type to update.</param>
        /// <param name="refProductType">The updated RefProductType object.</param>
        /// <returns>NoContent if update is successful, or BadRequest/NotFound for errors.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefProductType(string id, RefProductType refProductType)
        {
            if (id != refProductType.ProductTypeCode)
            {
                return BadRequest("ProductTypeCode mismatch.");
            }

            _context.Entry(refProductType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefProductTypeExists(id))
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
        /// Add a new product type.
        /// </summary>
        /// <param name="refProductType">The new RefProductType object to add.</param>
        /// <returns>The created RefProductType object.</returns>
        [HttpPost]
        public async Task<ActionResult<RefProductType>> PostRefProductType(RefProductType refProductType)
        {
            _context.RefProductTypes.Add(refProductType);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RefProductTypeExists(refProductType.ProductTypeCode))
                {
                    return Conflict("ProductTypeCode already exists.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRefProductType", new { id = refProductType.ProductTypeCode }, refProductType);
        }

        /// <summary>
        /// Delete a specific product type by ProductTypeCode.
        /// </summary>
        /// <param name="id">The ProductTypeCode of the reference product type to delete.</param>
        /// <returns>NoContent if deletion is successful, or NotFound for errors.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRefProductType(string id)
        {
            var refProductType = await _context.RefProductTypes.FindAsync(id);

            if (refProductType == null)
            {
                return NotFound();
            }

            _context.RefProductTypes.Remove(refProductType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if a reference product type exists by ProductTypeCode.
        /// </summary>
        /// <param name="id">The ProductTypeCode to check for existence.</param>
        /// <returns>True if exists, false otherwise.</returns>
        private bool RefProductTypeExists(string id)
        {
            return _context.RefProductTypes.Any(e => e.ProductTypeCode == id);
        }
    }
}
