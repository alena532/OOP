using BankApplication.Models;
using BankApplication.Models.Entity;

namespace BankApplication.ViewModel
{
    public class MoneyTransactionFromEnterprise
    {
        public string[] Enterprises { get; set; } = new string[100];
        public double[] Sums { get; set; } = new double [100];
        public Dictionary<BankApplication.Models.Entity.Client, User> ClientUsers { get; set; } = new();
        public BankApplication.Models.Account[] Accounts { get; set; } = new BankApplication.Models.Account[100];
        public int[] MoneyTransactionRequestId { get; set; } = new int [100];
    }
}
