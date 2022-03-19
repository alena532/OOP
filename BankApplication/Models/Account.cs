
using BankApplication.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;


namespace BankApplication.Models
{
    
    public class Account
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
       // public int CreditId { get; set; }
       // public int InstallmentId { get; set; }
        public string Name { get; set; }
        public virtual Client Client { get; set; } = null!;
        public double Sum { get; set; }
        public virtual ICollection<Credit> Credits { get; set; }
        public virtual ICollection<Installment> Installments { get; set; }

        public Account()
        {
            Credits = new HashSet<Credit>();
            Installments = new HashSet<Installment>();
        }

        



    }
}
