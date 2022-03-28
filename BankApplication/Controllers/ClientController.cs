using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankApplication.Models;
using BankApplication.Models.Entity;
using BankApplication.ViewModel;
using BankApplication.Data;
using Microsoft.EntityFrameworkCore;
using BankApplication.ViewModel.Client;
using BankApplication.ViewModel.Account;

namespace BankApplication.Controllers
{
    
    public class ClientController : Controller
    {
        UserManager<User> _userManager;
        ApplicationDbContext _context;
       

        public ClientController(UserManager<User> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            //Index();
           
        }
        public async Task<IActionResult> Index()
        {
            return View();//account,creditst:menu
        }

        public async Task<IActionResult> Bank()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks= _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bankings).Single();
            return View(clientWithBanks.Bankings.ToList());
        }

        
        [HttpGet]
        public async Task<IActionResult> AddBank(int Id)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bankings).Single();
            List<Banking> bnkList = new();
           
            foreach(var bnk in _context.Banking.ToList())
            {
                if (!clientWithBanks.Bankings.Contains(bnk))
                {
                    bnkList.Add(bnk)
;                }
            }
            if (bnkList.Count == 0)
            {
                return RedirectToAction("Index", "Client");
            }
            return View(bnkList);
        }

        [HttpPost]
        public async Task<IActionResult> AddBank(string Id)
        {
           
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
           
            Banking bnk = BankRepository.FindByName(_context,Id);

            UserCreation creation = new() { clId = client.Id, bankId= bnk.Id,ClientName=userName,Email=userWithClient.Email,Number=(string)userWithClient.PhoneNumber,PassportSeries=userWithClient.PassportSeries,PassportNumber=(int)userWithClient.PassportNumber,IdentificationNumber=(string)userWithClient.IdentificationNumber };
            foreach(var us in _context.UserCreation.ToList())
            {
                if(us.clId == creation.clId && us.bankId == creation.bankId)
                {
                    return View("Application");
                }
            }
            await _context.UserCreation.AddAsync(creation);
            _context.SaveChanges();

            return View("Application");
        }

        public async Task<IActionResult> ChooseBankSalaryProject()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bankings).Single();

            if (clientWithBanks.Bankings.ToList().Count == 0)
            {
                return View("EmptyBanks");
            }
            return View(clientWithBanks.Bankings.ToList());

        }

        public async Task<IActionResult> SelectEnterpriseForSalaryProject(int BankId)
        {   
            Banking bnk = await _context.Banking.FindAsync(BankId);
            var bnkWithEnterprises= _context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.Enterprises).Single();
            
            
            return View(bnkWithEnterprises.Enterprises.ToList());

        }

        public async Task<IActionResult> SelectSalaryProject(int EnterpriseId)
        {
            Enterprise ent = await _context.Enterprise.FindAsync(EnterpriseId);
            var entWithSalary= _context.Enterprise.Where(x => x.Id == ent.Id).Include(x => x.SalaryProjects).Single();
            Dictionary<List<SalaryProject>, int> salaryEnterprise = new();
            salaryEnterprise.Add(entWithSalary.SalaryProjects.ToList(), EnterpriseId);

            return View(salaryEnterprise);

        }

        public async Task<IActionResult> SalaryProjectApplication(int salaryId,int EnterpriseId)
        {
            var salary = _context.SalaryProject.Find(salaryId);
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            
         
            if(client.EnterpriseId!=null)
            {
                Enterprise enterprise1 = _context.Enterprise.Find(client.EnterpriseId);
                 return View("RejectSalaryProject", enterprise1); 
            }
            
            foreach(var el in _context.SalaryProjectApplication.ToList())
            {
                if (el.ClientId==client.Id)
                {
                    return View();
                }
            }
            Enterprise enterprise = _context.Enterprise.Find(EnterpriseId);
            var enterpriseWithBank= _context.Enterprise.Where(x => x.Id == enterprise.Id).Include(x => x.Banking).Single();
            Banking bnk = enterpriseWithBank.Banking;
            SalaryProjectApplication application = new() { ClientId = client.Id, SalaryProjectId = salaryId,Banking=bnk };
            _context.SalaryProjectApplication.Add(application);
            bnk.SalaryProjectApplications.Add(application);
            _context.SaveChanges();
            
            return View();
            

        }


        public async Task<IActionResult> TransferApply()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransferApply(AccountTransferModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.Receiver);
                if (user == null )
                {
                    return View(model);
                }

                var userWithClients=_context.Users.Where(x => x.Id == user.Id).Include(x => x.Clients).Single();
                Client receiver = userWithClients.Clients.First();
                if (receiver == null)
                {
                    return View(model);
                }

                Banking bankReceiver = new();
                var clientWithBankings= _context.Client.Where(x => x.Id == receiver.Id).Include(x => x.Bankings).Single();
                foreach(var bnk in clientWithBankings.Bankings.ToList())
                {
                    if (model.BankNameReceiver == bnk.Name)
                    {
                        bankReceiver = bnk;
                    }
                }

                if (bankReceiver.Name!=model.BankNameReceiver)
                {
                    return View(model);
                }

                Account accountReceiver = new();
                var clientWithAccounts = _context.Client.Where(x => x.Id == receiver.Id).Include(x => x.Accounts).Single();
                foreach(var acc in clientWithAccounts.Accounts.ToList())
                {
                    if(acc.Banking== bankReceiver && acc.Name == model.AccountReceiver)
                    {
                        accountReceiver = acc;
                    }

                }

                if (accountReceiver.Name != model.AccountReceiver)
                {
                    return View(model);
                }

                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
                Client clientSender = userWithClient.Clients.First();

                Banking bankSender = new();
                var clWithBankings = _context.Client.Where(x => x.Id == clientSender.Id).Include(x => x.Bankings).Single();
                foreach (var bnk in clWithBankings.Bankings)
                {
                    if (bnk.Name == model.BankNameSender)
                    {
                        bankSender = bnk;
                    }

                }
                if (bankSender.Name != model.BankNameSender)
                {
                    return View(model);
                }

                var clWithAccounts= _context.Client.Where(x => x.Id == clientSender.Id).Include(x => x.Accounts).Single();
                Account accountSender = new();
                foreach(var acc in clWithAccounts.Accounts.ToList())
                {
                    if(acc.Banking==bankSender && acc.Name == model.AccountSender)
                    {
                        accountSender = acc;
                    }
                }

                if (accountSender.Name != model.AccountSender)
                {
                    return View(model);
                }

                if (accountSender.Sum < model.Sum || accountSender.State=="Freeze" || accountSender.State == "Block" || accountReceiver.State=="Freeze" || accountReceiver.State == "Block" )
                {
                    return View(model);
                }
                accountSender.Sum -= model.Sum;
                accountReceiver.Sum += model.Sum;

                _context.SaveChanges();
            }
            else
            {
                return View(model);
            }
            return View("TransferApplication");
        }
        public async Task<IActionResult> ChooseBankCredit()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bankings).Single();
            if (clientWithBanks.Bankings.ToList().Count == 0)
            {
                return View("EmptyBanks");
            }
            return View("Views/Client/Credit/ChooseBankCredit.cshtml", clientWithBanks.Bankings.ToList());

        }

        public async Task<IActionResult> ChooseBankInstallment()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bankings).Single();
            if (clientWithBanks.Bankings.ToList().Count == 0)
            {
                return View("EmptyBanks");
            }
            return View("Views/Client/Installment/ChooseBankInstallment.cshtml",clientWithBanks.Bankings.ToList());

        }

        [HttpGet]
        public async Task<IActionResult> Credit(int BankId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var ClientWithCredits = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Installments).Single();
            Dictionary<List<Credit>, int> creditsWithBank = new();
            List<Credit> credits = new();
            foreach(var credit in ClientWithCredits.Installments.ToList())
            {
                if(credit is Credit cr && credit.State!=false)
                {
                    credits.Add(cr);
                }
            }
            foreach (var credit in credits)
            {
                if (DateTime.Now > credit.TimeFinish)
                {
                    var creditWithAccounts = _context.Installment.Where(x => x.Id == credit.Id).Include(x => x.Account).Single();
                    credit.Account.State = "Block";
                }

            }
            creditsWithBank.Add(credits, BankId);

            return View("Views/Client/Credit/Credit.cshtml",creditsWithBank);
        }

        [HttpGet]
        public async Task<IActionResult> Installment(int BankId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var ClientWithInstallments = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Installments).Single();
            Dictionary<List<Installment>, int> installmentsWithBank = new();
            List<Installment> installments = new();
            foreach (var installment in ClientWithInstallments.Installments.ToList())
            {
                if (!(installment is Credit cr) && installment.State != false)
                {
                    installments.Add(installment);
                }
            }
            foreach (var installment in installments)
            {
                if (DateTime.Now > installment.TimeFinish)
                {
                    var installmentWithAccounts = _context.Installment.Where(x => x.Id == installment.Id).Include(x => x.Account).Single();
                    installment.Account.State = "Block";
                }

            }
            installmentsWithBank.Add(installments, BankId);

            return View("Views/Client/Installment/Installment.cshtml", installmentsWithBank);
        }

        [HttpGet]
        public async Task<IActionResult> PutSumToCreditOrInstallment(int id)
        {
            Installment installment = await _context.Installment.FindAsync(id);
            var installmentWithAccount = _context.Installment.Where(x => x.Id == installment.Id).Include(x => x.Account).Single();
            if (installmentWithAccount.Account.State == "Freeze")
            {
                return View("FreezingAccount");
            }
            if (installmentWithAccount.Account.State == "Block")
            {
                return View("BlockAccount");
            }
            List<double> money= money =new() { installmentWithAccount.Account.Sum, installment.CurrentSum, installmentWithAccount.MustSum,id };
           
            return View("Views/Client/Credit/PutSumToCreditOrInstallment.cshtml", money);

        }


        [HttpPost]
        public async Task<IActionResult> PutSumToCreditOrInstallment(string Sum,double CreditId)
        {
            int res;
            if(!Int32.TryParse(Sum,out res))
            {
                return View("PutSumError");

            }
            
            Installment creditOrInstallment = await _context.Installment.FindAsync((int)CreditId);
            var creditWithBank = _context.Installment.Where(x => x.Id == creditOrInstallment.Id).Include(x => x.Banking).Single();
            var creditWithAccount = _context.Installment.Where(x => x.Id == creditOrInstallment.Id).Include(x => x.Account).Single();
            var account = creditWithAccount.Account;
            var accountWithCredits = _context.Account.Where(x => x.Id == account.Id).Include(x => x.Installments).Single();

            var creditWithClient = _context.Installment.Where(x => x.Id == creditOrInstallment.Id).Include(x => x.Client).Single();
            var client = creditWithAccount.Client;
            var clientWithCredits = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Installments).Single();

            var bnkWithInstallments=_context.Banking.Where(x => x.Id == creditWithBank.BankingId).Include(x => x.Installments).Single();
            if (creditOrInstallment.MustSum- creditOrInstallment.CurrentSum< res)
            {
                return View("PutSumError");
            }
            else if (creditOrInstallment.MustSum - creditOrInstallment.CurrentSum == res && account.Sum < res)
            {
                return View("PutSumError");
            }
            else if (creditOrInstallment.MustSum - creditOrInstallment.CurrentSum == res && account.Sum > res)
            {
                account.Sum -= res;
                creditOrInstallment.CurrentSum += res;
                // accountWithCredits.Installments.Remove(creditOrInstallment);
                creditOrInstallment.State = false;
               // clientWithCredits.Installments.Remove(creditOrInstallment);
                //bnkWithInstallments.Installments.Remove(creditOrInstallment);
               // _context.Installment.Remove(creditOrInstallment);
                _context.SaveChanges();
            }
            else if (creditOrInstallment.MustSum - creditOrInstallment.CurrentSum > res && account.Sum > res)
            {
                account.Sum -= res;
                creditOrInstallment.CurrentSum += res;
                _context.SaveChanges();
            }
            else if (creditOrInstallment.MustSum - creditOrInstallment.CurrentSum > res && account.Sum < res)
            {
                return View("PutSumError");
            }
            string userName = HttpContext.User.Identity.Name;
            User user =await _userManager.FindByNameAsync(userName);
            var userWithClient = _context.Users.Where(x => x.Id == user.Id).Include(x => x.Clients).Single();


            MoneyStatictic money = new() { NameOpration = "PutSumToCreditOrInstallment", Status = true, Sum = res, ClientI = userWithClient.Clients.First().Id ,BankingI=creditWithBank.BankingId,InstallmentI=creditOrInstallment.Id};
            _context.MoneyStatictic.Add(money);
            _context.SaveChanges();
            return RedirectToAction("Index", "Client");

        }

        [HttpGet]
        public async Task<IActionResult> ApplyForCredit(int BankId)
        {
            CreditOrInstallmentApplying.BankId = BankId;
            return View("Views/Client/Credit/ApplyForCredit.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ApplyForInstallment(int BankId)
        {
            CreditOrInstallmentApplying.BankId = BankId;
            return View("Views/Client/Installment/ApplyForInstallment.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForCredit(CreditOrInstallmentApplying applying)
        {
            string AccountName=null;
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
                
                Client client = userWithClient.Clients.First();
                var clientWithAccounts= _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();

                var clientWithInstallments = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Installments).Single();

                foreach (var acc in clientWithAccounts.Accounts)
                {
                    if (applying.Account == acc.Name && acc.BankingId==CreditOrInstallmentApplying.BankId)
                    {
                        if (acc.State == "Freeze")
                        {
                            return View("FreezingAccount");
                        }
                        if (acc.State == "Block")
                        {
                            return View("BlockAccount");
                        }
                        AccountName = acc.Name;
                        applying.AccountId=acc.Id;
                        applying.ClientId = client.Id;
                        applying.ClientName = userName;

                        InstallmentCreation creation = new() { NeededSum = applying.NeededSum, MonthProcent = applying.MonthProcent, Month = applying.Month, AccountId = (int)applying.AccountId, ClientId = (int)applying.ClientId,BankId=(int)CreditOrInstallmentApplying.BankId,ClientName=applying.ClientName };
                        _context.InstallmentCreation.Add(creation);
                        _context.SaveChanges();
                    }
                }
                if (AccountName==null)
                {
                    return View("Views/Client/Credit/ApplyForCredit.cshtml", applying);
                }
             
                return View("Views/Client/Credit/ApplicationForCredit.cshtml");
            }
            else
            {
                return View("Views/Client/Credit/ApplyForCredit.cshtml",applying);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForInstallment(CreditOrInstallmentApplying applying)
        {
            string AccountName = null;
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();

                Client client = userWithClient.Clients.First();
                var clientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();

                var clientWithInstallments = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Installments).Single();

                foreach (var acc in clientWithAccounts.Accounts)
                {
                    if (applying.Account == acc.Name && acc.BankingId == CreditOrInstallmentApplying.BankId)
                    {
                        if (acc.State == "Freeze")
                        {
                            return View("FreezingAccount");
                        }
                        if (acc.State == "Block")
                        {
                            return View("BlockAccount");
                        }
                        AccountName = acc.Name;
                        applying.AccountId = acc.Id;
                        applying.ClientId = client.Id;
                        applying.ClientName = userName;

                        InstallmentCreation creation = new() { NeededSum = applying.NeededSum,  Month = applying.Month, AccountId = (int)applying.AccountId, ClientId = (int)applying.ClientId, BankId = (int)CreditOrInstallmentApplying.BankId, ClientName = applying.ClientName };
                        _context.InstallmentCreation.Add(creation);
                        _context.SaveChanges();
                    }
                }
                if (AccountName == null)
                {
                    return View("Views/Client/Credit/ApplyForCredit.cshtml", applying);
                }

                return View("Views/Client/Installment/ApplicationForInstallment.cshtml");
            }
            else
            {
                return View("Views/Client/Credit/ApplyForCredit.cshtml", applying);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChooseBankAccount()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bankings).Single();

            if (clientWithBanks.Bankings.ToList().Count==0)
            {
                return View("EmptyBanks");
            }
            return View(clientWithBanks.Bankings.ToList());

        }

        [HttpGet]
        public async Task<IActionResult> Account(int BankId)
        {
            
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();

            Client client = userWithClient.Clients.First();
            var ClientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();

            Banking bnk = await _context.Banking.FindAsync(BankId);
            Dictionary<List<Account>,int> accountBnk = new();
            List<Account> accountsWithBnk = new();
            foreach (var acc in ClientWithAccounts.Accounts.ToList())
            {
                if (acc.Banking == bnk)
                {
                    accountsWithBnk.Add(acc);
                }
            }
            accountBnk.Add(accountsWithBnk, BankId);
            return View(accountBnk);
        }

        [HttpGet]
        public IActionResult BankAccountCreate(int BankId)
        {
            Banking bnk = _context.Banking.Find(BankId);
            AccountCreation.banking = bnk;
            return View("BankAccountCreate");
        }
        
            
        [HttpPost]
        public async Task<IActionResult> BankAccountCreate(AccountCreation creation)
        {
            uint res;
            if (!UInt32.TryParse(creation.Sum, out res))
            {
                return View(creation);
            }
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithAccounts= _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            var clientWithBankings = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bankings).Single();
            Banking bank = _context.Banking.Find(AccountCreation.banking.Id);
            
            if (ModelState.IsValid)
            {
                foreach(var el in clientWithAccounts.Accounts.ToList())
                {
                   
                    if (el.Name == creation.Name &&  el.Banking== bank)
                    {
                        return View(creation);
                    }
                }
                Account acc = new () { Name = creation.Name, Sum = res, Client=client,Banking= bank,State="Usual"};
               
                await _context.Account.AddAsync(acc);

                bank.Accounts.Add(acc);
                
                client.Accounts.Add(acc);
                _context.SaveChanges();

                return RedirectToAction("Index", "Client");
            }
            else
            {
               return View(creation);
            }    
  ;            
        }

        public async Task<IActionResult>  StatusChangeToFreeze(int id)
        {
            Account acc = _context.Account.Find(id);
            acc.State = "Freeze";
            _context.SaveChanges();
            return RedirectToAction("Index", "Client");
        }

        public async Task<IActionResult> StatusChangeToUsual(int id)
        {
            Account acc = _context.Account.Find(id);
            acc.State = "Usual";
            _context.SaveChanges();
            return RedirectToAction("Index", "Client");
        }

        public async Task<IActionResult> BankAccountClose(int id)
        {
            var account = await _context.Account.FindAsync(id);
      
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();

            Client client = userWithClient.Clients.First();
            var clientwithaccounts=_context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();

            Banking bank = await _context.Banking.FindAsync(account.BankingId);
            _context.Account.Remove(account);
            if (clientwithaccounts.Accounts.Contains(account))
            {
                client.Accounts.Remove(account);

            }
            bank.Accounts.Remove(account);
            _context.SaveChanges();
            return RedirectToAction("Index", "Client");

        }

        public async Task<IActionResult> Redact(int id)
        {
            var account = await _context.Account.FindAsync(id);
            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> AddSumToAccount(int id)
        {
            AddingGettingMoney.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSumToAccount(AddingGettingMoney model)
        {
            uint res;
            if (!UInt32.TryParse(model.Sum, out res))
            {
                return View(model);

            }
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
                Client client = userWithClient.Clients.First();
                var clientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                
                var account=_context.Account.Find(AddingGettingMoney.Id);
                account.Sum += res;

                var accountWithBank= _context.Account.Where(x => x.Id == account.Id).Include(x => x.Banking).Single();
                MoneyStatictic money = new() { NameOpration = "AddSumToAccount", ClientI = client.Id, Status = true, Sum = (int)res, BankingI = accountWithBank .Banking.Id,AccountI=account.Id};
                _context.MoneyStatictic.Add(money);
                _context.SaveChanges();


                _context.SaveChanges();
            }
          
            else
            {
                return View(model);
            }

            return View("Index", "Client");
        }

        [HttpGet]
        public async Task<IActionResult> TakeSumFromAccount(int id)
        {
            AddingGettingMoney.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TakeSumFromAccount(AddingGettingMoney model)
        {
            uint res;
            if (!UInt32.TryParse(model.Sum, out res))
            {
                return View(model);

            }
            if (ModelState.IsValid)
            {
                var account = _context.Account.Find(AddingGettingMoney.Id);
                if (account.Sum < res)
                {
                    return View(model);
                }
                account.Sum -= res;
                _context.SaveChanges();
            }

            else
            {
                return View(model);
            }
            return View("Index");

        }

    }
}
