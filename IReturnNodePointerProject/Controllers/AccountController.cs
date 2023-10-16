using Humanizer;
using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IReturnNodePointerProject.Controllers
{
	public class AccountController : Controller
	{
        private readonly ApplicationDbContext _storeContext;
		
        public AccountController(ApplicationDbContext context)
		{
            _storeContext = context;
		}
		[HttpGet]
		public ActionResult AccountView()
		{
			
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
				//its so stupid but it wont run without it
				var UserData = (Patrons)_storeContext.Patrons.FirstOrDefault(x => x.UserID == Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                var UserShoppingData = (TO)_storeContext.TO.FirstOrDefault(x => x.patronID == Convert.ToInt32(HttpContext.Session.GetString("UserID")));
				
				ViewBag.Patrons = UserData;
				ViewBag.TO = UserShoppingData;
                return View(new LoginViewModel()); //gives the user data from the Patrons and TO databases and creates a new LVM to store it 
			}
			else
			{ 
				//add something about not being logged in 
				return RedirectToAction("Login");
			}
		}
		[HttpPost]
		public IActionResult AccountView(LoginViewModel NewData)
		{
            var UserData = (Patrons)_storeContext.Patrons.FirstOrDefault(x => x.UserID == Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            var UserShoppingData = (TO)_storeContext.TO.FirstOrDefault(x => x.patronID == Convert.ToInt32(HttpContext.Session.GetString("UserID")));

			//if (ModelState.IsValid)
			//{
				//popups bby, user feedback from the machine
				UserData.Email = NewData.Username;
				UserData.Name = NewData.PreferedName;
				UserShoppingData.Email = NewData.Username;
                UserShoppingData.PhoneNumber = NewData.PhoneNumber;
                UserShoppingData.StreetAddress = NewData.StreetAddress;
                UserShoppingData.PostCode = NewData.PostCode;
                UserShoppingData.Suburb = NewData.Suburb;
                UserShoppingData.State = NewData.State;
                UserShoppingData.CardNumber = NewData.CardNumber;
                UserShoppingData.CardOwner = NewData.CardOwner;
                UserShoppingData.Expiry =	NewData.Expiry;
                UserShoppingData.CVV = NewData.CVV;
                _storeContext.Update(UserData);
                _storeContext.Update(UserShoppingData);
				_storeContext.SaveChanges();
				ViewBag.Patrons = UserData;
				ViewBag.TO = UserShoppingData;
                return View();
			//}
			//else
			//{
          //      ViewBag.Patrons = UserData;
           //     ViewBag.TO = UserShoppingData;
           //     return View();

           // }
			//data not inputted or good 
			
		}
		
		[HttpGet]
		public IActionResult Login()
		{
			HttpContext.Session.SetString("UserID", ""); //session state tracking logs the user out 
            HttpContext.Session.SetString("AccessLevel", "Patron"); //resets the access level
            return View(new ViewModel());
		}

        //https://stackoverflow.com/questions/70644423/c-sharp-and-asp-net-mvc-how-to-check-if-record-exist-in-database-and-return-id
        [HttpPost]
        public IActionResult Login(ViewModel model)
        {
			if (ModelState.IsValid)
			{
				var Admin_EmployeeCheck = _storeContext.User;
				var PatronCheck = _storeContext.Patrons;
				if (Admin_EmployeeCheck.Any(x => x.UserName == model.Username))
				{
					//Make a NULL checker since that would be a vaild login and would break it 
					var User = Admin_EmployeeCheck.FirstOrDefault(x => x.UserName == model.Username);
					var Password = User.Salt + model.Password;
					byte[] BytePassword = Encoding.UTF8.GetBytes(Password, 0, Password.Length); //idk if they used UTF8 for the existing ones but whatever 
					var HashedPassword = SHA256.HashData(BytePassword).ToString(); //creates a hashed version of the local password inputted
					if (HashedPassword == User.HashPW) //compares the 2 hashed passwords if they are the same the user has used the right login information
					{
						//Admin Pages
						HttpContext.Session.SetInt32("UserID", (int)User.UserID);
						if ((bool)User.IsAdmin)
						{
							//Return view to the admin side
							HttpContext.Session.SetString("AccessLevel", "Admin");
						}
						//employee View
						HttpContext.Session.SetString("AccessLevel", "Employee");
						return RedirectToAction("Index", "Home");
					}
				}
				else if (PatronCheck.Any(x => x.Email == model.Username))
				{
                    var User = PatronCheck.FirstOrDefault(x => x.Email == model.Username);
					//Make a NULL checker since that would be a vaild login and would break it 
					var Password = User.Salt + model.Password;
					byte[] BytePassword = Encoding.UTF8.GetBytes(Password, 0, Password.Length);
					var HashedPassword = SHA256.HashData(BytePassword).ToString(); //creates a hashed version of the local password inputted
					if (HashedPassword == User.HashPW) //compares the 2 hashed passwords if they are the same the user has used the right login information
					{
						//Patron View
						HttpContext.Session.SetString("UserID", Convert.ToString(User.UserID));
						HttpContext.Session.SetString("AccessLevel", "Patron");
						return RedirectToAction("Index", "Home");
					}
				}
				else {
                    return View(); ; //Login Failed add something to represent this 
				}
			}
				return View();
        }
		[HttpGet]
        public IActionResult Register()
		{
			ViewBag.Data = new LoginViewModel();
			return View(new LoginViewModel());
		}
		[HttpPost]
		//Only Patron accounts can be created this way, to create admin/employee accounts you must be an admin 
		public IActionResult Register(LoginViewModel model) 
		{
			if (ModelState.IsValid)
			{
				//Check if that Patron already exists 
				if (!_storeContext.Patrons.Any(x => x.Email == model.Username))
				{
					Patrons patrons = new Patrons();
					TO tO = new TO();
					//add to database and generate a salt / hash the password 
					//Creates a salt of size 16 and randon numbers/letters
					byte[] salt = RandomNumberGenerator.GetBytes(16);
					//generating a string to combine with the password and to have a value ready to go into the database
					var stringSalt = salt.ToString();
					var Combinedpassword = stringSalt + model.Password;
					//converts the password into bytes 
					byte[] CombinedPassword = Encoding.UTF8.GetBytes(Combinedpassword, 0, Combinedpassword.Length);
					//turns it into a hashed value
					byte[] HashedPassword = SHA256.HashData(CombinedPassword);
					//changing the typing of the HashPW to fit into the db 
					var stringHashedPassword = HashedPassword.ToString();
					//database data 
					patrons.Email = model.Username;
					patrons.Name = model.PreferedName;
					patrons.Salt = stringSalt;
					patrons.HashPW = stringHashedPassword;
                    _storeContext.Patrons.Add(patrons);
                    _storeContext.SaveChanges();
					//to use the primary key generated in patrons we gotta save it first to stop foreign key issues 
                    tO.patronID = patrons.UserID;
					tO.Email = model.Username;
					tO.PhoneNumber = model.PhoneNumber;
					tO.StreetAddress = model.StreetAddress;
					tO.PostCode = model.PostCode;
					tO.Suburb = model.Suburb;
					tO.State = model.State;
					tO.CardNumber = model.CardNumber;
					tO.CardOwner = model.CardOwner;
					tO.Expiry = model.Expiry;
					tO.CVV = model.CVV;
					
					_storeContext.TO.Add(tO);
                    _storeContext.SaveChanges();
                    //Flag a message saying you have correctly created an account
                    HttpContext.Session.SetString("UserID", Convert.ToString(patrons.UserID));
                    HttpContext.Session.SetString("AccessLevel", "Patron"); //reductive but its whatever
                    return RedirectToAction("Index", "Home");
				}
                else
				{
                    //your input is not lost apon validation failure 
                    //would be funny if there was an easier way to do this and im just bad 
                    ViewBag.Data = model;
                    
					//user name already exists please enter a new name 
					return View(model);
				}
			}
            //Model Not Valid
            ViewBag.Data = model;
            return View(model);
			
        }
		public void UpdateData(LoginViewModel model)
		{
            ViewBag.UserName = model.Username;
            ViewBag.PreferedName = model.PreferedName;
            ViewBag.Password = model.Password;
            ViewBag.ConfirmPassword = model.ConfirmPassword;
            ViewBag.PhoneNumber = model.PhoneNumber;
            ViewBag.StreetAddress = model.StreetAddress;
            ViewBag.PostCode = model.PostCode;
            ViewBag.Suburb = model.Suburb;
            ViewBag.State = model.State;
            ViewBag.CardNumber = model.CardNumber;
            ViewBag.CardOwner = model.CardOwner;
            ViewBag.Expiry = model.Expiry;
            ViewBag.CVV = model.CVV;
        }
	}
}