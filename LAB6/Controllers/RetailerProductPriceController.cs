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

        // GET: api/RetailerProductPrice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetailerProductPrice>>> GetRetailerProductPrices()
        {
            return await _context.RetailerProductPrices.ToListAsync();
        }

        // GET: api/RetailerProductPrice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RetailerProductPrice>> GetRetailerProductPrice(int id)
        {
            var retailerProductPrice = await _context.RetailerProductPrices.FindAsync(id);

            if (retailerProductPrice == null)
            {
                return NotFound();
            }

            return retailerProductPrice;
        }

        // PUT: api/RetailerProductPrice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRetailerProductPrice(int id, RetailerProductPrice retailerProductPrice)
        {
            if (id != retailerProductPrice.ProductId)
            {
                return BadRequest();
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

        // POST: api/RetailerProductPrice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RetailerProductPrice>> PostRetailerProductPrice(RetailerProductPrice retailerProductPrice)
        {
            _context.RetailerProductPrices.Add(retailerProductPrice);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RetailerProductPriceExists(retailerProductPrice.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRetailerProductPrice", new { id = retailerProductPrice.ProductId }, retailerProductPrice);
        }

        // DELETE: api/RetailerProductPrice/5
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

        private bool RetailerProductPriceExists(int id)
        {
            return _context.RetailerProductPrices.Any(e => e.ProductId == id);
        }
    }
}
