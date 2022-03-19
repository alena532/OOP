

namespace BankApplication.Models.Entity
{
    public class Client
    {
        public int Id { get; set; }
        public int BankingId { get; set; }
        public string UserId { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Credit> Credits { get; set; }
        public virtual ICollection<Installment> Installments { get; set; }
        
        public virtual Banking Bank { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public Client()
        {
            Accounts = new HashSet<Account>();
            Credits=new HashSet<Credit>();
            Installments = new HashSet<Installment>();
           
        }
    }
}
