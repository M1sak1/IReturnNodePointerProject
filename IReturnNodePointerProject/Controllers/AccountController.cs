using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
		public IActionResult Login()
		{
			HttpContext.Session.SetInt32("UserID",0); //session state tracking 
			var User = _storeContext.User;
			return View("Login", User);
		}

		public IActionResult Register()
		{

			return View("Register");
		}
	}
}