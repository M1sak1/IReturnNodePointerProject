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
		public ProductController(ApplicationDbContext context)
		{
			SelectedProduct = new prodAmalgam();
			_storeContext = context;
		}
		public async Task <IActionResult> Index(int productID)
		{
			Console.WriteLine("Here");
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
				SelectedProduct.StokNum = _st[0].Quantity;
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
		public async Task<IActionResult> updateData(prodAmalgam newData)
		{
            var gg = _storeContext.Genre.AsQueryable();
            var pd = _storeContext.Product.AsQueryable();
            var st = _storeContext.Stocktake.AsQueryable();
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
            //fancy stuff
			//updateing
			_storeContext.Update(_st);
			_storeContext.Update(_pd);

            return View();
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
