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
			//Console.WriteLine(products);
			var pList = products.ToList();

			return View(pList);
		//if (string.IsNullOrEmpty(searchString)) { }
		//	return  != null ?
		//		View(await _storeContext.Product.ToListAsync() ) :
		//		Problem("Entity set 'MvcMovieContext.Movie'  is null.");
		}
    }
}