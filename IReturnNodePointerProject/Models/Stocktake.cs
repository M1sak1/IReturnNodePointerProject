using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Stocktake
	{
		[Key]
		public int ItemID { get; set; }
		//[Key]
		public int SourceId { get; set; }
		//[Key]
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public float Price { get; set; }


	}
}
