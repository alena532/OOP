
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
using BankApplication.ViewModel.Client;
using BankApplication.ViewModel.Account;

namespace BankApplication.Models
{
    public static class BankRepository
    {
        static UserManager<User> _userManager;
        static RoleManager<User> _roleManager;
        static ApplicationDbContext _context;
        static List<Banking> banks { get; set; } = new();
        public  static async Task Initialize(ApplicationDbContext context,UserManager<User> userManager )
        {
            _context = context;
            _userManager = userManager;

            if(await  _context.Banking.FindAsync(1) ==null)
            {
               await  _context.Banking.AddAsync(new Banking() { Name="BSB"});
            }

            if(await _context.Manager.FindAsync(1) == null)
            {
                User user = new User { Email = "example@.ru", UserName = "manager", PhoneNumber = "3775510", PassportSeries = "HB", PassportNumber = 3245599, IdentificationNumber = "HBf345343" };

                var result = await _userManager.CreateAsync(user, "123@Qa");
                
                await _userManager.AddToRoleAsync(user, "Manager");
                Banking banking = await _context.Banking.FindAsync(1);
                Manager manager = new() { User = user, Bank = banking };
                var bnkWithManagers = _context.Banking.Where(x => x.Id == banking.Id).Include(x => x.Managers).Single();
                bnkWithManagers.Managers.Add(manager);
                _context.Manager.Add(manager);
                user.Managers.Add(manager);
                _context.SaveChanges();
            }

            
            if (await _context.Operator.FindAsync(1) == null)
            {
                User user = new User { Email = "example@.ru", UserName = "operator", PhoneNumber = "3775510", PassportSeries = "HB", PassportNumber = 3245599, IdentificationNumber = "HBf345343" };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Operator");
                Banking banking = await _context.Banking.FindAsync(1);
                Operator op = new() { User = user, Bank = banking };
                banking.Operators.Add(op);
                _context.Operator.Add(op);
                
                user.Operators.Add(op);
                _context.SaveChanges();

            }

            

            if (await _context.Enterprise.FindAsync(1) == null)
            {
                await _context.Enterprise.AddAsync(new Enterprise() { Type = "Individual Entrepreneur", LegalName = "Vorobey", PayerAccountNumber = 54567745, BankIdentificationNumber = 456343434, LegalAddress = "Phrunzenskaya", Banking = await _context.Banking.FindAsync(1) });
                _context.SaveChanges();

            }

            if (await _context.Enterprise.FindAsync(2) == null)
            {
                await _context.Enterprise.AddAsync(new Enterprise() { Type = "Individual Entrepreneur", LegalName = "Maya", PayerAccountNumber = 54567745, BankIdentificationNumber = 456343434, LegalAddress = "Pushkina", Banking = await _context.Banking.FindAsync(1) });
                _context.SaveChanges();

            }
            if (await _context.Enterprise.FindAsync(3) == null)
            {
                await _context.Enterprise.AddAsync(new Enterprise() { Type = "LLC", LegalName = "MozyrSalt", PayerAccountNumber = 54567745, BankIdentificationNumber = 456343434, LegalAddress = "Yakuba Kolasa", Banking = await _context.Banking.FindAsync(1) });
                _context.SaveChanges();

            }

            if (await _context.SalaryProject.FindAsync(1) == null)
            {

                Enterprise enterprise1 = await _context.Enterprise.FindAsync(1);
                SalaryProject accountant = new() { ProfessionName = "Accountant", Salary = 900, Enterprise = enterprise1 };
                await _context.SalaryProject.AddAsync(accountant);
                enterprise1.SalaryProjects.Add(accountant);

                SalaryProject salesman = new() { ProfessionName = "Salesman", Salary = 700, Enterprise = enterprise1 };
                await _context.SalaryProject.AddAsync(salesman);
                enterprise1.SalaryProjects.Add(salesman);

                SalaryProject director = new() { ProfessionName = "Director", Salary = 1900, Enterprise = enterprise1 };
                await _context.SalaryProject.AddAsync(director);
                enterprise1.SalaryProjects.Add(director);

                _context.SaveChanges();
               


            }

            if (await _context.EnterpriseSpecialist.FindAsync(1) == null)
            {
                User user = new User { Email = "example@.ru", UserName = "enterprise", PhoneNumber = "3775510", PassportSeries = "HB", PassportNumber = 3245599, IdentificationNumber = "HBf345343" };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Enterprise specialist");
                Banking banking = await _context.Banking.FindAsync(1);
                EnterpriseSpecialist ent = new() { User = user, Banking = banking,Enterprise=await _context.Enterprise.FindAsync(1) };
                _context.EnterpriseSpecialist.Add(ent);
                banking.EnterpriseSpecialists.Add(ent);
                _context.SaveChanges();
            }

            if (await _context.SalaryProject.FindAsync(4) == null)
            {

                Enterprise enterprise2 = await _context.Enterprise.FindAsync(2);
                SalaryProject programmer = new() { ProfessionName = "Programmer", Salary = 1800, Enterprise = enterprise2 };
                await _context.SalaryProject.AddAsync(programmer);
                enterprise2.SalaryProjects.Add(programmer);

                SalaryProject manager = new() { ProfessionName = "Manager", Salary = 900, Enterprise = enterprise2 };
                await _context.SalaryProject.AddAsync(manager);
                enterprise2.SalaryProjects.Add(manager);


                _context.SaveChanges();



            }

            if (await _context.EnterpriseSpecialist.FindAsync(2) == null)
            {
                User user = new User { Email = "example@.ru", UserName = "enterpriseMaya", PhoneNumber = "3775510", PassportSeries = "HB", PassportNumber = 3245599, IdentificationNumber = "HBf345343" };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Enterprise specialist");
                Banking banking = await _context.Banking.FindAsync(1);
                EnterpriseSpecialist ent = new() { User = user, Banking = banking, Enterprise = await _context.Enterprise.FindAsync(2) };
                _context.EnterpriseSpecialist.Add(ent);
                banking.EnterpriseSpecialists.Add(ent);
                _context.SaveChanges();
            }


            if (await _context.Banking.FindAsync(2) == null)
            {
                await _context.Banking.AddAsync(new Banking() { Name = "BelarusBank" });
            }
            if (await _context.Banking.FindAsync(3) == null)
            {
               await _context.Banking.AddAsync(new Banking() { Name = "AlphaBank" });
            }
            _context.SaveChanges();

           
           _context.SaveChanges();
           
                
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
