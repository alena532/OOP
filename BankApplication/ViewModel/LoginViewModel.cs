using System.ComponentModel.DataAnnotations;
namespace BankApplication.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string NameUser { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
