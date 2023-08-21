using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IReturnNodePointerProject.Models.DatabaseControllers
{
	public class Product : Controller
	{
		public int ID { get; set; }	
		public string Name { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public int Genre { get; set; }
		public int SubGenres { get; set; }
	}
}
