using Microsoft.AspNetCore.Mvc;

namespace IReturnNodePointerProject.Controllers
{
	public class DBController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
