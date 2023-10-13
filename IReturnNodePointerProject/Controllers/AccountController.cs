using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
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
		
		public async Task<IActionResult> AccountView(int UserID)
		{
			var User = _storeContext.User;
			if (HttpContext.Session.GetInt32("UserID") != 0 || HttpContext.Session.GetInt32("UserID") != -1)
			{
				var pList = User.Where(x => x.UserID == HttpContext.Session.GetInt32("UserID")).ToList(); //grabbing one user using the id declared in homecontroller.cs
				return View("AccountView", pList);
			}
			else
			{ 
				return View("Login", User);
			}
		}
		
		[HttpGet]
		public IActionResult Login()
		{
			HttpContext.Session.SetInt32("UserID", 0); //session state tracking 
			return View(new LoginViewModel());
		}

        //https://stackoverflow.com/questions/70644423/c-sharp-and-asp-net-mvc-how-to-check-if-record-exist-in-database-and-return-id
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
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
				if(HashedPassword == User.HashPW) //compares the 2 hashed passwords if they are the same the user has used the right login information
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
                    HttpContext.Session.SetInt32("UserID", (int)User.UserID);
                    HttpContext.Session.SetString("AccessLevel", "Patron");
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home"); //Login Failed add something to represent this 
        }
		[HttpGet]
        public IActionResult Register()
		{
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
					if (model.Password == model.ConfirmPassword)
					{
						Patrons patrons = new Patrons();
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
						patrons.Email = model.Username.ToString();
						patrons.Name = model.PreferedName.ToString();
						patrons.Salt = stringSalt.ToString();
						patrons.HashPW = stringHashedPassword.ToString();
						_storeContext.Patrons.Add(patrons);
						_storeContext.SaveChanges();
						return RedirectToAction("Index", "Home");
					}
					else
					{
						//Passwords Do not match
						return View();
					}
				}
				else
				{
					//user name already exists please enter a new name 
					return View();
				}
			}
			//Something went wrong 
			return View();
			
        }
	}
}