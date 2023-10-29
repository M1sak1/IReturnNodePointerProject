using System.ComponentModel.DataAnnotations;
namespace IReturnNodePointerProject.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter an Email.")]
        [EmailAddress(ErrorMessage ="Email must be in the format of an email something@gmail.com")]
        //try to see if you can run a validation against already existing emails
        public string? Username { get; set; } = null;

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(32, MinimumLength = 5, ErrorMessage = "Password must be within 5 to 32 characters")]
        public string? Password { get; set; } = null;
        
        [Required(ErrorMessage = "Please Confirm your password.")]
        [StringLength(32, MinimumLength = 5, ErrorMessage ="Password must be within 5 to    32 characters")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")] //https://stackoverflow.com/questions/4938078/using-dataannotations-to-compare-two-model-properties
        public string? ConfirmPassword { get; set; } = null;
        [StringLength(32)]
        [Required]  
        public string? PreferedName { get; set; } = null;
 
        public bool RememberMe { get; set; }

    }
}
