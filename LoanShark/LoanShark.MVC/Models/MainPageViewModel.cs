using LoanShark.Domain;
using System.Collections.Generic;

namespace LoanShark.MVC.Models
{
    public class MainPageViewModel
    {
        public string WelcomeText { get; set; } = "Welcome to LoanShark!";
        public List<BankAccount> BankAccounts { get; set; } = new();
        public string? SelectedAccountIban { get; set; }
        public string? BalanceButtonContent { get; set; }

    }
}