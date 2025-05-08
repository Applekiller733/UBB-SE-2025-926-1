using System;
using LoanShark.Domain;
using LoanShark.Service.BankService;
using LoanShark.ViewModel.BankViewModel;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace LoanShark.View.BankView
{
    // this displays the details of a transaction, no biggie
    public sealed partial class TransactionDetailsView : Window
    {
        private Transaction transaction;
        private AppWindow appWindow;
        private ITransactionHistoryService service;

        public TransactionDetailsView(string transactionDetails, Transaction transaction, ITransactionHistoryService s)
        {
            this.InitializeComponent();
            TransactionDetailsTextBlock.Text = transactionDetails;
            this.transaction = transaction;
            this.service = s;

            // Get the AppWindow associated with this Window
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd));

            ResizeWindow(800, 600);
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            string newDescription = DescriptionTextBox.Text;
            if (!string.IsNullOrEmpty(newDescription))
            {
                transaction.TransactionDescription = newDescription;
                service.UpdateTransactionDescription(transaction.TransactionId, newDescription);
                TransactionDetailsTextBlock.Text = transaction.TostringDetailed();
            }

            DescriptionTextBox.Text = string.Empty;
        }

        public void ResizeWindow(int width, int height)
        {
            if (appWindow != null)
            {
                appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
            }
        }
    }
}