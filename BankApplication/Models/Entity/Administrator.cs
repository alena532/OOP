namespace BankApplication.Models.Entity
{
    public class Administrator
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } = null!;
       
        public virtual User User { get; set; } = null!;
        public Administrator()
        {


        }
    }
}