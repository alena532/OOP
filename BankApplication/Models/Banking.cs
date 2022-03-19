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
        public virtual ICollection<Administrator> Administrators { get; set; }
        public virtual ICollection<EnterpriseSpecialist> EnterpriseSpecialists { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        
        public Banking()
        {
            Clients = new HashSet<Client>();
            Operators = new HashSet<Operator>();
            Administrators = new HashSet<Administrator>();
            EnterpriseSpecialists = new HashSet<EnterpriseSpecialist>();
            Managers = new HashSet<Manager>();
        }




    }
}
