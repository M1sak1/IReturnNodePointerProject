using Antlr.Runtime.Tree;
using IReturnNodePointerProject.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Immutable;

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
            var cart = new CartDeets();
            cart.Cart = new List<prodAmalgam>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart")))
            {
                return View(cart);
            }
            else
            {
                var SesCart = JsonConvert.DeserializeObject<cart>(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart"));
				SesCart.productIDs.Sort();  //THIS IS INTEGERAL. DO NOT TOUCH!
                var prod = new prodAmalgam();
                var prodindx = -1;
                for (var i = 0; i < SesCart.productIDs.Count; i++)
                {
                    try
                    {
                        var _pd = pd.Where(pd => pd.ID == SesCart.productIDs[i]).ToArray()[0];
                        var _st = st.Where(st => st.ProductId == SesCart.productIDs[i]).ToArray()[0];
                        //so if the product has already been displayed, it will just incriment its quantity. however this relys on the list being sorted.
                        if (SesCart.productIDs[i] == prodindx)
                        {
                            prod.Quantity++;
                            cart.cartTotal += Math.Round(_st.Price);

                        }
                        else
                        {
                            //maintaining quantity controll
                            prodindx = SesCart.productIDs[i];
                            prod = new prodAmalgam();
                            //adding the new item
                            cart.cartTotal += Math.Round(_st.Price, 2);
                            prod.price = _st.Price;
                            prod.Author = _pd.Author;
                            prod.Name = _pd.Name;
                            prod.Quantity++;
                            cart.Cart.Add(prod);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("shit broke lol");
                    }
                }
				return View(cart);
            }
            //this should never be used, its just to stop visual studio's tantrum
            return View();
            
        }
        //displays the cart to the side and lets the user enter there account details 
        public IActionResult UserDetails()
        {
            var gg = _storeContext.Genre.AsQueryable();
            var pd = _storeContext.Product.AsQueryable();
            var st = _storeContext.Stocktake.AsQueryable();
            //i dont need most of the things in this class, but aslong as i am careful with it, it saves creating a bespoke class for this page
            var cart = new CartDeets();
            cart.Cart = new List<prodAmalgam>();
            var SesCart = JsonConvert.DeserializeObject<cart>(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart"));
            SesCart.productIDs.Sort();  //THIS IS INTEGERAL. DO NOT TOUCH!
            var prod = new prodAmalgam();
            var prodindx = -1;
            for (var i = 0; i < SesCart.productIDs.Count; i++)
            {
                try
                {
                    var _pd = pd.Where(pd => pd.ID == SesCart.productIDs[i]).ToArray()[0];
                    var _st = st.Where(st => st.ProductId == SesCart.productIDs[i]).ToArray()[0];
                    //so if the product has already been displayed, it will just incriment its quantity. however this relys on the list being sorted.
                    if (SesCart.productIDs[i] == prodindx)
                    {
                        prod.Quantity++;
                        cart.cartTotal += Math.Round(_st.Price);

                    }
                    else
                    {
                        //maintaining quantity controll
                        prodindx = SesCart.productIDs[i];
                        prod = new prodAmalgam();
                        //adding the new item
                        cart.cartTotal += Math.Round(_st.Price, 2);
                        prod.price = _st.Price;
                        prod.Author = _pd.Author;
                        prod.Name = _pd.Name;
                        prod.Quantity++;
                        cart.Cart.Add(prod);
                    }
                }
                catch
                {
                    Console.WriteLine("shit broke lol");
                }
            }
            //parses it to an int
            int ID = int.Parse(HttpContext.Session.GetString("UserID"));
            //Grabs the existing details
            TO To = new TO();
            To = _storeContext.TO.FirstOrDefault(x => x.patronID == ID);
            ViewBag.Account = To;
            return View(cart);
        }
    }
    public class CartDeets
    {
        public double cartTotal { get; set; }
        public List<prodAmalgam> Cart { get; set; }
    }
}
