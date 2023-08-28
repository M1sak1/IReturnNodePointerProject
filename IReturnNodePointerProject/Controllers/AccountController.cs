using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			var pList = User.ToList();
			return View(pList);
		}
    }
}
