using Microsoft.AspNetCore.Mvc;

namespace IReturnNodePointerProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Name = "Mary";
            ViewBag.FV = 999999999.99;
            return View();
        }
    }
}
