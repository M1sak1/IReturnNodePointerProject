using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IReturnNodePointerProject.Controllers
{
    public class CartController : Controller
    {
		private readonly ApplicationDbContext _storeContext;

        public CartController(ApplicationDbContext storeContext)
        {
            _storeContext = storeContext;
        }

		public IActionResult Cart()
        {
			var gg = _storeContext.Genre.AsQueryable();
			var pd = _storeContext.Product.AsQueryable();
			var st = _storeContext.Stocktake.AsQueryable();
			//i dont need most of the things in this class, but aslong as i am careful with it, it saves creating a bespoke class for this page
			var cart = new List<prodAmalgam>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart")))
            {
                Console.WriteLine("Gix Failed");
                return View();
            }
            else
            {
                var SesCart = JsonConvert.DeserializeObject<cart>(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart"));
				Console.WriteLine(SesCart.productIDs);
				SesCart.productIDs.OrderBy(o => o).ToList();
                Console.WriteLine(SesCart.productIDs);
                for (var i = 0; i < SesCart.productIDs.Count; i++)
                {
                    try
                    {
						var _pd = pd.Where(pd => pd.ID == SesCart.productIDs[i]).ToArray()[0];
						var _st = st.Where(st => st.ProductId == SesCart.productIDs[i]).ToArray()[0];
						var prod = new prodAmalgam();
                        prod.price = _st.Price;
                        prod.Name = _pd.Name;
                    }
                    catch 
                    {
                    
                    }
                }
				return View();
            }
            //this should never be used
            return View();
            
        }
    }
}
