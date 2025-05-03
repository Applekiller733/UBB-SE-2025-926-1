using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LoanShark.Service.BankService;
using LoanShark.Domain;

namespace LoanShark.ViewModel.BankViewModel
{
    public class CurrencyExchangeTableViewModel : ObservableObject
    {
        private readonly TransactionsService transactionService;

        public ObservableCollection<CurrencyExchange> ExchangeRates { get; } = new ObservableCollection<CurrencyExchange>();

        public ICommand CloseCommand { get; }

        public Action CloseAction { get; set; }

        public CurrencyExchangeTableViewModel()
        {
            transactionService = new TransactionsService();
            LoadExchangeRatesAsync();

            CloseCommand = new RelayCommand(CloseWindow);
        }

        private async void LoadExchangeRatesAsync()
        {
            var rates = await transactionService.GetAllCurrencyExchangeRates();

            ExchangeRates.Clear();
            foreach (var rate in rates)
            {
                ExchangeRates.Add(rate);
            }
        }

        private void CloseWindow()
        {
            CloseAction?.Invoke();
        }
    }
}
