using BankApplication.Models;

namespace BankApplication.ViewModel
{
    public class StatisticForEnterpriseSpecialist
    {
        public static List<EnterpriseSpecialistStatistic> Information { get; set; } = new();
        public static Stack<EnterpriseSpecialistStatistic> Statistic { get; set; } = new();
    }
}
