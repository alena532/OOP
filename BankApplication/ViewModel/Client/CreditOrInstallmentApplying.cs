using BankApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace BankApplication.ViewModel.Client
{
    public class CreditOrInstallmentApplying
    {
        [Required]
        [Display(Name = "Bank account")]
        public string Account { get; set; }
      
        [Required]
        [Display(Name = "Sum")]
        public double NeededSum { get; set; }

       // [Required]
       // [Display(Name = "MonthProcent")]
        public int? MonthProcent { get; set; }

        [Required]
        [Display(Name = "Amount of month")]
        public int Month { get; set; }

        public  int? AccountId { get; set; }
        public  int? ClientId { get; set; }
        public static int? BankId { get; set; }
        public string? ClientName { get; set; }

       
    }
}
