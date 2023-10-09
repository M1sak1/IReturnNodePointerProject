using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IReturnNodePointerProject.Models
{
    [NotMapped] //mvc believes as this class is using the IdentityUser stuff it has to be def.. might need a discriminated column
    public class User : IdentityUser
    {
        [Key]
        public int? UserID { get; set; }
        public string? userName { get; set; }
        public string? email { get; set; }
        public string? Name { get; set; }
        public bool? IsAdmin { get; set; }
        public string? Salt { get; set; }
        public string? HashPW { get; set; }
    }
}
