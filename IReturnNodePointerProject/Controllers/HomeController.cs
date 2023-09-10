using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text.Encodings.Web;
using System.Xml.Serialization;

namespace IReturnNodePointerProject.Controllers
{
    public class HomeController : Controller
    {
		private readonly OnlineStoreContext _storeContext;
		public HomeController(OnlineStoreContext context) {
			_storeContext = context;
		}

		//Get Items
		//The login page will always go to the store page 
		public async Task<IActionResult> Index(int UserID)
		{
			if (UserID != -1)
			{
				HttpContext.Session.SetInt32("UserID", UserID);
			}
			var products = _storeContext.Product;
			//Console.WriteLine(products);
			var pList = products.ToList();
			return View(pList);
		//if (string.IsNullOrEmpty(searchString)) { }
		//	return  != null ?
		//		View(await _storeContext.Product.ToListAsync() ) :
		//		Problem("Entity set 'MvcMovieContext.Movie'  is null.");
		}
		public static int ConvertToYear(DateTime dt)

		{
			var year = dt.Year;
			return year;
		}
    }
}