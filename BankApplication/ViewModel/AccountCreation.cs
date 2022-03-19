using Microsoft.EntityFrameworkCore;
using BankApplication.Models;
using System.ComponentModel.DataAnnotations;
namespace BankApplication.ViewModel
{
    public class AccountCreation
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Sum")]
        public  string Sum { get; set; }
    }
}
