using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Genre
	{
		[Key]
		public int genreID { get; set; }
		public string Name { get; set; }
	}
}
