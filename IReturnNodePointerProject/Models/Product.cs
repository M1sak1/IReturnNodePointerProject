using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Product
	{
		//[Key]
		public int ID { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public int Genre { get; set; }
		public int subGenre { get; set; }
		public DateTime Published { get; set; }
		public String LastUpdatedBy { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}
