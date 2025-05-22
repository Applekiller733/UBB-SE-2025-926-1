using LoanShark.Domain;

namespace LoanShark.MVC.Models

{
    public class PayLoanViewModel
    {
        public List<string> BankAccounts { get; set; } = new();
        public string SelectedBankAccount { get; set; }

        public int? SelectedLoanId { get; set; }
        public List<Loan> UnpaidLoans { get; set; } = new();
        public Loan SelectedLoanDisplay { get; set; }

        public string SelectedLoanAmount { get; set; }
        public string SelectedAccountBalance { get; set; }
        public string ConvertedLoanAmount { get; set; }

        public string PayErrorMessage { get; set; }
    }
}
