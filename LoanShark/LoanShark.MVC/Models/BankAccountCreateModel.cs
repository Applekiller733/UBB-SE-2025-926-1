using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace LoanShark.MVC.Models
{
    public class BankAccountCreateModel
    {
        [Required(ErrorMessage = "Please select a currency.")]
        public CurrencyItemModel SelectedCurrency { get; set; }

        [Required(ErrorMessage = "Please enter a custom name.")]
        public string CustomName { get; set; }

        public List<CurrencyItemModel> AvailableCurrencies { get; set; } = new();
    }

}
