using Microsoft.AspNetCore.Mvc;

namespace IReturnNodePointerProject.Controllers
{
	public class DBController : Controller
	{

		public DBController() { }
		public IActionResult Index()
		{
			return View();
		}
	}
}
