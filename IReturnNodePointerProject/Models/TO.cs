using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IReturnNodePointerProject.Models
{
	public class TO
    {
        [Key]
		public int customerID { get; set; }
		[ForeignKey("Patrons")]
		public int? patronID { get; set; } 
		//[Required]
		public string Email	{ get; set; }
		public string? PhoneNumber { get; set; } = null;
		public string? StreetAddress { get; set; } = null;	
		public int? PostCode { get; set; } = null;
		public string? Suburb { get; set; } = null;
		public string? State { get; set; } = null;
		public string? CardNumber { get; set; } = null;
		public string? CardOwner { get; set; } = null;
		public string? Expiry { get; set; } = null;
		public int? CVV { get; set; } = null;
	}
}
