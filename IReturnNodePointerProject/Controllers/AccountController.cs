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
			var pList = User.ToList();
			return View(pList);
		}
		// putting this here for funzies, but this is *broadly* how you return somthing from the database based on a seach string or ID, (in this example ID)
		/*
			public async Task<IActionResult> Details(int? id) { 
			if (id == null) {
				 return NotFound();
			}
			var prod = await _storeContext.Product.FirstOrDefaultAsync(m => m.ID == id);
			if (prod == null) {
				return NotFound();
			}
			return View(prod);
		}	 
		 */
	}
}
