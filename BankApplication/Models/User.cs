using Microsoft.AspNetCore.Identity;
using BankApplication.Models.Entity;
using BankApplication.Models;

namespace BankApplication.Models
{
    public class User:IdentityUser
    {
        public int? Number { get; set; }
        public string? PassportSeries { get; set; }
        public int? PassportNumber { get; set; }
        public string? IdentificationNumber { get; set; }
       
        public string? Type { get; set; } 
        public string? LegalName { get; set; }
        public int? PAN { get; set; }
        public int? BIC { get; set; }
        public string? LegalAdress { get; set; }


        public virtual ICollection<Client> Clients { get; set; } 
        public virtual ICollection<Operator> Operators { get; set; } 
        public virtual ICollection<EnterpriseSpecialist> EnterpriseSpecialists { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<Administrator> Administrators { get; set; }

        public User()
        {
            Managers = new HashSet<Manager>();
            Clients = new HashSet<Client>();
            Operators=new HashSet<Operator>();
            EnterpriseSpecialists = new HashSet<EnterpriseSpecialist>();
            Administrators = new HashSet<Administrator>();
        }
       
    }
}
