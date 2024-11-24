using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LAB5.Services;

namespace LAB5.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? brandId, string? productTypeCode, string? search, int? pageNumber, int? pageSize)
        {
            var products = await _productService.GetProductsAsync(brandId, productTypeCode, search, pageNumber, pageSize);
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
