namespace BankApplication.Models
{
    public class UserCreation
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportSeries { get; set; }
        public int PassportNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string Password { get; set; }
        public string BankName { get; set; }
    }
}
