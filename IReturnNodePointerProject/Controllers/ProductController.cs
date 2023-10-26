using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Mvc;
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
		public ProductController(ApplicationDbContext context)
		{
			SelectedProduct = new prodAmalgam();
			_storeContext = context;
		}
		public async Task <IActionResult> Index(int productID)
		{
			var gg = _storeContext.Genre.AsQueryable();
			var pd = _storeContext.Product.AsQueryable();
			var st = _storeContext.Stocktake.AsQueryable();

			var _pd = pd.Where(pd => pd.ID == productID).ToArray();
			var _st = st.Where(st => st.ProductId == productID).ToArray();

			if(_pd.Length > 0) {
				SelectedProduct.ProductID = productID;
                SelectedProduct.Name = _pd[0].Name;
                SelectedProduct.Description = _pd[0].Description;
                SelectedProduct.price = _st[0].Price;
                SelectedProduct.Author = _pd[0].Author;
                SelectedProduct.Genre = gg.Where(gg => gg.genreID == _pd[0].Genre).ToArray()[0].Name;
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
                SelectedProduct.Name = "Not Found";
                SelectedProduct.Description = "Not Found";
                SelectedProduct.price = 0.00;
                SelectedProduct.Author = "Not Found";
                SelectedProduct.Genre = "Not Found";
            }
			return View(SelectedProduct);
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
