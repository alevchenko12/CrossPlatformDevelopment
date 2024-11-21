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

        // GET: api/RefColour
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefColour>>> GetRefColours()
        {
            return await _context.RefColours.ToListAsync();
        }

        // GET: api/RefColour/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RefColour>> GetRefColour(string id)
        {
            var refColour = await _context.RefColours.FindAsync(id);

            if (refColour == null)
            {
                return NotFound();
            }

            return refColour;
        }

        // PUT: api/RefColour/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefColour(string id, RefColour refColour)
        {
            if (id != refColour.ColourCode)
            {
                return BadRequest();
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

        // POST: api/RefColour
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRefColour", new { id = refColour.ColourCode }, refColour);
        }

        // DELETE: api/RefColour/5
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

        private bool RefColourExists(string id)
        {
            return _context.RefColours.Any(e => e.ColourCode == id);
        }
    }
}
