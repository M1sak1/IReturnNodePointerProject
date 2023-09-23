using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text.Encodings.Web;
using System.Xml.Serialization;

namespace IReturnNodePointerProject.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _storeContext;
		private static ListViewModel allLists;
		public HomeController(ApplicationDbContext context) {
			allLists = new ListViewModel();
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
			var gg = _storeContext.Genre;
			var pd = _storeContext.Product;
			var st = _storeContext.Stocktake;
			allLists.Stonks = st.ToList();
			allLists.genres = gg.ToList();
			allLists.Products = pd.ToList();
			return View(allLists);
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
		public static double findPrice(int itemID) {
			
			var price = 0d;
			var loc = allLists.Stonks.ElementAt(0);
			for (var i = 0; i < allLists.Stonks.Count; i++){
				loc = allLists.Stonks.ElementAt(i);
				if (loc.ItemId == itemID){
					Console.WriteLine("found");
					price = loc.Price; 
					break; //just in case the price is 0, O(n)
				}
			}
			return price;
		}
    }
}
public class ListViewModel{
	public List<Genre> genres { get; set; }
	public List<Product> Products { get; set; }
	public List<Stocktake> Stonks { get; set; }
}