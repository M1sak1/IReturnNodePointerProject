using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Orders
	{
		[Key]
		public int OrderID { get; set; }
		public int customer { get ; set; }
		public string StreetAdress { get; set; }
		public int PostCode{  get; set; }
		public string Suburb { get; set; }
		public string State { get; set; }

	}
}
