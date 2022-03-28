using BankApplication.Models;

namespace BankApplication.ViewModel
{
    public class ClientApplicationForSimular
    {
        public Dictionary<User, SalaryProject> UserSalary { get; set; } = new();
        public List<SalaryProjectApplication> Applications { get; set; } = new();
        
    }
}
