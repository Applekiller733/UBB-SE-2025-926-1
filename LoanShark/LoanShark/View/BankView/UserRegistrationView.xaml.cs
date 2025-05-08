using LoanShark.Helper;
using LoanShark.ViewModel.BankViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark.View.BankView
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserRegistrationView : Window
    {
        public UserRegistrationView()
        {
            this.InitializeComponent();
            var viewModel = App.Services.GetRequiredService<UserRegistrationViewModel>();
            viewModel.CloseAction = () =>
            {
                LoginView loginWindow = new LoginView();
                loginWindow.Activate();
                this.Close();
            };
            MainPanel.DataContext = viewModel;
            WindowManager.RegisterWindow(this);
        }
    }
}
