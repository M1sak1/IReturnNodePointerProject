using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class OnlineStoreContext : DbContext
{
	//the place to put settings
	public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options)
		: base(options)
	{
	}
	public DbSet<IReturnNodePointerProject.Models.DatabaseControllers.Product> Product { get; set; }

}