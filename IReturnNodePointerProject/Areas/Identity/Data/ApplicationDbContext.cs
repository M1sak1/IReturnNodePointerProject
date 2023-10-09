using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IReturnNodePointerProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
	//the place to put settings
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

	public DbSet<Product> Product { get; set; }
	public DbSet<Genre> Genre { get; set; }
	public DbSet<Genre_Book> GenreBook { get; set; }
	public DbSet<Genre_Book_New> GenreBook_New { get; set; }
	public DbSet<Genre_Game> GenreGame { get; set; }
	public DbSet<Genre_Movie> GenreMovie { get; set; }
	public DbSet<Orders> Orders { get; set; }
	public DbSet<User> User { get; set; }
	public DbSet<Stocktake> Stocktake { get; set; }
	public DbSet<TO> TO { get; set; }
	public DbSet<Source> Source { get; set; }
	public DbSet<ProductsInOrders> ProductsInOrders { get; set; }
	public DbSet<Patrons> Patrons { get; set; }
	//public DbSet<LoginViewModel> LoginViewModel { get; set; }	
}