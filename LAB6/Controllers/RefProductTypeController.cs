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

        // GET: api/RefProductType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefProductType>>> GetRefProductTypes()
        {
            return await _context.RefProductTypes.ToListAsync();
        }

        // GET: api/RefProductType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RefProductType>> GetRefProductType(string id)
        {
            var refProductType = await _context.RefProductTypes.FindAsync(id);

            if (refProductType == null)
            {
                return NotFound();
            }

            return refProductType;
        }

        // PUT: api/RefProductType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefProductType(string id, RefProductType refProductType)
        {
            if (id != refProductType.ProductTypeCode)
            {
                return BadRequest();
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

        // POST: api/RefProductType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRefProductType", new { id = refProductType.ProductTypeCode }, refProductType);
        }

        // DELETE: api/RefProductType/5
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

        private bool RefProductTypeExists(string id)
        {
            return _context.RefProductTypes.Any(e => e.ProductTypeCode == id);
        }
    }
}
