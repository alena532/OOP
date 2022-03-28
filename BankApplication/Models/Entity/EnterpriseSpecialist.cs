namespace BankApplication.Models.Entity
{
    public class EnterpriseSpecialist
    {
        public int Id { get; set; }
        public int BankingId { get; set; }
        public int EnterpriseId { get; set; }
        public string UserId { get; set; } = null!;
        
        // public List<Account> Accounts { get; set; }
        public virtual Banking Banking { get; set; } = null!;
        public virtual Enterprise Enterprise { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        public EnterpriseSpecialist()
        {


        }



    }
}
