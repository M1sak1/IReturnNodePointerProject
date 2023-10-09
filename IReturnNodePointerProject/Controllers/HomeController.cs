using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.XEvent;
using Microsoft.VisualBasic;
using System.Drawing;
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
		public async Task<IActionResult> Index(int UserID, string Product, string Genre)
		{
			ViewBag.GenSortParam = String.IsNullOrEmpty(Product);
			//sortingME = "Books";
			if (UserID != -1)
			{
				HttpContext.Session.SetString("AccessLevel", "Patron");
				HttpContext.Session.SetInt32("UserID", UserID);
			}
			var bg = _storeContext.Book_genre.AsQueryable();
			var jg = _storeContext.Game_genre.AsQueryable();
			var mg = _storeContext.Movie_genre.AsQueryable();

			var gg = _storeContext.Genre.AsQueryable();
			var pd = _storeContext.Product.AsQueryable();
			var st = _storeContext.Stocktake.AsQueryable();

			if (Product != null)
			{
				if (Product != "")
				{
					var target = gg.Where(gg => gg.Name == Product).ToArray()[0].genreID;
					pd = pd.Where(pd => pd.Genre == target);
					switch(Product){
						case "Books":
							allLists.bookGen = bg.ToList();
							break;
						case "Movies":
							allLists.movieGen = mg.ToList();
							break;
						case "Games":
							allLists.gameGen = jg.ToList();	
							break;
						default:
							break;
					}

					if (Genre != null)
					{ 
						try
						{
							var subTarget = 0;			  
							switch (Product)
							{
								case "Books":
									subTarget = bg.Where(bg => bg.Name == Genre).ToArray()[0].subGenreID;
									break;
								case "Games":
									subTarget = jg.Where(jg => jg.Name == Genre).ToArray()[0].subGenreID;
									break;
								case "Movies":
									subTarget = mg.Where(mg => mg.Name == Genre).ToArray()[0].subGenreID;
									break;
								default:
									break;
							}
							pd = pd.Where(pd => pd.subGenre == subTarget && pd.Genre == target);
						}
						catch (Exception) {
							pd = pd.Where(pd => pd.Genre == target);
						}
					}
				}
			}
			allLists.Stonks = st.ToList();
			allLists.genres = gg.ToList();
			allLists.Products = pd.ToList();
			return PartialView(allLists);
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
			//this is a stupid way to do this but it works lol O(n)
			var price = 0d;
			var loc = allLists.Stonks.ElementAt(0);
			for (var i = 0; i < allLists.Stonks.Count; i++){
				loc = allLists.Stonks.ElementAt(i);
				if (loc.ItemId == itemID){
					//Console.WriteLine("found");
					price = loc.Price; 
					break; //just in case the price is 0, O(n)
				}
			}
			return price;
		}
    }
}
public class ListViewModel {
	public List<Book_genre> bookGen { get; set; }
	public List<Game_genre> gameGen { get; set; }
	public List<Movie_genre> movieGen { get; set; }
	public List<Genre> genres { get; set; }
	public List<Product> Products { get; set; }
	public List<Stocktake> Stonks { get; set; }
}