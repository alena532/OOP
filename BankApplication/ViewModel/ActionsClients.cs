using BankApplication.Models;
using BankApplication.Models.Entity;
using System.Collections.Generic;
namespace BankApplication.Models
{
    public static class ActionsClients
    {
        public static List<string> NamesOperations { get; set; } = new();
        public static List<string> NamesUsers { get; set; } = new();
        public static List<Client> ClientsId { get; set; } = new();
        public static Dictionary<string, string> Names { get; set; } = new();
    }
}
