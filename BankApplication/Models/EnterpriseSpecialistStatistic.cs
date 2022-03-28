namespace BankApplication.Models
{
    public class EnterpriseSpecialistStatistic
    {
        public int Id { get; set; }
        public string NameOperation { get; set; }
        public int? MoneyTransactionRequest { get; set; }
        public int? ClientSalaryProject { get; set; }
        public int EnterpriseSpecialistId { get; set; }
    }
}
