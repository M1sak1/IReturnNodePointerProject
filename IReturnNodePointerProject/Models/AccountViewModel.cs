using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Please enter an Email.")]
        [EmailAddress(ErrorMessage = "Email must be in the format of an email something@gmail.com")]
        //try to see if you can run a validation against already existing emails
        public string? Username { get; set; } = "";

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(32, MinimumLength = 5, ErrorMessage = "Password must be within 5 to 32 characters")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Please Confirm your password.")]
        [StringLength(32, MinimumLength = 5, ErrorMessage = "Password must be within 5 to 32 characters")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")] //https://stackoverflow.com/questions/4938078/using-dataannotations-to-compare-two-model-properties
        public string? ConfirmPassword { get; set; } = "";
        [StringLength(32)]
        [Required] //probs not required but i havent coded against it 
        public string PreferedName { get; set; } = "";
        [StringLength(10, ErrorMessage = "Phone number must be in standard australian format 04 or 02")]
        [RegularExpression(@"(\S\d)+")] //No whitespace or non-decimal characters

        public string? PhoneNumber { get; set; } = "";
        //[RegularExpression(@"(\A\1-9\z\D\W)+")]
        public string? StreetAddress { get; set; } = "";

        [DataType(DataType.PostalCode)]
        //[Range(3,5)]
        public int? PostCode { get; set; } = null;

        //[RegularExpression(@"(\D\W)+")]
        public string? Suburb { get; set; } = "";
        public string? State { get; set; } = "";

        public string? CardNumber { get; set; } = "";
        public string? CardOwner { get; set; } = "";


        [DataType(DataType.Date)]
        [StringLength(5, MinimumLength = 5)]
        public string? Expiry { get; set; } = "";

        //[Range(2,4)]
        public int? CVV { get; set; } = null;

    }
}
