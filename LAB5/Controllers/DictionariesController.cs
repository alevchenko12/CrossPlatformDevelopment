using Microsoft.AspNetCore.Mvc;

namespace LAB5.Controllers
{
    public class DictionariesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
