
using BankApplication.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;


namespace BankApplication.Models
{
    
    public class Account
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public bool? IsSalaryProject { get; set; }
        public bool? IsSalaryDay { get; set; }
        public virtual Client Client { get; set; } = null!;
        public int BankingId { get; set; }
        public virtual Banking Banking { get; set; }
        public virtual ICollection<Installment> Installments { get; set; }
         
        public string State { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
       // public virtual ICollection<Credit> Credits { get; set; }
       

        public Account()
        {
         //   Credits = new HashSet<Credit>();
            Installments = new HashSet<Installment>();
        }

        



    }
}
