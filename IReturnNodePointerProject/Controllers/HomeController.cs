using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

namespace IReturnNodePointerProject.Controllers
{
    public class HomeController : Controller
    {
		private readonly OnlineStoreContext _storeContext;
		public HomeController(OnlineStoreContext context) {
			_storeContext = context;
		}

		//Get Items
		public async Task<IActionResult> Details(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var product = await _storeContext.Products.FirstOrDefaultAsync(m => m.ID == id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

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