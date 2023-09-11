using IReturnNodePointerProject.Models;
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
			var vm = new ListViewModel();
			var gg = _storeContext.Genre;
			var pd = _storeContext.Product;
			vm.genres = gg.ToList();
			vm.Products = pd.ToList();
			return View(vm);
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
public class ListViewModel{
	public List<Genre> genres { get; set; }
	public List<Product> Products { get; set; }
	public List<Stocktake> Stonks { get; set; }
}