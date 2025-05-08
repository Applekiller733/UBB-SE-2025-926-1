using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LoanShark.Service.BankService;
using LoanShark.Domain;
using LoanShark.API.Proxies;

namespace LoanShark.ViewModel.BankViewModel
{
    public class CurrencyExchangeTableViewModel : ObservableObject
    {
        private readonly ITransactionsService transactionService;

        public ObservableCollection<CurrencyExchange> ExchangeRates { get; } = new ObservableCollection<CurrencyExchange>();

        public ICommand CloseCommand { get; }

        public Action CloseAction { get; set; }

        public CurrencyExchangeTableViewModel()
        {
            this.transactionService = new TransactionsServiceProxy(new System.Net.Http.HttpClient());

            LoadExchangeRatesAsync();

            CloseCommand = new RelayCommand(CloseWindow);
        }

        //public CurrencyExchangeTableViewModel(ITransactionsService transactionsService)
        //{
        //    this.transactionService = transactionService;
        //    LoadExchangeRatesAsync();

        //    CloseCommand = new RelayCommand(CloseWindow);
        //}

        private async void LoadExchangeRatesAsync()
        {
            var rates = await this.transactionService.GetAllCurrencyExchangeRates();

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
