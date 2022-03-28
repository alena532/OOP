namespace BankApplication.Models
{
    public class EnterpriseAccount
    {
        public int Id { get; set; }
        
        public int BankingId { get; set; }
        public virtual Banking Banking { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
        
        
        
    }
}
