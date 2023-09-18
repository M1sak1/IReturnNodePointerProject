using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Stocktake
	{
		[Key]
		public int ItemId { get; set; }
		//[Key]
		public int SourceId { get; set; }
		//[Key]
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }


	}
}
