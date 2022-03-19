namespace BankApplication.Models.Entity
{
    public class Operator
    {
        public int Id { get; set; }
       public int BankingId { get; set; }
        public string UserId { get; set; } = null!;
        // public List<Account> Accounts { get; set; }
       public virtual Banking Bank { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        public Operator()
        {

        }
    }
}
