using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IReturnNodePointerProject.Models
{
    
    public class User 
    {
        [Key]
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public string? email { get; set; }
        public string? Name { get; set; }
        public bool? IsAdmin { get; set; }
        public string? Salt { get; set; }
        public string? HashPW { get; set; }
    }
}
