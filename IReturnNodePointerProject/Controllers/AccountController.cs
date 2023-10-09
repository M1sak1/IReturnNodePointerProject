using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
		public IActionResult Login(string returnUrl = " ")
		{
			HttpContext.Session.SetInt32("UserID", 0); //session state tracking 
			//var model = _storeContext.LoginViewModel;
			return View(new LoginViewModel());
		}

        //https://stackoverflow.com/questions/70644423/c-sharp-and-asp-net-mvc-how-to-check-if-record-exist-in-database-and-return-id
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
			var Admin_EmployeeCheck = _storeContext.User;
			var PatronCheck = _storeContext.Patrons;
			bool result; 
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
                    return RedirectToAction("Home" , "Index");
                }
			}
			else if (PatronCheck.Any(x => x.Email == model.Username))
			{
				var User = PatronCheck.FirstOrDefault(x => x.Email == model.Username);
                //Make a NULL checker since that would be a vaild login and would break it 
                var Password = User.Salt + model.Password;
                byte[] BytePassword = Encoding.UTF8.GetBytes(Password, 0, Password.Length); //idk if they used UTF8 for the existing ones but whatever 
                var HashedPassword = SHA256.HashData(BytePassword).ToString(); //creates a hashed version of the local password inputted
                if (HashedPassword == User.HashPW) //compares the 2 hashed passwords if they are the same the user has used the right login information
                {
					//Patron View
                    result = true;
                    HttpContext.Session.SetInt32("UserID", (int)User.UserID);
                    HttpContext.Session.SetString("AccessLevel", "Patron");
                    return RedirectToAction("Home", "Index");
                }
            }
            return View(); //Login Failed
        }
		[HttpGet]
        public IActionResult Register()
		{
			return View("Register");
		}
		[HttpPost]
		public IActionResult Register(LoginViewModel model) 
		{
			//Check if that user already exists 
			if(_storeContext.User.Any(x => x.UserName == model.Username)){
			}
			else if(_storeContext.Patrons.Any(x => x.Email == model.Username)){
			}
			else
			{

				//add to database and generate a salt / hash the password 
                return RedirectToAction("Home", "Index");
            }
			//user name already exists 
			return View();
        }
	}
}