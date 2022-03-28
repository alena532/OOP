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

namespace BankApp.Controllers
{
    [Authorize(Roles = "Manager")]
    
    public class ManagerController : Controller
    {
        UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;
        



        public ManagerController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
           
        }

        public IActionResult WatchEnterpriseTransactionRequests()
        {
            MoneyTransactionFromEnterprise transact = new();
            int idx = -1;
            foreach (var request in _context.MoneyTransactionRequest.ToList())
            {
                ++idx;
                EnterpriseSpecialist specialist = _context.EnterpriseSpecialist.Find(request.SenderEnterpriseSpecialistId);
                var specialistWithEnterprise = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.Enterprise).Single();

                transact.Enterprises[idx] = specialistWithEnterprise.Enterprise.LegalName;

                transact.Sums[idx] = request.Sum;

                Client client = _context.Client.Find(request.ReceiverClientId);
                var clientWithUser = _context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();

                transact.ClientUsers.Add(client, clientWithUser.User);

                Account account = _context.Account.Find(request.AccountId);

                transact.Accounts[idx] = account;

                transact.MoneyTransactionRequestId[idx] = request.Id;
            }
            return View("Views/Operator/WatchEnterpriseTransactionRequests.cshtml", transact);
        }
        public IActionResult ConfirmTransactionFromEnterprise(int RequestId)
        {
            MoneyTransactionRequest transact = _context.MoneyTransactionRequest.Find(RequestId);
            Account acc = _context.Account.Find(transact.AccountId);
            acc.Sum += transact.Sum;
            _context.MoneyTransactionRequest.Remove(transact);
            _context.SaveChanges();
            return View("Views/Operator/ConfirmTransactionFromEnterprise.cshtml");
        }

        public IActionResult RejectTransactionFromEnterprise(int RequestId)
        {
            MoneyTransactionRequest transact = _context.MoneyTransactionRequest.Find(RequestId);
            _context.MoneyTransactionRequest.Remove(transact);
            _context.SaveChanges();
            return View("Index");
        }


        

        public IActionResult ChooseEnterpriseForSP()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Banking banking = _context.Banking.Find(managerWithBank.BankingId);
            var bankingWithEnterprises = _context.Banking.Where(x => x.Id == banking.Id).Include(x => x.Enterprises).Single();
            return View("Views/Operator/ChooseEnterpriseForSP.cshtml", bankingWithEnterprises.Enterprises.ToList());
        }
        public IActionResult WatchClientApplication(int EnterpriseId)
        {
            Enterprise enterprise = _context.Enterprise.Find(EnterpriseId);
            var enterpriseWithBank = _context.Enterprise.Where(x => x.Id == enterprise.Id).Include(x => x.Banking).Single();
            var enterpriseWithClentApplicatiob = _context.Enterprise.Where(x => x.Id == enterprise.Id).Include(x => x.ClientSalaryProjects).Single();
            Banking bnk = _context.Banking.Find(enterpriseWithBank.BankingId);
            var bnkWithSalaryApplication = _context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.SalaryProjectApplications).Single();

            ClientApplicationForSimular checking = new();

            foreach (var salaryapplication in bnkWithSalaryApplication.SalaryProjectApplications.ToList())
            {
                foreach (var clientSalarys in enterpriseWithClentApplicatiob.ClientSalaryProjects.ToList())
                {
                    if (salaryapplication.ClientId == clientSalarys.ClientId && salaryapplication.SalaryProjectId == clientSalarys.SalaryProjectId && clientSalarys.EnterpriseId == enterprise.Id)
                    {
                        salaryapplication.Simular = "Has documents";
                        Client client = _context.Client.Find(salaryapplication.ClientId);
                        var clientWithUser = _context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                        checking.UserSalary.Add(clientWithUser.User, _context.SalaryProject.Find(salaryapplication.SalaryProjectId));
                        checking.Applications.Add(salaryapplication);
                        _context.SaveChanges();
                    }
                }

            }
            return View("Views/Operator/WatchClientApplication.cshtml", checking);
        }

        public IActionResult ConfirmSalaryProject(int SalaryProjectApplicationId)
        {
            SalaryProjectApplication application = _context.SalaryProjectApplication.Find(SalaryProjectApplicationId);
            Banking bank = _context.Banking.Find(application.BankingId);
            SalaryProject salary = _context.SalaryProject.Find(application.SalaryProjectId);
            var salaryWithEnterprise = _context.SalaryProject.Where(x => x.Id == salary.Id).Include(x => x.Enterprise).Single();
            Enterprise ent = salaryWithEnterprise.Enterprise;
            var entWithClientSalary = _context.Enterprise.Where(x => x.Id == ent.Id).Include(x => x.ClientSalaryProjects).Single();
            Client client = _context.Client.Find(application.ClientId);
            var clientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            foreach (var acc in clientWithAccounts.Accounts.ToList())
            {
                if (acc.Name == salary.ProfessionName)
                {
                    acc.IsSalaryProject = true;
                    client.ProfessionName = salary.ProfessionName;
                    client.PayForProfession = salary.Salary;
                    client.EnterpriseId = ent.Id;
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

                    return View("Views/Operator/ConfirmSalaryProject.cshtml");
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

            return View("Views/Operator/ConfirmSalaryProject.cshtml");

        }

        public IActionResult EnterprisesStatistics()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(managerWithBank.BankingId);
            var bnkWithEnterpriseSpecialists = _context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.EnterpriseSpecialists).Single();
            Dictionary<EnterpriseSpecialist, User> userSpecialist = new();
            foreach (var specialist in bnkWithEnterpriseSpecialists.EnterpriseSpecialists.ToList())
            {
                var specialistWithUser = _context.EnterpriseSpecialist.Where(x => x.Id == specialist.Id).Include(x => x.User).Single();
                userSpecialist.Add(specialist, specialistWithUser.User);
            }

            return View(userSpecialist);
        }

        public IActionResult PersonalEnterpriseStatistics(int enterpriseId)
        {
            
            StatisticForEnterpriseSpecialist.Information = new();
            foreach(var el in _context.EnterpriseSpecialistStatistic.ToList())
            {
                 if (el.EnterpriseSpecialistId == enterpriseId)
                 {
                    StatisticForEnterpriseSpecialist.Information.Add(el);
                 }
            }

            StatisticForEnterpriseSpecialist.Statistic = new();
            if (StatisticForEnterpriseSpecialist.Information.Count >= 2)
            {
                StatisticForEnterpriseSpecialist.Statistic.Push(StatisticForEnterpriseSpecialist.Information[StatisticForEnterpriseSpecialist.Information.Count - 2]);
                StatisticForEnterpriseSpecialist.Statistic.Push(StatisticForEnterpriseSpecialist.Information[StatisticForEnterpriseSpecialist.Information.Count - 1]);
            }

            if (StatisticForEnterpriseSpecialist.Information.Count == 1)
            {
                StatisticForEnterpriseSpecialist.Statistic.Push(StatisticForEnterpriseSpecialist.Information[StatisticForEnterpriseSpecialist.Information.Count - 1]);
            }
            StatisticForEnterpriseSpecialist statistics = new();


            return View(statistics);
        }

        public async Task<IActionResult> RejectPersonalEnterpriseStatistics(int EnterpriseSpecialistStatisticId)
        {
            EnterpriseSpecialistStatistic statistic = await _context.EnterpriseSpecialistStatistic.FindAsync(EnterpriseSpecialistStatisticId);
            EnterpriseSpecialistStatistic statistic1 = StatisticForEnterpriseSpecialist.Statistic.Peek();


            if (statistic.NameOperation == "AddClientToEnterprise")
            {

                ClientSalaryProject salary = await _context.ClientSalaryProject.FindAsync(statistic.ClientSalaryProject);
                _context.ClientSalaryProject.Remove(salary);
                _context.SaveChanges();
            }
            if (statistic.NameOperation == "CreateRequestForEnterpriseTransaction")
            {
                MoneyTransactionRequest transact= await _context.MoneyTransactionRequest.FindAsync(statistic.MoneyTransactionRequest);
                _context.MoneyTransactionRequest.Remove(transact);
                _context.SaveChanges();
            }
            if (statistic.Id != statistic1.Id)
            {
                StatisticForEnterpriseSpecialist.Statistic.Pop();
                if (statistic.NameOperation == "AddClientToEnterprise")
                {

                    ClientSalaryProject salary = await _context.ClientSalaryProject.FindAsync(statistic.ClientSalaryProject);
                    _context.ClientSalaryProject.Remove(salary);
                    _context.SaveChanges();
                }
                if (statistic.NameOperation == "CreateRequestForEnterpriseTransaction")
                {
                    MoneyTransactionRequest transact = await _context.MoneyTransactionRequest.FindAsync(statistic.MoneyTransactionRequest);
                    _context.MoneyTransactionRequest.Remove(transact);
                    _context.SaveChanges();
                }


            }
           
            _context.SaveChanges();

            return View("Index");
            
        }

        public IActionResult Statistics()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(managerWithBank.BankingId);
            var bnkWithClients = _context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
            Dictionary<Client, User> userClient = new();
            foreach(var client in bnkWithClients.Clients.ToList())
            {
                var clientWithUser=_context.Client.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                userClient.Add(client, clientWithUser.User);
            }
            
            return View(userClient);
        }

        public IActionResult PersonalStatistics(int client)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(managerWithBank.BankingId);
            MoneyStatisticForClient.IndividualInformation = new();
            foreach (var el in _context.MoneyStatictic.ToList())
            {
                if(el.ClientI==client && el.BankingI == bnk.Id)
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
            
            if(MoneyStatisticForClient.IndividualInformation.Count == 1)
            {
                MoneyStatisticForClient.IndividualStatistic.Push(MoneyStatisticForClient.IndividualInformation[MoneyStatisticForClient.IndividualInformation.Count - 1]);
            }
            MoneyStatisticForClient statistics = new();
            return View(statistics);
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
            if(statistic.NameOpration == "PutSumToCreditOrInstallment")
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
            if(statistic.Id != statistic1.Id)
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
        //[Authorize(Roles = "Manager","Operator")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreditRegistr()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(managerWithBank.BankingId);

            List<InstallmentCreation> creation = _context.InstallmentCreation.Where(x => x.BankId == bnk.Id).Where(x=>x.MonthProcent!=null).ToList();
            return View(creation);
        }

        public IActionResult InstallmentRegistr()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(managerWithBank.BankingId);

            List<InstallmentCreation> creation = _context.InstallmentCreation.Where(x => x.BankId == bnk.Id).Where(x => x.MonthProcent == null).ToList();
            return View(creation);
        }

        [HttpPost]
        public IActionResult RejectCreditOrInstallment(int Id)
        {
            InstallmentCreation creation = _context.InstallmentCreation.Find(Id);
            
            _context.InstallmentCreation.Remove(creation);
            _context.SaveChanges();
            if (creation.MonthProcent != null)
            {
                List<InstallmentCreation> creations = new();
                foreach (var el in _context.InstallmentCreation.ToList())
                {
                    if (el.BankId == creation.BankId && el.MonthProcent!=null)
                    {
                        creations.Add(el);
                    }
                }
                return View("CreditRegistr", creations);

            }
            if (creation.MonthProcent == null)
            {
                List<InstallmentCreation> creations = new();
                foreach (var el in _context.InstallmentCreation.ToList())
                {
                    if (el.BankId == creation.BankId && el.MonthProcent == null)
                    {
                        creations.Add(el);
                    }
                }
                return View("InstallmentRegistr", creations);

            }
            return View("Index");

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCredit(int Id)
        {
            InstallmentCreation creation = _context.InstallmentCreation.Find(Id);
            
            Credit credit = new Credit() { MonthProcent =(int) creation.MonthProcent, TimeBegin = DateTime.Now, TimeFinish = DateTime.Now.AddMonths(creation.Month), MustSum = (int)(creation.Month * creation.MonthProcent * 0.01 * creation.NeededSum + creation.NeededSum), CurrentSum = 0, LengthMonth = creation.Month };
            _context.Installment.Add(credit);

            Client client = await _context.Client.FindAsync(creation.ClientId);
            client.Installments.Add(credit);

            Account account= await _context.Account.FindAsync(creation.AccountId);
            account.Installments.Add(credit);

            Banking bnk = await _context.Banking.FindAsync(creation.BankId);
            bnk.Installments.Add(credit);

            account.Sum += creation.NeededSum;
            _context.InstallmentCreation.Remove(creation);
            _context.SaveChanges();
            List<InstallmentCreation> creations = new();
            foreach (var el in _context.InstallmentCreation.ToList())
            {
                if (el.BankId == creation.BankId && el.MonthProcent!=null)
                {
                    creations.Add(el);
                }
            }

            return View("InstallmentRegistr", creations);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmInstallment(int Id)
        {
            InstallmentCreation creation = _context.InstallmentCreation.Find(Id);

            Installment installment = new () {  TimeBegin = DateTime.Now, TimeFinish = DateTime.Now.AddMonths(creation.Month), MustSum = creation.NeededSum, CurrentSum = 0, LengthMonth = creation.Month };
            _context.Installment.Add(installment);

            Client client = await _context.Client.FindAsync(creation.ClientId);
            client.Installments.Add(installment);

            Account account = await _context.Account.FindAsync(creation.AccountId);
            account.Installments.Add(installment);

            Banking bnk = await _context.Banking.FindAsync(creation.BankId);
            bnk.Installments.Add(installment);

            account.Sum += creation.NeededSum;
            _context.InstallmentCreation.Remove(creation);
            _context.SaveChanges();
            List<InstallmentCreation> creations = new();
            foreach (var el in _context.InstallmentCreation.ToList())
            {
                if (el.BankId == creation.BankId && el.MonthProcent == null)
                {
                    creations.Add(el);
                }
            }

            return View("InstallmentRegistr", creations);
        }

        public IActionResult ClientRegistr()
        {

            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

             var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
             Banking bnk = _context.Banking.Find(managerWithBank.BankingId);

             List<UserCreation> creation = _context.UserCreation.Where(x => x.bankId == bnk.Id).ToList();
             return View(creation);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int clientId,int bankId)
        {
            Client client = _context.Client.Find(clientId);
            Banking bank = _context.Banking.Find(bankId);
            client.Bankings.Add(bank);
            bank.Clients.Add(client);
            foreach(var reg in _context.UserCreation.ToList())
            {
                if(reg.bankId==bankId && reg.clId == clientId)
                {
                    _context.UserCreation.Remove(reg);
                }
            }
            _context.SaveChanges();
            
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int clientId, int bankId)
        {
            foreach (var reg in _context.UserCreation.ToList())
            {
                if (reg.bankId == bankId && reg.clId == clientId)
                {
                    _context.UserCreation.Remove(reg);
                }
            }
            _context.SaveChanges();

            return View("Index");
            
        }
       
    }
}
