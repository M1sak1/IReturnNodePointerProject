using Microsoft.AspNetCore.Mvc;

namespace IReturnNodePointerProject.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
