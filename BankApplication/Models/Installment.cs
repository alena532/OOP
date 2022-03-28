using BankApplication.Models.Entity;

namespace BankApplication.Models
{
    public class Installment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int BankingId { get; set; }
        public virtual Banking Banking { get; set; } = null!;
        public int AccountId { get; set; }
        public virtual Client Client { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
        public DateTime TimeBegin { get; set; }
        public DateTime? TimeNow { get; set; }
        public DateTime TimeFinish { get; set; }
        public double MustSum { get; set; }
        public double CurrentSum { get; set; }
        public bool? State { get; set; }
        public int LengthMonth { get; set; }







    }
}
