using System.ComponentModel.DataAnnotations;
using static IReturnNodePointerProject.Models.Patrons;
namespace IReturnNodePointerProject.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter an Email.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [EmailAddress(ErrorMessage ="Email must be in the format of an email something@gmail.com")]
        //try to see if you can run a validation against already existing emails

        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a password")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(32, MinimumLength = 5, ErrorMessage = "Password must be within 5 to 32 characters")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please Confirm your password.")]
        [StringLength(32, MinimumLength = 5, ErrorMessage ="Password must be within 5 to 32 characters")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")] //https://stackoverflow.com/questions/4938078/using-dataannotations-to-compare-two-model-properties
        public string ConfirmPassword { get; set; } = string.Empty;
        [StringLength(32)]
        [Required] //probs not required but i havent coded against it 
        public string PreferedName { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        //idk might use this, we'll see 
        public bool RememberMe { get; set; }

    }
}
