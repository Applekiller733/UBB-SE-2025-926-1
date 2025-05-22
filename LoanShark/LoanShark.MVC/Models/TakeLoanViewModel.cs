using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanShark.MVC.Models
{
    public class TakeLoanViewModel
    {
        [Required(ErrorMessage = "Please select a bank account")]
        public string SelectedBankAccount { get; set; }
         
        public List<string> BankAccounts { get; set; } = new();

        [Required(ErrorMessage = "Please enter an amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select the loan duration")]
        public int SelectedMonths { get; set; }

        public List<int> Months { get; set; } = new();

        public decimal TaxPercentage { get; set; }

        public decimal AmountToPay { get; set; }

        public string TakeErrorMessage { get; set; }
    }
}
