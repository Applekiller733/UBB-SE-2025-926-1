namespace LoanShark.MVC.Models
{
    public class BankAccountEditModel
    {
        public string Iban { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double DailyLimit { get; set; }
        public double MaximumPerTransaction { get; set; }
        public int MaximumNrTransactions { get; set; }
        public bool IsBlocked { get; set; }
    }
}
