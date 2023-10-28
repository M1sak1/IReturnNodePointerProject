using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Patrons
	{
		[Key]
		public int UserID { get; set; }
		public string? Email { get; set; }
		public string? Name { get; set; }
		public string Salt { get; set; } = string.Empty;
		public string? HashPW { get; set; } = string.Empty;
	}
}
