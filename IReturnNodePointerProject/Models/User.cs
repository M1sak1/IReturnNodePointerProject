namespace IReturnNodePointerProject.Models
{
    public class User
    { 
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public bool? IsAdmin { get; set; }
        public string? Salt { get; set; }
        public string? HashPW { get; set; }
    }
}
