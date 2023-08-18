using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

internal class OnlineStoreContext : DbContext{
	//the place to put settings
	public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) 
		: base(options){
		ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;	// you cannot trace me muwahahahahaahahah
	}
	public DbSet<IReturnNodePointerProject.Models.DatabaseControllers.Product> Products { get; set; }

}