using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankApplication.Models;
using BankApplication.Models.Entity;
using BankApplication.ViewModel;
using BankApplication.Data;
using Microsoft.EntityFrameworkCore;
using BankApplication.ViewModel.Client;


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
            

        }
        public async Task<IActionResult> Index()
        { 
           string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            return View(userWithClient.Clients.First());//account,creditst:menu
        }

        [HttpGet]
        public async Task<IActionResult> Credit()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var ClientWithCredits = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Credits).Single();

            return View(ClientWithCredits.Credits);
        }

        [HttpGet]
        public async Task<IActionResult> PutSumToCredit(int id)
        {
            Credit credit = await _context.Credit.FindAsync(id);
            var creditWithAccount = _context.Credit.Where(x => x.Id == credit.Id).Include(x => x.Account).Single();
            List<double> money= money =new() { creditWithAccount.Account.Sum, credit.CurrentSum, credit.MustSum,id };
           
            return View(money);

        }

        [HttpPost]
        public async Task<IActionResult> PutSumToCredit(string Sum,double CreditId)
        {
            int res;
            if(!Int32.TryParse(Sum,out res))
            {
                return RedirectToAction("Index", "Client");

            }
            
            Credit credit = await _context.Credit.FindAsync((int)CreditId);
            var creditWithAccount = _context.Credit.Where(x => x.Id == credit.Id).Include(x => x.Account).Single();
            var account = creditWithAccount.Account;
            var accountWithCredit= _context.Account.Where(x => x.Id == account.Id).Include(x => x.Credits).Single();

            var creditWithClient = _context.Credit.Where(x => x.Id == credit.Id).Include(x => x.Client).Single();
            var client = creditWithAccount.Client;
            var clientWithCredits = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Credits).Single();
            if (credit.MustSum-credit.CurrentSum< res)
            {
                return View("PutSumToCredit",(int)CreditId);
            }
            else if (credit.MustSum - credit.CurrentSum == res)
            {
                accountWithCredit.Credits.Remove(credit);
                clientWithCredits.Credits.Remove(credit);
                _context.Credit.Remove(credit);
                _context.SaveChanges();
            }
            else if (credit.MustSum - credit.CurrentSum > res && account.Sum>res)
            {
                account.Sum -= res;
                credit.CurrentSum += res;
                _context.SaveChanges();
            }
            else if (credit.MustSum - credit.CurrentSum > res && account.Sum < res)
            {
                return View("PutSumToCredit", (int)CreditId);
            }


            return RedirectToAction("Index", "Client");

        }

        [HttpGet]
        public async Task<IActionResult> ApplyForCredit() => View();

        [HttpPost]
        public async Task<IActionResult> ApplyForCredit(CreditApplying applying)
        {
            string AccountName=null;
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
                
                Client client = userWithClient.Clients.First();
                var clientWithAccounts= _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                var clientWithBank = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Bank).Single();
                foreach (var acc in clientWithAccounts.Accounts)
                {
                    if (applying.Account == acc.Name)
                    {
                        AccountName = acc.Name;
                        applying.AccountId=acc.Id;
                        applying.ClientId = client.Id;
                       
                        CreditCreation creation = new() { NeededSum = applying.NeededSum, MonthProcent = applying.MonthProcent, Month = applying.Month, AccountId = (int)applying.AccountId, ClientId = (int)applying.ClientId,BankName=clientWithBank.Bank.Name };
                        _context.CreditCreation.Add(creation);
                        _context.SaveChanges();
                    }
                }
                if (AccountName==null)
                {
                    return View(applying);
                }
             
                return RedirectToAction("Index", "Client");
            }
            else
            {
                return View(applying);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var ClientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            
            return View(ClientWithAccounts.Accounts);
           
        }


        [HttpGet]
        public IActionResult BankAccountCreate() => View();
        
            
        [HttpPost]
        public async Task<IActionResult> BankAccountCreate(AccountCreation creation)
        {
            int res;
            if (!Int32.TryParse(creation.Sum, out res))
            {
                return View(creation);

            }
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithAccounts= _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();

            if (ModelState.IsValid)
            {
                foreach(var el in clientWithAccounts.Accounts.ToList())
                {
                   
                    if (el.Name == creation.Name )
                    {
                        return View(creation);
                    }
                }
                Account acc = new () { Name = creation.Name, Sum = res };
               
                await _context.Account.AddAsync(acc);

                
                
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

        public async Task<IActionResult> BankAccountClose(int id)
        {
            var account = await _context.Account.FindAsync(id);
            var accountWithCredits = _context.Account.Where(x => x.Id == account.Id).Include(x => x.Credits).Single();
            if (accountWithCredits.Credits.Count() != 0)
            {
                return RedirectToAction("Index", "Client");
            }
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientwithaccounts=_context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            _context.Account.Remove(account);
            if (clientwithaccounts.Accounts.Contains(account))
            {
                client.Accounts.Remove(account);

            }

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
            int res;
            if (!Int32.TryParse(model.Sum, out res))
            {
                return View(model);

            }
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
                Client client = userWithClient.Clients.First();
                var clientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                
                MoneyStatictic money = new() { NameOpration = "Refill", UserName = HttpContext.User.Identity.Name, ClientI = client.Id };
                _context.MoneyStatictic.Add(money);
                _context.SaveChanges();


                var account=_context.Account.Find(AddingGettingMoney.Id);
                account.Sum += res;


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
            int res;
            if (!Int32.TryParse(model.Sum, out res))
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
