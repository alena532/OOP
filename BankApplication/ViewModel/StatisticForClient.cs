using BankApplication.Models;

namespace BankApplication.ViewModel
{
    public class MoneyStatisticForClient
    {
        public static Stack<MoneyStatictic> IndividualStatistic { get; set; } = new();

        public static List<MoneyStatictic> IndividualInformation { get; set; } = new();
        public static bool Status { get; set; } = true;
    }
}
