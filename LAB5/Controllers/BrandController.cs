using System.Threading.Tasks;
using LAB5.Services;
using Microsoft.AspNetCore.Mvc;

namespace LAB5.Controllers
{
    public class BrandController : Controller
    {
        private readonly BrandService _brandService;

        public BrandController(BrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetBrandsAsync();
            return View(brands);
        }

        public async Task<IActionResult> Details(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
    }
}
