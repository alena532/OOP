namespace BankApplication.Models
{
    public class InstallmentCreation
    {
        public int Id { get; set; }
        //public string Account { get; set; }

        public double NeededSum { get; set; }
        public string ClientName { get; set; }
        public int? MonthProcent { get; set; }
        public int Month { get; set; }

        public int AccountId { get; set; }
        public int ClientId { get; set; }
        public int BankId { get; set; }
    }
}
