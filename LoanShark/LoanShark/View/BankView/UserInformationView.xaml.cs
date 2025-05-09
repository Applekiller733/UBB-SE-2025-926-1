using LoanShark.ViewModel.BankViewModel;
using Microsoft.UI.Xaml;
using LoanShark.Helper;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark.View.BankView
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class UserInformationView : Window
    {
        public UserInformationView()
        {
            this.InitializeComponent();
            var viewModel = App.Services.GetRequiredService<UserInformationViewModel>();
            viewModel.CloseAction = () => this.Close();
            MainPanel.DataContext = viewModel;
            WindowManager.RegisterWindow(this);
        }
    }
}
