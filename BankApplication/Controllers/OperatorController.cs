using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BankApplication.Models;
using BankApplication.Models.Entity;
using BankApplication.ViewModel;
using BankApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Controllers
{
    public class OperatorController:Controller
    {
        UserManager<User> _userManager;
        readonly SignInManager<User> _signManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;

        public OperatorController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult WatchEnterpriseTransactionRequests()
        {
            MoneyTransactionFromEnterprise transact = new();
            int idx = -1;
            foreach(var request in _context.MoneyTransactionRequest.ToList())
            {
                ++idx;
                EnterpriseSpecialist specialist = _context.EnterpriseSpecialist.Find(request.SenderEnterpriseSpecialistId);
                var specialistWithEnterprise= _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Enterprise).Single();

                transact.Enterprises[idx] = specialistWithEnterprise.Enterprise.LegalName;

                transact.Sums[idx] = request.Sum;

                Client client= _context.Client.Find(request.ReceiverClientId);
                var clientWithUser = _context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();

                transact.ClientUsers.Add(client, clientWithUser.User);

                Account account = _context.Account.Find(request.AccountId);

                transact.Accounts[idx] = account;

                transact.MoneyTransactionRequestId[idx] = request.Id;
            }
            return View(transact);
        }

        public IActionResult ConfirmTransactionFromEnterprise(int RequestId)
        {
            MoneyTransactionRequest transact= _context.MoneyTransactionRequest.Find(RequestId);
            Account acc = _context.Account.Find(transact.AccountId);
            acc.Sum += transact.Sum;
            _context.MoneyTransactionRequest.Remove(transact);
            _context.SaveChanges();
            return View();
        }

        public IActionResult RejectTransactionFromEnterprise(int RequestId)
        {
            MoneyTransactionRequest transact = _context.MoneyTransactionRequest.Find(RequestId);
            _context.MoneyTransactionRequest.Remove(transact);
            _context.SaveChanges();
            return View("Index");
        }

        public IActionResult ChooseBankApplicationsForSP()
        {
            return View(_context.Banking.ToList());
        }

        public IActionResult ChooseEnterpriseForSP(int BankId)
        {
            Banking banking = _context.Banking.Find(BankId);
            var bankingWithEnterprises= _context.Banking.Where(x => x.Id == banking.Id).Include(x => x.Enterprises).Single();
            return View(bankingWithEnterprises.Enterprises.ToList());
        }
        public IActionResult WatchClientApplication(int EnterpriseId)
        {
            Enterprise enterprise=_context.Enterprise.Find(EnterpriseId);
            var enterpriseWithBank= _context.Enterprise.Where(x => x.Id == enterprise.Id).Include(x => x.Banking).Single();
            var enterpriseWithClentApplicatiob = _context.Enterprise.Where(x => x.Id == enterprise.Id).Include(x => x.ClientSalaryProjects).Single();
            Banking bnk = _context.Banking.Find(enterpriseWithBank.BankingId);
            var bnkWithSalaryApplication=_context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.SalaryProjectApplications).Single();

            ClientApplicationForSimular checking = new();
            
            foreach(var salaryapplication in bnkWithSalaryApplication.SalaryProjectApplications.ToList())
            {
                foreach(var clientSalarys in enterpriseWithClentApplicatiob.ClientSalaryProjects.ToList())
                {
                    if(salaryapplication.ClientId== clientSalarys.ClientId && salaryapplication.SalaryProjectId== clientSalarys.SalaryProjectId && clientSalarys.EnterpriseId==enterprise.Id)
                    {
                        salaryapplication.Simular = "Has documents";
                        Client client = _context.Client.Find(salaryapplication.ClientId);
                        var clientWithUser= _context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                         checking.UserSalary.Add(clientWithUser.User, _context.SalaryProject.Find(salaryapplication.SalaryProjectId));
                        checking.Applications.Add(salaryapplication);
                        _context.SaveChanges();
                    }
                }
                
            }
            return View(checking); 
        }

        public IActionResult ConfirmSalaryProject(int SalaryProjectApplicationId)
        {
            SalaryProjectApplication application = _context.SalaryProjectApplication.Find(SalaryProjectApplicationId);
            Banking bank = _context.Banking.Find(application.BankingId);
            SalaryProject salary= _context.SalaryProject.Find(application.SalaryProjectId);
            var salaryWithEnterprise= _context.SalaryProject.Where(x => x.Id == salary.Id).Include(x => x.Enterprise).Single();
            Enterprise ent = salaryWithEnterprise.Enterprise;
            var entWithClientSalary= _context.Enterprise.Where(x => x.Id == ent.Id).Include(x => x.ClientSalaryProjects).Single();
            Client client = _context.Client.Find( application.ClientId);
            var clientWithAccounts= _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            foreach (var acc in clientWithAccounts.Accounts.ToList())
            {
                if (acc.Name == salary.ProfessionName)
                {
                    acc.IsSalaryProject = true;
                    client.ProfessionName = salary.ProfessionName;
                    client.PayForProfession = salary.Salary;
                    client.EnterpriseId = ent.Id;
                    bank.SalaryProjectApplications.Remove(application);
                    foreach(var sal in entWithClientSalary.ClientSalaryProjects.ToList())
                    {
                        if(sal.ClientId== application.ClientId)
                        {
                            ent.ClientSalaryProjects.Remove(sal);
                            _context.ClientSalaryProject.Remove(sal);
                        }
                        
                    }
                    _context.SalaryProjectApplication.Remove(application);
                    

                    _context.SaveChanges();

                    return View();
                }
            }
            Account accforpayment = new() { Client = client, IsSalaryProject = true, Banking = bank, Name = salary.ProfessionName, State = "Usual", Sum = 0 };
            client.ProfessionName = salary.ProfessionName;
            client.PayForProfession = salary.Salary;
            client.EnterpriseId = ent.Id;
            bank.Accounts.Add(accforpayment);
            bank.SalaryProjectApplications.Remove(application);
            foreach (var sal in entWithClientSalary.ClientSalaryProjects.ToList())
            {
                if (sal.ClientId == application.ClientId)
                {
                    ent.ClientSalaryProjects.Remove(sal);
                    _context.ClientSalaryProject.Remove(sal);
                }

            }
            _context.SalaryProjectApplication.Remove(application);
            _context.SaveChanges();
            
            return View();

        }
        public IActionResult Statistics()
        {
            string userName = HttpContext.User.Identity.Name;

            var userWithOperator = _context.Users.Where(x => x.UserName == userName).Include(x => x.Operators).Single();
            Operator operat = userWithOperator.Operators.First();

            var operatorWithBank = _context.Manager.Where(x => x.Id == operat.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(operatorWithBank.BankingId);
            var bnkWithClients = _context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
            Dictionary<Client, User> userClient = new();
            foreach (var client in bnkWithClients.Clients.ToList())
            {
                var clientWithUser = _context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                userClient.Add(client, clientWithUser.User);
            }

            return View("Views/Manager/Statistics.cshtml",userClient);
        }

        public IActionResult PersonalStatistics(int client)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Operators).Single();
            Operator operat = userWithManager.Operators.First();

            var managerWithBank = _context.Operator.Where(x => x.Id == operat.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(managerWithBank.BankingId);
            MoneyStatisticForClient.IndividualInformation = new();
            foreach (var el in _context.MoneyStatictic.ToList())
            {
                if (el.ClientI == client && el.BankingI == bnk.Id)
                {
                    MoneyStatisticForClient.IndividualInformation.Add(el);
                    if (el.Status == false)
                    {
                        MoneyStatisticForClient.Status = false;
                    }
                }

            }
            MoneyStatisticForClient.IndividualStatistic = new();
            if (MoneyStatisticForClient.IndividualInformation.Count >= 2)
            {
                MoneyStatisticForClient.IndividualStatistic.Push(MoneyStatisticForClient.IndividualInformation[MoneyStatisticForClient.IndividualInformation.Count - 2]);
                MoneyStatisticForClient.IndividualStatistic.Push(MoneyStatisticForClient.IndividualInformation[MoneyStatisticForClient.IndividualInformation.Count - 1]);
            }

            if (MoneyStatisticForClient.IndividualInformation.Count == 1)
            {
                MoneyStatisticForClient.IndividualStatistic.Push(MoneyStatisticForClient.IndividualInformation[MoneyStatisticForClient.IndividualInformation.Count - 1]);
            }
            MoneyStatisticForClient statistics = new();
            return View("Views/Manager/PersonalStatistics.cshtml", statistics);
        }

        public async Task<IActionResult> RejectPersonalStatistics(int MoneyStaticId)
        {
            MoneyStatictic statistic = await _context.MoneyStatictic.FindAsync(MoneyStaticId);
            MoneyStatictic statistic1 = MoneyStatisticForClient.IndividualStatistic.Peek();


            if (statistic.NameOpration == "AddSumToAccount")
            {
                Account account = await _context.Account.FindAsync(statistic.AccountI);
                account.Sum -= statistic.Sum;
                _context.SaveChanges();
            }
            if (statistic.NameOpration == "PutSumToCreditOrInstallment")
            {
                Installment installment = await _context.Installment.FindAsync(statistic.InstallmentI);
                if (installment.State == false)
                {
                    installment.State = true;
                }
                Installment inst = await _context.Installment.FindAsync(statistic.InstallmentI);
                var instWithAccount = _context.Installment.Where(x => x.Id == inst.Id).Include(x => x.Account).Single();
                if (instWithAccount.Account.State == "Freeze")
                {
                    instWithAccount.Account.State = "Usual";
                }
                instWithAccount.Account.Sum += statistic.Sum;
                inst.CurrentSum -= statistic.Sum;
            }
            if (statistic.Id != statistic1.Id)
            {
                MoneyStatisticForClient.IndividualStatistic.Pop();
                if (statistic.NameOpration == "AddSumToAccount")
                {
                    Account account = await _context.Account.FindAsync(statistic.AccountI);
                    account.Sum -= statistic.Sum;
                    _context.SaveChanges();
                }
                if (statistic.NameOpration == "PutSumToCreditOrInstallment")
                {
                    Installment installment = await _context.Installment.FindAsync(statistic.InstallmentI);
                    if (installment.State == false)
                    {
                        installment.State = true;
                    }
                    Installment inst = await _context.Installment.FindAsync(statistic.InstallmentI);
                    var instWithAccount = _context.Installment.Where(x => x.Id == inst.Id).Include(x => x.Account).Single();
                    if (instWithAccount.Account.State == "Freeze")
                    {
                        instWithAccount.Account.State = "Usual";
                    }
                    instWithAccount.Account.Sum += statistic.Sum;
                    inst.CurrentSum -= statistic.Sum;
                }


            }
            statistic.Status = false;
            _context.SaveChanges();

            return View("Index");
        }
    }
}
