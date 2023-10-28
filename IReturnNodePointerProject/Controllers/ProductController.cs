using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace IReturnNodePointerProject.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _storeContext;
		private static prodAmalgam SelectedProduct;
		private static hiddenData HiddenData;
		public ProductController(ApplicationDbContext context)
		{
			SelectedProduct = new prodAmalgam();
			_storeContext = context;
		}
		[HttpGet]
		public async Task <IActionResult> Index(int productID)
		{
			//calling the database
			var gg = _storeContext.Genre.AsQueryable();
			var pd = _storeContext.Product.AsQueryable();
			var st = _storeContext.Stocktake.AsQueryable();
			var sc = _storeContext.Source.AsQueryable();
			//genres
            var bg = _storeContext.Book_genre.AsQueryable();
            var jg = _storeContext.Game_genre.AsQueryable();
            var mg = _storeContext.Movie_genre.AsQueryable();

            //finding the data
            var _pd = pd.Where(pd => pd.ID == productID).ToArray();
			var _st = st.Where(st => st.ProductId == productID).ToArray();
			var _gg = gg.Where(gg => gg.genreID == _pd[0].Genre).ToArray()[0];


            if (_pd.Length > 0) {
				SelectedProduct.ProductID = productID;
                SelectedProduct.Name = _pd[0].Name;
                SelectedProduct.Description = _pd[0].Description;
                SelectedProduct.price = _st[0].Price;
                SelectedProduct.Author = _pd[0].Author;
                SelectedProduct.Genre = _gg.Name;
				SelectedProduct.GenreID = _gg.genreID;
				SelectedProduct.StokNum = _st[0].Quantity;
				//
				SelectedProduct.hidden = new hiddenData();
				//SelectedProduct.hidden.sources = sc.ToList();
				SelectedProduct.hidden.genres = gg.ToList();
				SelectedProduct.hidden.bookGen = bg.ToList();
				SelectedProduct.hidden.movieGen = mg.ToList();
				SelectedProduct.hidden.gameGen = jg.ToList();
				if (_st[0].Quantity > 0)
				{
					SelectedProduct.Stock = "In Stock";
				}
				else
				{
					SelectedProduct.Stock = "Out Of Stock";
				}
            }
			else
			{
                SelectedProduct.Name = "Name";
                SelectedProduct.Description = "Description";
                SelectedProduct.price = 0.00;
                SelectedProduct.Author = "Creator";
                SelectedProduct.Genre = "Type";
            }

            ViewBag.SelectedProduct = SelectedProduct;
            return View(SelectedProduct);
		}
		[HttpPost]
		public async Task<IActionResult> Index(prodAmalgam newData)
		{
            //calling the database
            var gg = _storeContext.Genre.AsQueryable();
            var pd = _storeContext.Product.AsQueryable();
            var st = _storeContext.Stocktake.AsQueryable();
            var sc = _storeContext.Source.AsQueryable();
            //genres
            var bg = _storeContext.Book_genre.AsQueryable();
            var jg = _storeContext.Game_genre.AsQueryable();
            var mg = _storeContext.Movie_genre.AsQueryable();

            var _pd = pd.Where(pd => pd.ID == newData.ProductID).ToArray()[0];
            var _st = st.Where(st => st.ProductId == newData.ProductID).ToArray()[0];
			//the stocktake
			_st.Price = newData.price;
			_st.Quantity = newData.Quantity;
			//product
			_pd.Author = newData.Author;
			_pd.Description = newData.Description;
			_pd.Name = newData.Name;
			_pd.LastUpdated = DateTime.Now;
			//hidden
			newData.hidden = new hiddenData();
            newData.hidden.genres = gg.ToList();
            newData.hidden.bookGen = bg.ToList();
            newData.hidden.movieGen = mg.ToList();
            newData.hidden.gameGen = jg.ToList();

            //Console.WriteLine(Request.Form["Genre"]);
            _pd.Genre = Convert.ToInt32(Request.Form );
            //fancy stuff

            //updateing
            _storeContext.Update(_st);
			_storeContext.Update(_pd);
			_storeContext.SaveChanges();
			ViewBag.SelectedProduct = newData;
            return View(newData);
		}


        [HttpPost]
		public void AddToCart(int productID)
		{
			Console.WriteLine(productID);
			var JsOBJ = "";
			//creating a cookie to fill in with the void
			if (string.IsNullOrEmpty( HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart") )) 
			{
				var Cart = new cart();
				Cart.productIDs.Add(productID);
				JsOBJ = JsonConvert.SerializeObject(Cart);
			}
			else
			{
				var Cart = JsonConvert.DeserializeObject<cart>(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart"));
				Cart.productIDs.Add(productID);
				JsOBJ = JsonConvert.SerializeObject(Cart);
			}
			HttpContext.Session.SetString("BlockBuster_2_Electric_Boogaloo_Cart", JsOBJ);
			Console.WriteLine(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart"));
		}
	}
}
