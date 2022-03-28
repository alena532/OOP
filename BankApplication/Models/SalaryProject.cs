namespace BankApplication.Models
{
    public class SalaryProject
    {
        public int EnterpriseId { get; set; }
        public virtual Enterprise Enterprise { get; set; }

        //  public virtual Banking Banking { get; set; }
        public int Id { get; set; }
        public string ProfessionName { get; set; }
       public double Salary { get; set; }
        
       // public int? EnterpriseAccountId { get; set; }
    }
}
