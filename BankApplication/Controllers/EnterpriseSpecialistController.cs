using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankApplication.Models;
using BankApplication.Models.Entity;
using BankApplication.ViewModel;
using BankApplication.Data;
using Microsoft.EntityFrameworkCore;
using BankApplication.ViewModel.Client;
using BankApplication.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BankApplication.Controllers
{
    [Authorize(Roles = "Enterprise specialist")]
    public class EnterpriseSpecialistController : Controller
    {
        UserManager<User> _userManager;
        ApplicationDbContext _context;

        public EnterpriseSpecialistController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChooseClientForTransaction()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.EnterpriseSpecialists).Single();
            EnterpriseSpecialist specialist = userWithEnterprise.EnterpriseSpecialists.First();
            var enterpriseSpeicalistWithEnterprise = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Enterprise).Single();
            var specialistWithBank = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Banking).Single();
            Banking bank = specialistWithBank.Banking;
            var bankWithClients = _context.Banking.Where(x => x.Id == bank.Id).Include(x => x.Clients).Single();
            Dictionary<Client,User> clientsEnterprise = new();
            foreach (var client in bankWithClients.Clients.ToList())
            {
                if (client.EnterpriseId == specialist.EnterpriseId)
                {
                    var clientWithUser= _context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                    clientsEnterprise.Add(client,clientWithUser.User);
                }
            }

            return View(clientsEnterprise);
            
        }

        public IActionResult SumForSelectedClient(int ClientId)
        {
            MoneyTransaction.ClientId = ClientId;

            return View();

        }

        public async Task<IActionResult> CreateRequestForEnterprise(MoneyTransaction model)
        {
            if (ModelState.IsValid)
            {
                Client client = await _context.Client.FindAsync(MoneyTransaction.ClientId);
                var clientWithAccounts= _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                Account account = new();
                foreach(var acc in clientWithAccounts.Accounts.ToList())
                {
                    if (acc.Name == model.AccountName)
                    {
                        account = acc;
                    }
                }
                if (account.Name != model.AccountName)
                {
                    return View("WrongAccountName");
                }

                string userName = HttpContext.User.Identity.Name;
                var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.EnterpriseSpecialists).Single();
                EnterpriseSpecialist specialist = userWithEnterprise.EnterpriseSpecialists.First();
                var enterpriseSpeicalistWithEnterprise = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Enterprise).Single();
                MoneyTransactionRequest transaction = new() { SenderEnterpriseSpecialistId = enterpriseSpeicalistWithEnterprise.EnterpriseId, ReceiverClientId = client.Id, Sum = model.Sum, AccountId=account.Id };
                _context.MoneyTransactionRequest.Add(transaction);
                _context.EnterpriseSpecialistStatistic.Add(new EnterpriseSpecialistStatistic() { NameOperation = "CreateRequestForEnterpriseTransaction", MoneyTransactionRequest = transaction.Id, EnterpriseSpecialistId = specialist.Id });
                _context.SaveChanges();
            }
            else
            {
                return View("Views/EnterpriseSpecialist/SumForSelectedClient.cshtml", model);
            }
           
            return View();
        }

        public IActionResult ChooseClients()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.EnterpriseSpecialists).Single();
            EnterpriseSpecialist specialist = userWithEnterprise.EnterpriseSpecialists.First();
            var enterpriseSpeicalistWithEnterprise = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Enterprise).Single();
            Enterprise ent = _context.Enterprise.Find(enterpriseSpeicalistWithEnterprise.Enterprise.Id);
            var entWithSalaryProjects= _context.Enterprise.Where(x => x.Id == ent.Id).Include(x => x.SalaryProjects).Single();
            var specialistWithBank= _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Banking).Single();
            Banking bank = specialistWithBank.Banking;
            var bankWithClients= _context.Banking.Where(x => x.Id == bank.Id).Include(x => x.Clients).Single();
            Dictionary<List<User>, List<SalaryProject>> userSalary = new();
            List<User> clientsWithoutProfession = new();
            var li=_context.SalaryProject.ToList();
            foreach(var client in bankWithClients.Clients.ToList())
            {
                if (client.ProfessionName == null)
                {
                    var clientWithUser= _context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                    clientsWithoutProfession.Add(clientWithUser.User);
                }
            }
            userSalary.Add(clientsWithoutProfession, entWithSalaryProjects.SalaryProjects.ToList());
            return View(userSalary);
        }

        public async Task<IActionResult> SelectClient(string id, int SalaryId)
        {
            
           User user = await _userManager.FindByIdAsync(id);
            var userWithClientts = _context.Users.Where(x => x.Id == user.Id).Include(x => x.Clients).Single();
            string userName = HttpContext.User.Identity.Name;
            var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.EnterpriseSpecialists).Single();
            EnterpriseSpecialist specialist = userWithEnterprise.EnterpriseSpecialists.First();
            var specialistWithEnterprise = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Enterprise).Single();
            Enterprise pr = specialistWithEnterprise.Enterprise;
            var prWithClients= _context.Enterprise.Where(x => x.Id == pr.Id).Include(x => x.ClientSalaryProjects).Single();

            foreach (var cl in _context.ClientSalaryProject.ToList())
            {
                if(cl.ClientId== userWithClientts.Clients.First().Id)
                {
                    return View("ClientEnterprise");
                }
            }
            ClientSalaryProject salary = new() { ClientId = userWithClientts.Clients.First().Id, Enterprise = specialistWithEnterprise.Enterprise, SalaryProjectId = SalaryId };
            specialistWithEnterprise.Enterprise.ClientSalaryProjects.Add(salary);
            _context.ClientSalaryProject.Add(salary);
            _context.EnterpriseSpecialistStatistic.Add(new EnterpriseSpecialistStatistic() { NameOperation = "AddClientToEnterprise", ClientSalaryProject = salary.Id, EnterpriseSpecialistId = specialist.Id });
            _context.SaveChanges();
            return View();
        }

        public IActionResult WatchEnterpriseWorkers()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.EnterpriseSpecialists).Single();
            EnterpriseSpecialist specialist = userWithEnterprise.EnterpriseSpecialists.First();
            var specialistWithBank = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Banking).Single();
            Banking bank = specialistWithBank.Banking;
            var bankWithClients = _context.Banking.Where(x => x.Id == bank.Id).Include(x => x.Clients).Single();
            List<Client> clientsWithProfession = new();
            foreach (var client in bankWithClients.Clients.ToList())
            {
                if (client.ProfessionName != null)
                {
                    clientsWithProfession.Add(client);
                }
            }
            return View(clientsWithProfession);
        }
    }
}
