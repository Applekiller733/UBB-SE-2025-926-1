using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.ViewModel.BankViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace LoanShark.MVC.Models
{
    public class BankAccountDetailsModel
    {
        public BankAccount BankAccount { get; set; }

    }
}
