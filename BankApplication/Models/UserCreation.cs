using BankApplication.Models.Entity;

namespace BankApplication.Models
{
    public class UserCreation
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public int  clId { get; set; }
        public int bankId { get; set; }
        public string Number { get; set; }
        public string PassportSeries { get; set; }
        public int PassportNumber { get; set; }
        public string IdentificationNumber { get; set; }
    }
}
