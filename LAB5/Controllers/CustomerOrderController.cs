using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LAB5.Services;
using LAB5.Models;

namespace LAB5.Controllers
{
    public class CustomerOrderController : Controller
    {
        private readonly CustomerOrderService _orderService;

        public CustomerOrderController(CustomerOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(
            DateTime? startDate,
            DateTime? endDate,
            string? statusCodes,
            string? startsWith,
            string? endsWith)
        {
            List<string> statusCodeList = null;

            if (!string.IsNullOrEmpty(statusCodes))
            {
                statusCodeList = new List<string>(statusCodes.Split(','));
            }

            var orders = await _orderService.GetOrdersAsync(
                startDate,
                endDate,
                statusCodeList,
                startsWith,
                endsWith);

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
