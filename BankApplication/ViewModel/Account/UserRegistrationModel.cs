using Microsoft.EntityFrameworkCore;
using BankApplication.Models;
using System.ComponentModel.DataAnnotations;
namespace BankApplication.ViewModel

{
    public class UserRegistrationModel
    {
       // [Required]
       // [Display(Name = "Role")]
       // public string Role { get; set; }

       // [Required]
      //  [Display(Name = "Bank")]
      //  public string BankName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Numer")]
        public string Number { get; set; }

        [Required]
        [Display(Name = "Passport series")]
        public string PassportSeries { get; set; }

        [Required]
        [Display(Name = "Passport number")]
        public int PassportNumber { get; set; }

        [Required]
        [Display(Name = "Identification number")]
        public string IdentificationNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords are not equals")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
