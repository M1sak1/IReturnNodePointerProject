using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Source
	{
		//[Required] [Key]
		public int sourceid {  get; set; }
		public string? Source_name { get; set; }
		//[Key]
		public int? Genre { get; set; }
	}
}
