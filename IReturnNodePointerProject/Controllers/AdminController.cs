using Microsoft.AspNetCore.Mvc;
using IReturnNodePointerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Ajax.Utilities;
using System.Text;
using System.Security.Cryptography;

namespace IReturnNodePointerProject.Controllers
{
	public class AdminController : Controller
    {
        private readonly ApplicationDbContext _storeContext;
        private static AdminListViewModel allLists;
        public AdminController(ApplicationDbContext context)
        {
			AdminListViewModel adminListViewModel = new AdminListViewModel();
			allLists = adminListViewModel;
            _storeContext = context;
        }
        [HttpGet]
        public IActionResult AdminAccountsView(string userType)
        {
			if (userType == "Patron")
			{
				ViewBag.View = "Add";
				ViewBag.UserType = userType;
				
			}
			else if (userType == "User")
			{
				ViewBag.View = "Add";
				ViewBag.UserType = userType;
				
			}
			else
			{
				ViewBag.View = "UnResolved";
			}
				var UserTable = _storeContext.User.AsQueryable();
				var PatronTable = _storeContext.Patrons.AsQueryable();
				var CheckoutTable = _storeContext.TO.AsQueryable();
				allLists.Admins = UserTable.Where(x => x.IsAdmin == true).ToList();
				allLists.Employees = UserTable.Where(x => x.IsAdmin != true).ToList();
				allLists.Users = PatronTable.ToList();
				allLists.UserData = CheckoutTable.ToList();
				User patrons = new User();
				ViewBag.Data = patrons;

                return View(allLists);

        }
        [HttpPost]
        public IActionResult AdminAccountsView(User model)
        {
            
            Patrons patrons = new Patrons();
            ViewBag.Data = model;
            if (ModelState.IsValid)
            {
                //Check if that Patron already exists 
                if (!_storeContext.Patrons.Any(x => x.Email == model.email))
                {
                    Random random = new Random();

                    TO tO = new TO();
                    //add to database and generate a salt / hash the password 
                    //Creates a salt of size 16 and randon numbers/letters
                    //generating a string to combine with the password and to have a value ready to go into the database
                    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    string Salt = new string(Enumerable.Range(1, 32).Select(_ => chars[random.Next(chars.Length)]).ToArray());
                    string Combinedpassword = Salt + model.HashPW;
                    //converts the password into bytes 
                    byte[] CombinedPassword = Encoding.UTF8.GetBytes(Combinedpassword, 0, Combinedpassword.Length);
                    //turns it into a hashed value
                    string Password = Encoding.UTF8.GetString(CombinedPassword);
                    SHA256 mySHA256 = SHA256.Create();
                    byte[] HashedPassword = mySHA256.ComputeHash(CombinedPassword);
                    //changing the typing of the HashPW to fit into the db 
                    string stringHashedPassword = Convert.ToHexString(HashedPassword);

                    //database data 
                    patrons.Email = model.email;
                    patrons.Name = model.Name;
                    patrons.Salt = Salt;
                    patrons.HashPW = stringHashedPassword;
                    _storeContext.Patrons.Add(patrons);
                    _storeContext.SaveChanges();
                    //to use the primary key generated in patrons we gotta save it first to stop foreign key issues 
                    tO.patronID = patrons.UserID;
                    tO.Email = model.email;
                    _storeContext.TO.Add(tO);

                    
                    _storeContext.SaveChanges();
                    ViewBag.View = "UnResolved";

                }
                else
                {
                    ModelState.AddModelError("Error", "Registration Failed: user already exists");
                }
            }
  
            var UserTable = _storeContext.User.AsQueryable();
            var PatronTable = _storeContext.Patrons.AsQueryable();
            var CheckoutTable = _storeContext.TO.AsQueryable();
            allLists.Admins = UserTable.Where(x => x.IsAdmin == true).ToList();
            allLists.Employees = UserTable.Where(x => x.IsAdmin != true).ToList();
            allLists.Users = PatronTable.ToList();
            allLists.UserData = CheckoutTable.ToList();
            return View(allLists);
        }

	}
}
public class AdminListViewModel
{
	public  List<User> Admins { get; set; }
	public  List<Patrons> Users { get; set; }
    public  List<TO> UserData { get; set; }
	public  List<User> Employees { get; set; }
}