using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IReturnNodePointerProject.Controllers
{
    public class cartController : Controller
    {
			public IActionResult Cart()
        {
            return View();
        }
    }
}
