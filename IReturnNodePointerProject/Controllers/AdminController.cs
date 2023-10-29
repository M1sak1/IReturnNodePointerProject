using Microsoft.AspNetCore.Mvc;
using IReturnNodePointerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Ajax.Utilities;
using System.Text;
using System.Security.Cryptography;
using System;
using WebGrease.Css.Extensions;

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
        public IActionResult AdminAccountsView(string userType, int item)
        {
            if (item != 0)
            {
                if (userType == "Patron")
                {
                    ViewBag.View = "Edit";
                    ViewBag.UserType = userType;
                    Patrons patron = new Patrons();
                    patron = _storeContext.Patrons.FirstOrDefault(x => x.UserID == item);
                    ViewBag.Patron = patron;
                }
                else if (userType == "Employee")
                {
                    ViewBag.View = "Edit";
                    ViewBag.UserType = userType;
                    User user = new User();
                    user = _storeContext.User.FirstOrDefault(x => x.UserID == item);
                    ViewBag.User = user;
                }
                else if (userType == "Admin")
                {
                    ViewBag.View = "Edit";
                    ViewBag.UserType = userType;
                    User user = new User();
                    user = _storeContext.User.FirstOrDefault(x => x.UserID == item);
                    ViewBag.User = user;
                }
            }
            else
            {
                if (userType == "Patron")
                {
                    ViewBag.View = "Add";
                    ViewBag.UserType = userType;

                }
                else if (userType == "Employee")
                {
                    ViewBag.View = "Add";
                    ViewBag.UserType = userType;

                }
                else if (userType == "Admin")
                {
                    ViewBag.View = "Add";
                    ViewBag.UserType = userType;
                }
                else
                {
                    ViewBag.View = "UnResolved";
                }
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
        //Add Accounts
        [HttpPost]
        public IActionResult AdminAccountsView(User model, string btn)
        {
            //cancel the add function
            ViewBag.Data = model;
            //add to database and generate a salt / hash the password 
            //Creates a salt of size 16 and randon numbers/letters
            //generating a string to combine with the password and to have a value ready to go into the database
            Random random = new Random();
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
            
            if (btn == "Patron")
            {
                //Check if that Patron already exists 
                if (!_storeContext.Patrons.Any(y => y.Email == model.email))
                {
                    //empty models
                    Patrons patrons = new Patrons();
                    TO tO = new TO();
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
            else if (btn == "Employee")
            {
                if (!_storeContext.User.Any(x => x.UserName == model.UserName))
                {
                    User user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.email = model.email;
                    user.Salt = Salt;
                    user.HashPW = stringHashedPassword;
                    user.IsAdmin = false;
                    _storeContext.User.Add(user);
                    _storeContext.SaveChanges();
                    ViewBag.View = "UnResolved";
                }
            }
            else if (btn == "Admin")
            {
                if (!_storeContext.User.Any(x => x.UserName == model.UserName))
                {
                    User user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Salt = Salt;
                    user.HashPW = stringHashedPassword;
                    user.IsAdmin = true;
                    _storeContext.User.Add(user);
                    _storeContext.SaveChanges();
                    ViewBag.View = "UnResolved";
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

        //use data gathered to edit account
        public IActionResult EditAccountView(User model, string btn)
        {
            ViewBag.User = model;
            ViewBag.Patron = model;

            if (btn == "Patron")
            {
                    //empty models

                    Patrons patrons = new Patrons();
                    patrons = _storeContext.Patrons.FirstOrDefault(x => x.UserID == model.UserID);
                    //database data 
                    patrons.Email = model.email;
                    patrons.Name = model.Name;
                    //changing the typing of the HashPW to fit into the db 
                    _storeContext.Patrons.Update(patrons);
                    _storeContext.SaveChanges();
                    ViewBag.View = "UnResolved";
            }
            else if (btn == "Employee")
            {
                //empty models
                User Account = new User();
                Account = _storeContext.User.FirstOrDefault(x => x.UserID == model.UserID);
                //database data 
                Account.email = model.email;
                Account.UserName = model.UserName;
                _storeContext.User.Update(Account);
                _storeContext.SaveChanges();
                ViewBag.View = "UnResolved";
            }
            else if (btn == "Admin")
            {
                //empty models
                User Account = new User();
                Account = _storeContext.User.FirstOrDefault(x => x.UserID == model.UserID);
                //database data 
                Account.email = model.email;
                Account.Name = model.Name;
                _storeContext.User.Update(Account);
                _storeContext.SaveChanges();
                ViewBag.View = "UnResolved";
            }

            return RedirectToAction("AdminAccoutsView");
        }
        public IActionResult Delete(string userType , int ID)
        {
			switch (userType)
            {
                case "Patron":
					var user = _storeContext.Patrons.FirstOrDefault(x => x.UserID == ID);
                    var TO = _storeContext.TO.FirstOrDefault(x => x.patronID == ID);
					_storeContext.TO.Remove(TO);
					_storeContext.SaveChanges();
                    _storeContext.Patrons.Remove(user);
                    break;
                case "Employee":
                    var Employee = _storeContext.User.FirstOrDefault(x => x.UserID == ID);
                    _storeContext.User.Remove((User)Employee);
                    break;
                case "Admin":
                    int count = _storeContext.User.Count(x => x.IsAdmin == true);
                    if (count > 1) {
                        var Admin = _storeContext.User.FirstOrDefault(x => x.UserID == ID);
                        //Takes out the NULLS
                        var products = _storeContext.Product.Where(gg => gg.LastUpdatedBy == Admin.UserName).ToArray();
						int i = 0;
                        while(i < products.Count())
                        { 
							var item = products[i];
                            item.LastUpdatedBy = null;
                            _storeContext.Product.Update(item);
							_storeContext.SaveChanges();
                            i++;
						}
                        _storeContext.User.Remove((User)Admin);
					}
                    else
                    {
                        //Error only one admin left
                    }
                    break;
                
			}
            _storeContext.SaveChanges();
			return RedirectToAction("AdminAccountsView","Admin","UnResolved");
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