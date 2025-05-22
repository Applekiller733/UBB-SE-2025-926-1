using LoanShark.Domain;

namespace LoanShark.MVC.Models
{
    public class LoansViewModel
    {
        public List<Loan>? Loans { get; set; }
        public List<Loan>? UnpaidLoans { get; set; }
    }
}
