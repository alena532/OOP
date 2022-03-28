namespace BankApplication.Models
{
    public class SalaryProjectApplication
    {
        public int Id { get; set; }
        public int BankingId { get; set; }
        public virtual Banking Banking { get; set; }
        public int ClientId { get; set; }
        public int SalaryProjectId { get; set; }
        public string? Simular { get; set; } 
       // public string EnterpriseName { get; set; }
    }
}
