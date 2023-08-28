using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IReturnNodePointerProject.Models;
using Microsoft.EntityFrameworkCore;

public class OnlineStoreContext : DbContext
{
	//the place to put settings
	public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options)
		: base(options)
	{
	}
	public DbSet<Product> Product { get; set; }
	public DbSet<Genre> Genre { get; set; }
	public DbSet<Genre_Book> GenreBook { get; set; }
	public DbSet<Genre_Book_New> GenreBook_New { get; set; }
	public DbSet<Genre_Game> GenreGame { get; set; }
	public DbSet<Genre_Movie> GenreMovie { get; set; }
	public DbSet<Orders> Orders { get; set; }
	public DbSet<User> User { get; set; }
}