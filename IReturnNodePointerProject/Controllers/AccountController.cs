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
			// grabbed how to parse non NUll from here https://stackoverflow.com/questions/3069748/how-to-remove-all-the-null-elements-inside-a-generic-list-in-one-go
			var pList = User.ToList();
			return View(pList);
		}
	}
}
