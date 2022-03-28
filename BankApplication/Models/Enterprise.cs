using BankApplication.Models.Entity;

namespace BankApplication.Models
{
    public class Enterprise
    {
        public int BankingId { get; set; }
        public virtual Banking Banking { get; set; }
        public virtual ICollection<SalaryProject> SalaryProjects { get; set; }
        public virtual ICollection<ClientSalaryProject> ClientSalaryProjects { get; set; }
        public virtual ICollection<EnterpriseSpecialist> EnterpriseSpecialists { get; set; }

        public int Id { get; set; }
        public string Type { get; set; }
        public string LegalName { get; set; }    
        public int PayerAccountNumber { get; set; }
        public int BankIdentificationNumber { get; set; }
        public string LegalAddress { get; set; }

        public Enterprise()
        {
            SalaryProjects = new HashSet<SalaryProject>();
            EnterpriseSpecialists = new HashSet<EnterpriseSpecialist>();
            ClientSalaryProjects=new HashSet<ClientSalaryProject>();
           // SalaryProjectApplications=new HashSet<SalaryProjectApplication>();
        }
    }
}
