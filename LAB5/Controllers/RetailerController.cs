using LAB5.Models;
using LAB5.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LAB5.Controllers
{
    public class RetailerController : Controller
    {
        private readonly RetailerService _retailerService;

        public RetailerController(RetailerService retailerService)
        {
            _retailerService = retailerService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var retailers = await _retailerService.GetRetailersAsync();
                return View(retailers);
            }
            catch
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var retailer = await _retailerService.GetRetailerByIdAsync(id);
                if (retailer == null)
                {
                    return NotFound();
                }
                return View(retailer);
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
