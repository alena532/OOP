using BankApplication.Models.Entity;

namespace BankApplication.Models
{
    public class Credit
    {
        public  int Id { get; set; }
        public int ClientId { get; set; }
        public int AccountId { get; set; }
        public virtual Client Client { get; set; } = null!;
        public  virtual Account Account { get; set; } = null!;
        public double MonthProcent { get; set; }
        public DateTime TimeBegin { get; set; }
        public DateTime? TimeNow { get; set; }
        public DateTime TimeFinish { get; set; }
        public double MustSum { get; set; }
        public double CurrentSum{ get; set; }

        public int LengthMonth { get; set; }


    }
}
