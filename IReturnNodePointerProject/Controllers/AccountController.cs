using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IReturnNodePointerProject.Controllers
{
    public class AccountController : Controller
    {
		private readonly OnlineStoreContext _storeContext;
		public AccountController(OnlineStoreContext context)
		{
			_storeContext = context;
		}

		public async Task<IActionResult> Account(string searchString)
		{
			var User = _storeContext.User;
			var pList = User.Where(x => x.UserID == HttpContext.Session.GetInt32("TestData")).ToList(); //grabbing one user using the id declared in homecontroller.cs
			
			return View(pList);
		}
	}
}
