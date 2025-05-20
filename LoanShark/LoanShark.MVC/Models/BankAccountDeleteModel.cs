using LoanShark.ViewModel.BankViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace LoanShark.MVC.Models
{
    public class BankAccountDeleteModel
    {
        public string IBAN { get; set; } = string.Empty;
    }

}
