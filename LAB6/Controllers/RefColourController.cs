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
    public class RefColourController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RefColourController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the reference colours with optional filtering and pagination.
        /// </summary>
        /// <param name="search">Search term for ColourDescription or ColourCode.</param>
        /// <param name="pageNumber">Page number for pagination.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>A list of RefColours.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefColour>>> GetRefColours(
            string search = null,
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            IQueryable<RefColour> query = _context.RefColours;

            // Optional search filter for ColourDescription or ColourCode
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.ColourDescription.Contains(search) || c.ColourCode.Contains(search));
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
        /// Get a specific reference colour by its ColourCode.
        /// </summary>
        /// <param name="id">The ColourCode of the reference colour.</param>
        /// <returns>The RefColour object if found, or a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RefColour>> GetRefColour(string id)
        {
            var refColour = await _context.RefColours
                                          .FirstOrDefaultAsync(c => c.ColourCode == id);

            if (refColour == null)
            {
                return NotFound();
            }

            return refColour;
        }

        /// <summary>
        /// Update a reference colour's details.
        /// </summary>
        /// <param name="id">The ColourCode of the reference colour to update.</param>
        /// <param name="refColour">The updated RefColour object.</param>
        /// <returns>NoContent if update is successful, or BadRequest/NotFound for errors.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefColour(string id, RefColour refColour)
        {
            if (id != refColour.ColourCode)
            {
                return BadRequest("ColourCode mismatch.");
            }

            _context.Entry(refColour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefColourExists(id))
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
        /// Add a new reference colour.
        /// </summary>
        /// <param name="refColour">The new RefColour object to add.</param>
        /// <returns>The created RefColour object.</returns>
        [HttpPost]
        public async Task<ActionResult<RefColour>> PostRefColour(RefColour refColour)
        {
            _context.RefColours.Add(refColour);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RefColourExists(refColour.ColourCode))
                {
                    return Conflict("ColourCode already exists.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRefColour", new { id = refColour.ColourCode }, refColour);
        }

        /// <summary>
        /// Delete a specific reference colour by ColourCode.
        /// </summary>
        /// <param name="id">The ColourCode of the reference colour to delete.</param>
        /// <returns>NoContent if deletion is successful, or NotFound for errors.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRefColour(string id)
        {
            var refColour = await _context.RefColours.FindAsync(id);

            if (refColour == null)
            {
                return NotFound();
            }

            _context.RefColours.Remove(refColour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if a reference colour exists by ColourCode.
        /// </summary>
        /// <param name="id">The ColourCode to check for existence.</param>
        /// <returns>True if exists, false otherwise.</returns>
        private bool RefColourExists(string id)
        {
            return _context.RefColours.Any(e => e.ColourCode == id);
        }
    }
}
