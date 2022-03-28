using System.ComponentModel.DataAnnotations;
namespace BankApplication.ViewModel.Account
{
    public class AccountTransferModel
    {
        [Required]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        [Required]
        [Display(Name = "Bank name receiver")]
        public string BankNameReceiver { get; set; }
        [Required]
        [Display(Name = "Account receiver")]
        public string AccountReceiver { get; set; }

        [Required]
        [Display(Name = "Bank name sender")]
        public string BankNameSender { get; set; }

        [Required]
        [Display(Name = "Account")]
        public string AccountSender { get; set; }

        [Required]
        [Display(Name = "Sum")]
        public double Sum { get; set; }

    }
}
