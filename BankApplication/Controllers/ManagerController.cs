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

        public IActionResult WatchingStatistics()
        {
            return View(_context.MoneyStatictic.ToList());
        }

        public IActionResult CreditRegistr()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();
            var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Banking bnk = _context.Banking.Find(managerWithBank.BankingId);
            List<CreditCreation> creations = new();
            foreach(var el in _context.CreditCreation.ToList())
            {
                if (el.BankName == bnk.Name)
                {
                    creations.Add(el);
                }
            }
           // Dictionary<Credit, string> applyings = new();
            return View(creations);
        }

        [HttpPost]
        public IActionResult RejectCredit(int Id)
        {
            CreditCreation creation = _context.CreditCreation.Find(Id);
            
            _context.CreditCreation.Remove(creation);
            _context.SaveChanges();
            List<CreditCreation> creations = new();
            foreach (var el in _context.CreditCreation.ToList())
            {
                if (el.BankName == creation.BankName)
                {
                    creations.Add(el);
                }
            }

            return View("CreditRegistr", creations);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCredit(int Id)
        {
            CreditCreation creation = _context.CreditCreation.Find(Id);
            
            Credit credit = new Credit() { MonthProcent = creation.MonthProcent, TimeBegin = DateTime.Now, TimeFinish = DateTime.Now.AddMonths(creation.Month), MustSum = (int)(creation.Month * creation.MonthProcent * 0.01 * creation.NeededSum + creation.NeededSum), CurrentSum = 0, LengthMonth = creation.Month };
            _context.Credit.Add(credit);
            Client client = await _context.Client.FindAsync(creation.ClientId);
            client.Credits.Add(credit);
            Account account= await _context.Account.FindAsync(creation.AccountId);
            account.Credits.Add(credit);
            account.Sum += creation.NeededSum;
            _context.CreditCreation.Remove(creation);
            _context.SaveChanges();
            List<CreditCreation> creations = new();
            foreach (var el in _context.CreditCreation.ToList())
            {
                if (el.BankName == creation.BankName)
                {
                    creations.Add(el);
                }
            }


            return View("CreditRegistr", creations);
        }
        public IActionResult ClientRegistr()
        {
            string userName = HttpContext.User.Identity.Name;
             var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
             Manager manager = userWithManager.Managers.First();
             var managerWithBank = _context.Manager.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
             Banking bnk = _context.Banking.Find(managerWithBank.BankingId);
            List<UserCreation> creations = new();
             foreach(var el in  _context.UserCreation.Where(x => x.BankName == bnk.Name).Select(x => x))
             {
                creations.Add(el);
             }
            return View(creations);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int userName)
        {
            UserCreation creation = _context.UserCreation.Find(userName);
            User user = new User { Email = creation.Email, UserName = creation.UserName, PhoneNumber = creation.PhoneNumber, PassportSeries = creation.PassportSeries, PassportNumber = creation.PassportNumber, IdentificationNumber = creation.IdentificationNumber };

            var result = await _userManager.CreateAsync(user, creation.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _userManager.AddToRoleAsync(user, "Client");
 
                Banking bank = BankRepository.FindByName(_context,creation.BankName);
                var client = new Client { Bank = bank,User=user };
               
               
                await  _context.Client.AddAsync(client);
                _context.SaveChanges();
                bank.Clients.Add(client);

                user.Clients.Add(client);
                
                _context.UserCreation.Remove(creation);
                _context.SaveChanges();
                List<UserCreation> creations = new();
                foreach (var el in _context.UserCreation.Where(x => x.BankName == creation.BankName).Select(x => x))
                {
                    
                    creations.Add(el);
                }
                return View("ClientRegistr",creations);
            }
            return View("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int userName)
        {
            UserCreation creation = _context.UserCreation.Find(userName);
            _context.UserCreation.Remove(creation);
            _context.SaveChanges();
            List<UserCreation> creations = new();
            foreach (var el in _context.UserCreation.Where(x => x.BankName == creation.BankName).Select(x => x))
            {
                creations.Add(el);
            }

            return View("ClientRegistr", creations);
        }
       
    }
}
