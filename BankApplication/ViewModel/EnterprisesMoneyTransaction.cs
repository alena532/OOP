namespace BankApplication.ViewModel

{
    public class MoneyTransaction
    {
        public int EntepriceId {get;set;}
        public double Sum { get; set; }
        public string AccountName { get; set; }
        public static int ClientId { get; set; }
    }
}
