using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Helper;
using LoanShark.Service.BankService;
using LoanShark.Service.SocialService.Implementations;
using LoanShark.View.SocialView;
using LoanShark.ViewModel.BankViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark.View.BankView
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageView : Window
    {
        public event EventHandler? LogOut;

        public MainPageViewModel ViewModel { get; private set; }

        private ISocialUserServiceProxy socialUserService;
        private IChatServiceProxy socialChatService;
        private IMessageServiceProxy socialMessageService;
        private IFeedServiceProxy socialFeedService;
        private IReportServiceProxy socialReportService;
        private INotificationServiceProxy socialNotificationService;
        private IRepository socialRepo;

        public MainPageView(ISocialUserServiceProxy socialUserService,
        IChatServiceProxy socialChatService,
        IMessageServiceProxy socialMessageService, IFeedServiceProxy socialFeedService,
        IReportServiceProxy socialReportService,
        INotificationServiceProxy socialNotificationService,
        IRepository socialRepo)
        {
            this.InitializeComponent();
            this.ViewModel = App.Services.GetRequiredService<MainPageViewModel>();

            this.socialUserService = socialUserService;
            this.socialChatService = socialChatService;
            this.socialMessageService = socialMessageService;
            this.socialFeedService = socialFeedService;
            this.socialReportService = socialReportService;
            this.socialNotificationService = socialNotificationService;
            this.socialRepo = socialRepo;
            // Register this window with the WindowManager
            WindowManager.RegisterWindow(this);

            // Set the welcome text from ViewModel
            centeredTextField.Text = this.ViewModel.WelcomeText;
        }

        public MainPageView()
        {
            this.InitializeComponent();
            this.ViewModel = App.Services.GetRequiredService<MainPageViewModel>();

            // Register this window with the WindowManager
            WindowManager.RegisterWindow(this);

            // Set the welcome text from ViewModel
            centeredTextField.Text = this.ViewModel.WelcomeText;
        }

        public async void CheckBalanceButtonHandler(object sender, RoutedEventArgs e)
        {
            await this.ViewModel.CheckBalanceButtonHandler();
        }

        public async void LoanButtonHandler(object sender, RoutedEventArgs e)
        {
            var errorMessage = await this.ViewModel.LoanButtonHandler();
            if (errorMessage != null)
            {
                await this.ShowDialog(errorMessage);
            }
        }

        private async void TransactionButtonHandler(object sender, RoutedEventArgs e)
        {
            var errorMessage = await this.ViewModel.TransactionButtonHandler();
            if (errorMessage != null)
            {
                await this.ShowDialog(errorMessage);
            }
        }

        private async void TransactionHistoryButtonHandler(object sender, RoutedEventArgs e)
        {
            var errorMessage = await this.ViewModel.TransactionHistoryButtonHandler();
            if (errorMessage != null)
            {
                await this.ShowDialog(errorMessage);
            }
        }

        private void AccountsFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ViewModel.ResetBalanceButtonContent();
            var flipView = sender as FlipView;
            if (flipView?.SelectedItem is BankAccount selectedAccount)
            {
                // Optional: You can handle the selection change here
                Debug.Print($"Selected account: {selectedAccount.Name}, IBAN: {selectedAccount.Iban}");

                try
                {
                    UserSession.Instance.SetUserData("current_bank_account_iban", selectedAccount.Iban);
                }
                catch (Exception ex)
                {
                    Debug.Print($"Error setting current bank account: {ex.Message}");
                }
            }
        }

        private void AccountSettingsButtonHandler(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AccountSettingsButtonHandler();
        }

        private void LogOutButtonHandler(object sender, RoutedEventArgs e)
        {
            // Create the login window but don't invalidate the session yet
            LoginView logInWindow = new LoginView();
            logInWindow.Activate();

            // Close current window
            this.Close();
        }

        private void ExitLoanSharkButtonHandler(object sender, RoutedEventArgs e)
        {
            // Just close the window, let WindowManager handle cleanup
            this.Close();
        }

        private void BankAccountCreateButtonHandler(object sender, RoutedEventArgs e)
        {
            this.ViewModel.BankAccountCreateButtonHandler();
        }

        private async void BankAccountDetailsViewButtonHandler(object sender, RoutedEventArgs e)
        {
            var errorMessage = await this.ViewModel.BankAccountDetailsButtonHandler();
            if (errorMessage != null)
            {
                await this.ShowDialog(errorMessage);
            }
        }

        private async void BankAccountSettingsButtonHandler(object sender, RoutedEventArgs e)
        {
            var errorMessage = await this.ViewModel.BankAccountSettingsButtonHandler();
            if (errorMessage != null)
            {
                await this.ShowDialog(errorMessage);
            }
        }

        public async Task RefreshBankAccounts()
        {
            await this.ViewModel.RefreshBankAccounts();
        }

        private async Task ShowDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private void GoToSocialButton_OnClick(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();
            MainWindow socialWindow = new MainWindow(
                new SocialUserServiceProxy(new System.Net.Http.HttpClient()),
                new ChatServiceProxy(new System.Net.Http.HttpClient()), 
                new MessageServiceProxy(new System.Net.Http.HttpClient()), 
                new FeedServiceProxy(new System.Net.Http.HttpClient()),
                new ReportServiceProxy(new System.Net.Http.HttpClient()), 
                new NotificationServiceProxy(new System.Net.Http.HttpClient())
            );

            socialWindow.Activate();

            //this.Close();
        }
    }
}
