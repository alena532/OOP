using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BankApplication.Models.Entity;
namespace BankApplication.Models
{
    public class Banking
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Operator> Operators { get; set; }
        
        public virtual ICollection<EnterpriseSpecialist> EnterpriseSpecialists { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Banking> Bankings { get; set; }
        public virtual ICollection<Installment> Installments { get; set; }
        public virtual ICollection<Enterprise> Enterprises { get; set; }
        public virtual ICollection<SalaryProjectApplication> SalaryProjectApplications { get; set; }
        public Banking()
        {
            Clients = new HashSet<Client>();
            Operators = new HashSet<Operator>();
            
            EnterpriseSpecialists = new HashSet<EnterpriseSpecialist>();
            Managers = new HashSet<Manager>();
            Accounts = new HashSet<Account>();
            Bankings=new HashSet<Banking>();
            Installments = new HashSet<Installment>();
            Enterprises=new HashSet<Enterprise>();
            SalaryProjectApplications=new HashSet<SalaryProjectApplication>();
        }




    }
}
