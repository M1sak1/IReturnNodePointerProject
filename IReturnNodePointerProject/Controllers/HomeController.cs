using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace IReturnNodePointerProject.Controllers
{
    public class HomeController : Controller
    {
        // 
        // GET: /Home/
        public IActionResult Index()
        {
            return View();
        }
		// 
		// GET: /Home/Welcome/3?name=Andrew
		public IActionResult Welcome(string name, int numTimes = 1)
		{
			ViewData["Message"] = "Hello " + name;
			ViewData["NumTimes"] = numTimes;
			return View();
		}
	}
}