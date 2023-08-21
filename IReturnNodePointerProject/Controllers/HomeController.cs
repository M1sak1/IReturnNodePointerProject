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
		public async Task<IActionResult> Index(string searchString)
		{
			var products = _storeContext.Product;
			var pList = products.ToList();

			return View(pList);
		//if (string.IsNullOrEmpty(searchString)) { }
		//	return  != null ?
		//		View(await _storeContext.Product.ToListAsync() ) :
		//		Problem("Entity set 'MvcMovieContext.Movie'  is null.");
		}

		public async Task<IActionResult> Details(int? id) { 
			if (id == null) {
				 return NotFound();
			}
			var prod = await _storeContext.Product.FirstOrDefaultAsync(m => m.ID == id);
			if (prod == null) {
				return NotFound();
			}
			return View(prod);
		}

		// 
		// GET: /Home/
		// GET: /Home/Welcome/3?name=Andrew
		public IActionResult Welcome(string name, int numTimes = 1)
		{
			ViewData["Message"] = "Hello " + name;
			ViewData["NumTimes"] = numTimes;
			return View();
		}
	}
}