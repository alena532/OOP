
using Microsoft.AspNetCore.Identity;

namespace BankApplication.Models.Entity
{
    public class Manager
    {
        public int Id { get; set; }
        public int BankingId { get; set; }
        public string UserId { get; set; } = null!;
       
        public virtual Banking Bank { get; set; } = null!;
        public virtual User User { get; set; } = null!;
      
        public Manager()
        {
            
        }

    }
}
