using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

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
                
                return View(new AccountViewModel()); //gives the user data from the Patrons and TO databases and creates a new LVM to store it 
			}
			else
			{
				//add something about not being logged in 
				
				return RedirectToAction("Login");
			}
		}
		[HttpPost]
		public IActionResult AccountView(AccountViewModel NewData)
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
                    SHA256 mySHA256 = SHA256.Create();
                    byte[] HashedPassword = mySHA256.ComputeHash(BytePassword);
                    //changing the typing of the HashPW to fit into the db 
                    string stringHashedPassword = Convert.ToHexString(HashedPassword);
                    if (stringHashedPassword == User.HashPW) //compares the 2 hashed passwords if they are the same the user has used the right login information
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
					 //creates a hashed version of the local password inputted
                    SHA256 mySHA256 = SHA256.Create();
                    byte[] HashedPassword = mySHA256.ComputeHash(BytePassword);
                    //changing the typing of the HashPW to fit into the db 
                    string stringHashedPassword = Convert.ToHexString(HashedPassword);
                    if (stringHashedPassword == User.HashPW) //compares the 2 hashed passwords if they are the same the user has used the right login information
					{
						//Patron View
						HttpContext.Session.SetString("UserID", Convert.ToString(User.UserID));
						HttpContext.Session.SetString("AccessLevel", "Patron");
						return RedirectToAction("Index", "Home");
					}
				}
				ModelState.AddModelError("key", "Login Failed:  Incorrect user details");
                 return View(); ; //Login Failed add something to represent this 
				
			}
            ModelState.AddModelError("key", "Login Failed:  Please enter all details");
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
					Random random = new Random();
					Patrons patrons = new Patrons();
					TO tO = new TO();
					//add to database and generate a salt / hash the password 
					//Creates a salt of size 16 and randon numbers/letters
					//generating a string to combine with the password and to have a value ready to go into the database
					const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
					string Salt = new string(Enumerable.Range(1,32).Select(_ => chars[random.Next(chars.Length)]).ToArray());
                    string Combinedpassword = Salt + model.Password;
					//converts the password into bytes 
					byte[] CombinedPassword = Encoding.UTF8.GetBytes(Combinedpassword, 0, Combinedpassword.Length);
					//turns it into a hashed value
					string Password = Encoding.UTF8.GetString(CombinedPassword);
					SHA256 mySHA256 = SHA256.Create();
                    byte[] HashedPassword = mySHA256.ComputeHash(CombinedPassword);
					//changing the typing of the HashPW to fit into the db 
					string stringHashedPassword = Convert.ToHexString(HashedPassword);
                    //database data 
                    patrons.Email = model.Username;
					patrons.Name = model.PreferedName;
					patrons.Salt = Salt;
					patrons.HashPW = stringHashedPassword;
                    _storeContext.Patrons.Add(patrons);
                    _storeContext.SaveChanges();
					//to use the primary key generated in patrons we gotta save it first to stop foreign key issues 
                    tO.patronID = patrons.UserID;
					tO.Email = model.Username;
					_storeContext.TO.Add(tO);
                    _storeContext.SaveChanges();
					
                    HttpContext.Session.SetString("UserID", Convert.ToString(patrons.UserID));
                    HttpContext.Session.SetString("AccessLevel", "Patron"); //reductive but its whatever
                    return RedirectToAction("Index", "Home");
				}
                else
				{
                    ModelState.AddModelError("Error", "Registration Failed: user already exists");
                    ViewBag.Data = model;
                   
					return View(model);
				}
			}
            //Model Not Valid
            ViewBag.Data = model;
            return View(model);
			
        }
    }
    
}