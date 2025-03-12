namespace LoginService.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int RoleID { get; set; } = 3; //rollID sätts till 3 som standard, för att standard är gäst.
        public string EmailAddress { get; set; }

    }
}
