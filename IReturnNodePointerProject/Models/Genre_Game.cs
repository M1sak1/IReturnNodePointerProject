using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Genre_Game
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }
	}
}
