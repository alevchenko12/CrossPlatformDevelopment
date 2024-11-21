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

        /// <summary>
        /// Get all customer order products with optional filtering and pagination.
        /// </summary>
        /// <param name="orderId">Optional filter by order ID.</param>
        /// <param name="productId">Optional filter by product ID.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A list of customer order products.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrderProduct>>> GetCustomerOrderProducts(
            int? orderId = null,
            int? productId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<CustomerOrderProduct> query = _context.CustomerOrderProducts;

            // Apply filters if provided
            if (orderId.HasValue)
            {
                query = query.Where(c => c.OrderId == orderId.Value);
            }

            if (productId.HasValue)
            {
                query = query.Where(c => c.ProductId == productId.Value);
            }

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Include related entities for eager loading
            var result = await query.Include(c => c.CustomerOrder)
                                     .Include(c => c.Product)
                                     .Include(c => c.Retailer)
                                     .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get a specific customer order product by id.
        /// </summary>
        /// <param name="id">The id of the customer order product.</param>
        /// <returns>The customer order product if found, otherwise a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrderProduct>> GetCustomerOrderProduct(int id)
        {
            var customerOrderProduct = await _context.CustomerOrderProducts
                                                     .Include(c => c.CustomerOrder)
                                                     .Include(c => c.Product)
                                                     .Include(c => c.Retailer)
                                                     .FirstOrDefaultAsync(c => c.OrderId == id);

            if (customerOrderProduct == null)
            {
                return NotFound();
            }

            return customerOrderProduct;
        }

        /// <summary>
        /// Update a customer order product.
        /// </summary>
        /// <param name="id">The id of the customer order product to update.</param>
        /// <param name="customerOrderProduct">The updated customer order product object.</param>
        /// <returns>No content if successful, or BadRequest if there are errors.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerOrderProduct(int id, CustomerOrderProduct customerOrderProduct)
        {
            if (id != customerOrderProduct.OrderId)
            {
                return BadRequest("Order ID mismatch.");
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

        /// <summary>
        /// Create a new customer order product.
        /// </summary>
        /// <param name="customerOrderProduct">The customer order product object to create.</param>
        /// <returns>The created customer order product object.</returns>
        [HttpPost]
        public async Task<ActionResult<CustomerOrderProduct>> PostCustomerOrderProduct(CustomerOrderProduct customerOrderProduct)
        {
            _context.CustomerOrderProducts.Add(customerOrderProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (CustomerOrderProductExists(customerOrderProduct.OrderId))
                {
                    return Conflict(new { message = "Customer order product already exists." });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomerOrderProduct", new { id = customerOrderProduct.OrderId }, customerOrderProduct);
        }

        /// <summary>
        /// Delete a customer order product.
        /// </summary>
        /// <param name="id">The id of the customer order product to delete.</param>
        /// <returns>No content if successful, or NotFound if the product does not exist.</returns>
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

        /// <summary>
        /// Check if a customer order product exists by id.
        /// </summary>
        /// <param name="id">The id of the customer order product to check.</param>
        /// <returns>True if the customer order product exists, otherwise false.</returns>
        private bool CustomerOrderProductExists(int id)
        {
            return _context.CustomerOrderProducts.Any(e => e.OrderId == id);
        }
    }
}
