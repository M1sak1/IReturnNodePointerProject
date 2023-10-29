using IReturnNodePointerProject.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.SqlServer.Management.Smo.Agent;
using Newtonsoft.Json;

namespace IReturnNodePointerProject.Controllers
{
    public class CheckoutController : Controller
    {

        private readonly ApplicationDbContext _storeContext;

        public CheckoutController(ApplicationDbContext storeContext)
        {
            _storeContext = storeContext;
        }

        public IActionResult Checkout()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                return RedirectToAction("Login", "Account");
            }

            //parses it to an int
            int ID = int.Parse(HttpContext.Session.GetString("UserID"));
            //Grabs the existing details
            TO To = new TO();
            To = _storeContext.TO.FirstOrDefault(x => x.patronID == ID);
            ViewBag.Accounts = To;
            if (string.IsNullOrWhiteSpace(To.Email) || string.IsNullOrWhiteSpace(To.PhoneNumber) || string.IsNullOrWhiteSpace(To.StreetAddress) || To.PostCode == null || string.IsNullOrWhiteSpace(To.Suburb) || string.IsNullOrWhiteSpace(To.State) || string.IsNullOrWhiteSpace(To.CardNumber) || string.IsNullOrWhiteSpace(To.CardOwner) || string.IsNullOrWhiteSpace(To.Expiry) || To.CVV == null)
            {
                return RedirectToAction("UserDetails", "Cart");
            }
            else
            {
                //You have bought the products
                ////adding Order
                Orders order = new Orders();
                order.Suburb = To.Suburb;
                order.StreetAddress = To.StreetAddress;
                order.State = To.State;
                order.customer = To.customerID;
                order.PostCode = (int)To.PostCode;
                _storeContext.Orders.Add(order);
                _storeContext.SaveChanges();
                //Going through the products ordered

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
                                prod.ProductID = prodindx;
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
                            Console.WriteLine("It Dont Work");
                        }
                    }
                    
                    foreach (var item in cart.Cart)
                    {
                        Stocktake stocktake = (Stocktake)_storeContext.Stocktake.FirstOrDefault(x => x.ProductId == item.ProductID);
                        ProductsInOrders productsInOrders = new ProductsInOrders();
                        productsInOrders.Quantity = item.Quantity;
                        productsInOrders.OrderID = order.OrderID;
                        productsInOrders.produktId = item.ProductID;
                        //reduces the Quantity by the number of items bought (does break for streaming services but its 11:30 on the Sun sooooo....)
                        stocktake.Quantity = stocktake.Quantity - item.Quantity ;
                        _storeContext.ProductsInOrders.Add(productsInOrders);
                        _storeContext.Stocktake.Update(stocktake);
                    }
                    _storeContext.SaveChanges();
                    var JsOBJ = "";
                    var Cart = JsonConvert.DeserializeObject<cart>(HttpContext.Session.GetString("BlockBuster_2_Electric_Boogaloo_Cart"));
                    Cart = new cart();
                    JsOBJ = JsonConvert.SerializeObject(Cart);
                    HttpContext.Session.SetString("BlockBuster_2_Electric_Boogaloo_Cart","");
                    return View();
                }

            }
        }
    }
}
