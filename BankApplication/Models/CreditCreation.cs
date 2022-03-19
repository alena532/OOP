namespace BankApplication.Models
{
    public class CreditCreation
    {
        public int Id { get; set; }
        //public string Account { get; set; }

        public double NeededSum { get; set; }

        public int MonthProcent { get; set; }
        public int Month { get; set; }

        public int AccountId { get; set; }
        public int ClientId { get; set; }
        public string BankName { get; set; }
    }
}
