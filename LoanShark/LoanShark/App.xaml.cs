using System.Diagnostics;
using Microsoft.UI.Xaml;
using LoanShark.View.BankView;
using LoanShark.Data;
using LoanShark.API.Proxies;
using LoanShark.ViewModel.BankViewModel;
using Microsoft.Extensions.DependencyInjection;
using LoanShark.Service.BankService;
using System;

namespace LoanShark
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } 
        public App()
        {
            Debug.Print("Application is now opening...");
            this.InitializeComponent();
            ConfigureServices();
            DataLink.Instance.OpenConnection();
        }
        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddHttpClient<IUserService, UserServiceProxy>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7097/"); // adjust as needed
            });
            services.AddHttpClient<IBankAccountService, BankAccountServiceProxy>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7097/");
            });

            services.AddTransient<DeleteAccountViewModel>();
            services.AddTransient<UserInformationViewModel>();
            services.AddTransient<UserRegistrationViewModel>();
            services.AddTransient<BankAccountCreateViewModel>();
            services.AddTransient<BankAccountDeleteViewModel>();
            services.AddTransient<BankAccountDetailsViewModel>();
            services.AddTransient<BankAccountListViewModel>();
            services.AddTransient<BankAccountUpdateViewModel>();
            services.AddTransient<BankAccountVerifyViewModel>();





            Services = services.BuildServiceProvider();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            loginWindow = new LoginView();
            loginWindow.Activate();
        }

        private LoginView? loginWindow;
    }
}
