using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Patrons
	{
		[Key]
		public int UserID { get; set; }
		public string? Email { get; set; }
		public string? Name { get; set; }
		public string? Salt { get; set; }
		public string? HashPW { get; set; }
	}
}
