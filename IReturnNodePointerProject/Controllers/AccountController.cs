using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IReturnNodePointerProject.Controllers
{
    public class AccountController : Controller
    {
		private readonly ApplicationDbContext _storeContext;
		private readonly LoginViewModel loginViewModel;
		public AccountController(ApplicationDbContext context)
		{
			_storeContext = context;
			loginViewModel = new LoginViewModel();
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
		public IActionResult Login()
		{
			HttpContext.Session.SetInt32("UserID",0); //session state tracking 
			var User = loginViewModel;
            return View("Login", User);
		}
        public IActionResult LoginToAccount(string Username, string Password)
        {
            if (Username == null || Password == null)
            {
                return new ContentResult() { Content = "<script language='javascript' type='text/javascript'>alert('Enter a name');</script>" };
            }
            return new ContentResult() { Content = "<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>" };
        }
        
        public IActionResult Register()
		{
			return View("Register");
		}
	}
}