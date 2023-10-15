using System.ComponentModel.DataAnnotations;
namespace IReturnNodePointerProject.Models
{
    public class ViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
  
    }
}
