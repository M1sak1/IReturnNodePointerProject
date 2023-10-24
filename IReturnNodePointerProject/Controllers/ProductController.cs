using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;

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
		public string AddToCart(){
			Console.WriteLine("Booo!");
			return "Boo";
		}
	}
	public class prodAmalgam
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Double price { get; set; }
		public string Author { get; set; }
		public string Genre { get; set; }
		public string Stock { get; set; }
	}
}
