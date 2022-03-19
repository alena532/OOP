using System.ComponentModel.DataAnnotations;

namespace BankApplication.ViewModel.Client
{
    public class AddingGettingMoney
    {
        static public int Id { get; set; }
        [Required]
        [Display(Name = "Sum")]
        public string Sum { get; set; }
    }
}
