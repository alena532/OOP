
namespace BankApplication.Models.Entity
{
    public class Client
    {
        public int Id { get; set; }
    
        public string UserId { get; set; } = null!;
        public string? ProfessionName { get; set; }
        public double? PayForProfession { get; set; }
        public int? EnterpriseId { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
      
        public virtual ICollection<Installment> Installments { get; set; }
        public virtual ICollection<Banking> Bankings { get; set; }

        public virtual User User { get; set; } = null!;
        public Client()
        {
            Accounts = new HashSet<Account>();
            Installments = new HashSet<Installment>();
            Bankings=new HashSet<Banking>();
        }
    }
}
