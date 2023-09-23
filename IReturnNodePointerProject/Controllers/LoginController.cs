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
		private readonly ApplicationDbContext _storeContext;
		public LoginController(ApplicationDbContext context)
		{
			_storeContext = context;
		}
		public IActionResult Login()
		{
			return View("Login");
		}
	}
}
