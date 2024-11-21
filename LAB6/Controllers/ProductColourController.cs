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

        // GET: api/ProductColour
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductColour>>> GetProductColours()
        {
            return await _context.ProductColours.ToListAsync();
        }

        // GET: api/ProductColour/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductColour>> GetProductColour(int id)
        {
            var productColour = await _context.ProductColours.FindAsync(id);

            if (productColour == null)
            {
                return NotFound();
            }

            return productColour;
        }

        // PUT: api/ProductColour/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductColour(int id, ProductColour productColour)
        {
            if (id != productColour.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(productColour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductColourExists(id))
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

        // POST: api/ProductColour
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                if (ProductColourExists(productColour.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductColour", new { id = productColour.ProductId }, productColour);
        }

        // DELETE: api/ProductColour/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductColour(int id)
        {
            var productColour = await _context.ProductColours.FindAsync(id);
            if (productColour == null)
            {
                return NotFound();
            }

            _context.ProductColours.Remove(productColour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductColourExists(int id)
        {
            return _context.ProductColours.Any(e => e.ProductId == id);
        }
    }
}
