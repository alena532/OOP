using BankApplication.Data;
using BankApplication.Models;
using BankApplication.Models.Entity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankApplication.Models;
using BankApplication.Models.Entity;
using BankApplication.ViewModel;
using BankApplication.Data;
using Microsoft.EntityFrameworkCore;
namespace BankApplication.ViewModel.Account
{
    public class SendSalary
    {
        static UserManager<User> _userManager;
        static RoleManager<User> _roleManager;
        static ApplicationDbContext _context;
        static List<Banking> banks { get; set; } = new();
        public static async Task Initialize(UserManager<User> userManager,ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;

            if (DateTime.Now.Day == 25)
            {
                foreach (var bnk in _context.Banking.ToList())
                {
                    var bnkWithClients = _context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
                    foreach (var client in bnkWithClients.Clients.ToList())
                    {
                        if (client.ProfessionName != null)
                        {
                            var clientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                            foreach (var acc in clientWithAccounts.Accounts.ToList())
                            {
                                if (acc.Name == client.ProfessionName && acc.IsSalaryDay != true)
                                {
                                    acc.Sum += (double)client.PayForProfession;
                                    acc.IsSalaryDay = true;
                                }
                            }
                        }
                    }
                    _context.SaveChanges();
                }

            }
            if(!(DateTime.Now.Day == 25))
            {
                foreach (var bnk in _context.Banking.ToList())
                {
                    var bnkWithClients =_context.Banking.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
                    foreach (var client in bnkWithClients.Clients.ToList())
                    {
                        if (client.ProfessionName != null)
                        {
                            var clientWithAccounts = _context.Client.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                            foreach (var acc in clientWithAccounts.Accounts.ToList())
                            {
                                if (acc.Name == client.ProfessionName && acc.IsSalaryDay == true)
                                {

                                    acc.IsSalaryDay = false;
                                }
                            }
                        }
                    }

                }
                _context.SaveChanges();
            }
                
        }
    }
}
