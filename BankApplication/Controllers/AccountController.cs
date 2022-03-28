using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankApplication.ViewModel;
using BankApplication.Models;
using BankApplication.Models.Entity;
using BankApplication.Data;
using BankApplication.ViewModel.Account;

namespace Bank.Controllers
{
    public class AccountController:Controller
    {
        UserManager<User> _userManager;
        readonly SignInManager<User> _signManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;
        
        

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            
        }

        [HttpGet]
        public async Task<IActionResult> Registration(string userRole)
        {
            await Initializer.InitializeAsync(_userManager, _roleManager, _signManager);
            await BankRepository.Initialize(_context,_userManager);
            await SendSalary.Initialize(_userManager, _context);
            return View("RegistrationUser");
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationUser(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {

                User user = new User { Email = model.Email, UserName = model.UserName, PhoneNumber = model.Number,PassportSeries = model.PassportSeries, PassportNumber = model.PassportNumber, IdentificationNumber = model.IdentificationNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "Client");
                    Client client = new Client() { User = user };
                    client.User = user;
                    user.Clients.Add(client);
                    _context.Client.Add(client);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Client");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                //return View(model);
                /* if (model.Role == "Client")
                 {
                     UserCreation creation = new() { Email = model.Email, UserName = model.UserName, PhoneNumber = model.Number, PassportSeries = model.PassportSeries, PassportNumber = model.PassportNumber, IdentificationNumber = model.IdentificationNumber, Password = model.Password, BankName = model.BankName };
                     _context.UserCreation.Add(creation);
                     _context.SaveChanges();
                     return RedirectToAction("Index", "Home");
                 }

                 else
                 {
                     var result = await _userManager.CreateAsync(user, model.Password);
                     if (result.Succeeded)
                     {
                         await _signManager.SignInAsync(user, false);
                         await _userManager.AddToRoleAsync(user, model.Role);
                         Banking bank = BankRepository.FindByName(_context,model.BankName);


                         switch (model.Role)
                         {
                             case "Administrator":
                                 var admin = new Administrator { Bank = bank,User=user};
                                 bank.Administrators.Add(admin);
                                 user.Administrators.Add(admin);
                                 await _context.Administrator.AddAsync(admin);
                                 _context.SaveChanges();
                                 break;
                             case "Manager":
                                 Manager man = new Manager() { Bank = bank,User=user};
                                  bank.Managers.Add(man);
                                  user.Managers.Add(man);
                                 await _context.Manager.AddAsync(man);
                                 _context.SaveChanges();
                                 break;
                             case "Operator":
                                 var op = new Operator {Bank = bank,User=user };
                                 bank.Operators.Add(op);
                                 user.Operators.Add(op);
                                 await _context.Operator.AddAsync(op);
                                 _context.SaveChanges();
                                 break;
                         }
                         return RedirectToAction("Index", "Home");
                     }

                     else
                     {
                        foreach (var error in result.Errors)
                         {
                             ModelState.AddModelError(string.Empty, error.Description);
                         }
                     }
                 }
                */
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        public async Task<IActionResult> ShowAllUsers()
        {
            return View( _userManager.Users.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result= await _signManager.PasswordSignInAsync(model.NameUser, model.Password, model.RememberMe, false);//123@Qa
                
                 if (result.Succeeded)
                 {
                
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                 }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
       
    }
}
