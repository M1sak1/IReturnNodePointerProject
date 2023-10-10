using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
    public class LoginViewModel
    {
        [Key]
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(255)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(32)]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please Confirm your password.")]
        [StringLength(32)]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string PreferedName { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        //idk might use this, we'll see 
        public bool RememberMe { get; set; }

    }
}
