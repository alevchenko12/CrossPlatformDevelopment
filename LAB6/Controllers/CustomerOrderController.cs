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
    public class CustomerOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomerOrders(
            string? search,
            string? orderStatusCode,
            int? customerId,
            DateTime? startDate,
            DateTime? endDate,
            int? pageNumber,
            int? pageSize)
        {
            IQueryable<CustomerOrder> query = _context.CustomerOrders
                                                       .Include(co => co.Customer)
                                                       .Include(co => co.CustomerOrderProducts)
                                                       .Include(co => co.CustomerOrderSpecialOffers);

            // Filter by CustomerId
            if (customerId.HasValue)
            {
                query = query.Where(co => co.CustomerId == customerId.Value);
            }

            // Filter by OrderStatusCode
            if (!string.IsNullOrEmpty(orderStatusCode))
            {
                query = query.Where(co => co.OrderStatusCode == orderStatusCode);
            }

            // Filter by OrderDate range
            if (startDate.HasValue)
            {
                query = query.Where(co => co.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(co => co.OrderDate <= endDate.Value);
            }

            // Search by OrderDetails or CustomerName
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(co => co.OrderDetails.Contains(search) ||
                                          co.Customer.CustomerName.Contains(search));
            }

            // Add Pagination
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        // GET: api/CustomerOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrder>> GetCustomerOrder(int id)
        {
            var customerOrder = await _context.CustomerOrders
                                               .Include(co => co.Customer)
                                               .Include(co => co.CustomerOrderProducts)
                                               .Include(co => co.CustomerOrderSpecialOffers)
                                               .FirstOrDefaultAsync(co => co.OrderId == id);

            if (customerOrder == null)
            {
                return NotFound();
            }

            return customerOrder;
        }

        // PUT: api/CustomerOrder/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerOrder(int id, CustomerOrder customerOrder)
        {
            if (id != customerOrder.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(customerOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerOrderExists(id))
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

        // POST: api/CustomerOrder
        [HttpPost]
        public async Task<ActionResult<CustomerOrder>> PostCustomerOrder(CustomerOrder customerOrder)
        {
            _context.CustomerOrders.Add(customerOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerOrder", new { id = customerOrder.OrderId }, customerOrder);
        }

        // DELETE: api/CustomerOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerOrder(int id)
        {
            var customerOrder = await _context.CustomerOrders.FindAsync(id);
            if (customerOrder == null)
            {
                return NotFound();
            }

            _context.CustomerOrders.Remove(customerOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerOrderExists(int id)
        {
            return _context.CustomerOrders.Any(e => e.OrderId == id);
        }
    }
}
