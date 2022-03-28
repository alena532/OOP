namespace BankApplication.Models
{
    public class MoneyStatictic
    {
        public int Id { get; set; }
        public string NameOpration { get; set; }
        public bool Status { get; set; }
        public int Sum { get; set; }
        public int ClientI { get; set; }
        public int BankingI { get; set; }
        public int? AccountI { get; set; }
        public int? InstallmentI { get; set; }
    }
}
