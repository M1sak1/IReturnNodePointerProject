using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class TO
	{
		[Key]
		public int customerID { get; set; }
		[Key]
		public int? patronID { get; set; }
		[Required]
		public string Email	{ get; set; }
		public int? PhoneNumber { get; set; }
		public string? StreetAddress { get; set; }
		public int? PostCode { get; set; }
		public string? Suburb { get; set; }
		public string? State { get; set; }
		public int?	CardNumber { get; set; }
		public string? CardOwner { get; set; }
		public string? Expiry { get; set; }
		public int? CVV { get; set; }
	}
}
