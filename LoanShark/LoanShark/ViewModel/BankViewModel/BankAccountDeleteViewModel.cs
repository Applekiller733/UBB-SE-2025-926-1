using System;
using System.Diagnostics;
using System.Windows.Input;
using LoanShark.Helper;
using LoanShark.Service.BankService;
using LoanShark.View;
using LoanShark.Domain;
using LoanShark.API.Proxies;

namespace LoanShark.ViewModel.BankViewModel
{
    /// <summary>
    /// ViewModel for the bank account deletion confirmation view
    /// </summary>
    public class BankAccountDeleteViewModel
    {
        /// <summary>
        /// Command for the No button to cancel deletion
        /// </summary>
        public ICommand NoCommand { get; }

        /// <summary>
        /// Command for the Yes button to proceed with deletion
        /// </summary>
        public ICommand YesCommand { get; }

        /// <summary>
        /// Action to be invoked when the view should be closed
        /// </summary>
        public Action? OnClose { get; set; }

        private IBankAccountService service;
        private string iban;

        /// <summary>
        /// Initializes a new instance of the BankAccountDeleteViewModel class
        /// </summary>
        public BankAccountDeleteViewModel(IBankAccountService s)
        {
            var bankAccService = new BankAccountServiceProxy(new System.Net.Http.HttpClient());
            service = s;
            iban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;
            NoCommand = new RelayCommand(OnNoButtonClicked);
            YesCommand = new RelayCommand(OnYesButtonClicked);
        }

        /// <summary>
        /// Handler for the No button click
        /// Cancels the deletion and closes the view
        /// </summary>
        public void OnNoButtonClicked()
        {
            Debug.WriteLine("Back button");
            WindowManager.ShouldReloadBankAccounts = false;
            OnClose?.Invoke();
        }

        /// <summary>
        /// Handler for the Yes button click
        /// Opens the verification view to proceed with deletion
        /// </summary>
        public void OnYesButtonClicked()
        {
            Debug.WriteLine("Yes button");
            View.BankView.BankAccountVerifyView window = new View.BankView.BankAccountVerifyView();
            window.Activate();
            WindowManager.ShouldReloadBankAccounts = false;
            OnClose?.Invoke();
        }
    }
}
