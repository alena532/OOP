using BankApplication.Models;

namespace BankApplication.ViewModel.Client
{
    public static class CreditApplyingRepository
    {
        public static Dictionary<CreditApplying, string> applyings { get; set; } = new();
    }
}
