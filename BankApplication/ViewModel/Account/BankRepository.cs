
using BankApplication.Data;
using BankApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BankApplication.Models
{
    public static class BankRepository
    {
        static UserManager<User> _userManager;
        static RoleManager<User> _roleManager;
        static ApplicationDbContext _context;
        static List<Banking> banks { get; set; } = new();
        public  static async Task Initialize(ApplicationDbContext context )
        {
            _context = context;
            if(await  _context.Banking.FindAsync(1) ==null)
            {
               await  _context.Banking.AddAsync(new Banking() { Name="BSB"});
            }

            if (await _context.Banking.FindAsync(2) == null)
            {
                await _context.Banking.AddAsync(new Banking() { Name = "BelarusBank" });
            }
            if (await _context.Banking.FindAsync(3) == null)
            {
               await _context.Banking.AddAsync(new Banking() { Name = "BelarusBank" });
            }
            _context.SaveChanges();

           
           
            
        }

        public static Banking? FindByName(ApplicationDbContext context,string name)
        {
           foreach(var bnk in context.Banking)
            {
                if (bnk.Name == name)
                {
                    return bnk;
                }
            }
            return null;

        }
       

    }
}
