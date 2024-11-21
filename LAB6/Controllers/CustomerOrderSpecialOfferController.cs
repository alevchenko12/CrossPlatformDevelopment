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

        // GET: api/CustomerOrderSpecialOffer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrderSpecialOffer>>> GetCustomerOrderSpecialOffers()
        {
            return await _context.CustomerOrderSpecialOffers.ToListAsync();
        }

        // GET: api/CustomerOrderSpecialOffer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrderSpecialOffer>> GetCustomerOrderSpecialOffer(int id)
        {
            var customerOrderSpecialOffer = await _context.CustomerOrderSpecialOffers.FindAsync(id);

            if (customerOrderSpecialOffer == null)
            {
                return NotFound();
            }

            return customerOrderSpecialOffer;
        }

        // PUT: api/CustomerOrderSpecialOffer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerOrderSpecialOffer(int id, CustomerOrderSpecialOffer customerOrderSpecialOffer)
        {
            if (id != customerOrderSpecialOffer.OrderId)
            {
                return BadRequest();
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

        // POST: api/CustomerOrderSpecialOffer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerOrderSpecialOffer>> PostCustomerOrderSpecialOffer(CustomerOrderSpecialOffer customerOrderSpecialOffer)
        {
            _context.CustomerOrderSpecialOffers.Add(customerOrderSpecialOffer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerOrderSpecialOfferExists(customerOrderSpecialOffer.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomerOrderSpecialOffer", new { id = customerOrderSpecialOffer.OrderId }, customerOrderSpecialOffer);
        }

        // DELETE: api/CustomerOrderSpecialOffer/5
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

        private bool CustomerOrderSpecialOfferExists(int id)
        {
            return _context.CustomerOrderSpecialOffers.Any(e => e.OrderId == id);
        }
    }
}
