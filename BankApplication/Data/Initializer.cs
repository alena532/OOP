using BankApplication.Models;  
using Microsoft.AspNetCore.Identity;
namespace BankApplication.Data
{
    public static class Initializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {

            if (await roleManager.FindByNameAsync("Client") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }

            if (await roleManager.FindByNameAsync("Operator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Operator"));
            }

            if (await roleManager.FindByNameAsync("Manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }

            if (await roleManager.FindByNameAsync("Enterprise specialist") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Enterprise specialist"));
            }

            if (await roleManager.FindByNameAsync("Administrator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            


            /* string userName, email, password, role, passportSeries, identificationNumber, bankName;
             int number, passportNumber;

             email = "admin@example.com";
             userName = "Иванов Ивано ";
             password = "@!Secret1";
             role = "Administrator";
             number = 34751;
             passportSeries = "HB";
             passportNumber = 2466;
             identificationNumber = "23457";
             bankName = "BSB";

                 if (await userManager.FindByNameAsync("Ivam") == null)
                 {
                     //User user = new User { Email = email, UserName = userName, Number = number, PassportSeries = passportSeries, PassportNumber = passportNumber, IdentificationNumber = identificationNumber, BankName = bankName };
                     User user = new User { Email = "str", UserName = "Ivam", Number = 23, PassportSeries = "HB", PassportNumber = 234, IdentificationNumber = "dsdd", BankName = "BSB" };
                     IdentityResult result = await userManager.CreateAsync(user, "_Aa123456");

                     if (!result.Succeeded)
                     {
                         throw new Exception();
                         await signInManager.SignInAsync(user, false);
                         await userManager.AddToRoleAsync(user, role);

                      }

                 }
            */


        }
    }
}
