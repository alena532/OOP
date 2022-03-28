namespace BankApplication.Models
{
    public class ClientSalaryProject
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EnterpriseId { get; set; }
        public virtual Enterprise Enterprise { get; set; }
        
        
        public int SalaryProjectId { get; set; }
    }
}
