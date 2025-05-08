using LoanShark.Helper;
using LoanShark.ViewModel.BankViewModel;
using Microsoft.UI.Xaml;
using LoanShark.Domain;
using LoanShark.Service.BankService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace LoanShark.View.BankView
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BankAccountDeleteView : Window
    {
        public BankAccountDeleteView(IBankAccountService s)
        {
            this.InitializeComponent();
            var viewModel = new BankAccountDeleteViewModel(s);
            MainGrid.DataContext = viewModel;

            viewModel.OnClose = () => this.Close();

            WindowManager.RegisterWindow(this);
        }
    }
}
