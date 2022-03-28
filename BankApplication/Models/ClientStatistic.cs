namespace BankApplication.Models
{
    public class ClientStatistic
    {
        public int Id { get; set; }
        public string NameMethod { get; set; }
        public int ClientId { get; set; }

        //Account
        public double? Sum { get; set; }
        public int? AccountId { get; set; }
    }
}
