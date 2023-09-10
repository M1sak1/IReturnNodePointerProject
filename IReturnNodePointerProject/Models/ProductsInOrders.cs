using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class ProductsInOrders
	{
		[Key]
		public int? OrderID { get; set; }
		//[Key]
		public int? produktId { get; set; }
		public int? Quantity { get; set; }
	}
}
