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
		public async Task<IActionResult> CreateView(string searchTerm)
		{
			return _storeContext.Product != null ?
						View(await _storeContext.Product.ToListAsync()) :
						Problem("Entity set '_storContext.Movie' is null.");
		}

		// 
		// GET: /Home/
		public IActionResult Index()
		{
			try
			{
				Console.WriteLine("Here");
				return ((IActionResult)CreateView("A"));
			}
			catch(Exception ex)
			{
				return View();
			}
		} 
		// GET: /Home/Welcome/3?name=Andrew
		public IActionResult Welcome(string name, int numTimes = 1)
		{
			ViewData["Message"] = "Hello " + name;
			ViewData["NumTimes"] = numTimes;
			return View();
		}
	}
}