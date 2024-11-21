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
    public class CustomerOrderProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerOrderProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerOrderProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrderProduct>>> GetCustomerOrderProducts()
        {
            return await _context.CustomerOrderProducts.ToListAsync();
        }

        // GET: api/CustomerOrderProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrderProduct>> GetCustomerOrderProduct(int id)
        {
            var customerOrderProduct = await _context.CustomerOrderProducts.FindAsync(id);

            if (customerOrderProduct == null)
            {
                return NotFound();
            }

            return customerOrderProduct;
        }

        // PUT: api/CustomerOrderProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerOrderProduct(int id, CustomerOrderProduct customerOrderProduct)
        {
            if (id != customerOrderProduct.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(customerOrderProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerOrderProductExists(id))
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

        // POST: api/CustomerOrderProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerOrderProduct>> PostCustomerOrderProduct(CustomerOrderProduct customerOrderProduct)
        {
            _context.CustomerOrderProducts.Add(customerOrderProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerOrderProductExists(customerOrderProduct.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomerOrderProduct", new { id = customerOrderProduct.OrderId }, customerOrderProduct);
        }

        // DELETE: api/CustomerOrderProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerOrderProduct(int id)
        {
            var customerOrderProduct = await _context.CustomerOrderProducts.FindAsync(id);
            if (customerOrderProduct == null)
            {
                return NotFound();
            }

            _context.CustomerOrderProducts.Remove(customerOrderProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerOrderProductExists(int id)
        {
            return _context.CustomerOrderProducts.Any(e => e.OrderId == id);
        }
    }
}
