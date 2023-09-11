using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;


namespace IReturnNodePointerProject.Controllers
{
	public class LoginController : Controller
	{
		private readonly OnlineStoreContext _storeContext;
		public LoginController(OnlineStoreContext context)
		{
			_storeContext = context;
		}
		public IActionResult Login()
		{
			return View("Login");
		}
	}
}
