namespace BankApplication.Models
{
    public class MoneyTransactionRequest
    {
        public int Id { get; set; }
        public int SenderEnterpriseSpecialistId { get; set; }
        public double Sum { get; set; }
       
        public int ReceiverClientId { get; set; }
        public int AccountId { get; set; }

        public int Property
        {
            get => default;
            set
            {
            }
        }
    }
}
