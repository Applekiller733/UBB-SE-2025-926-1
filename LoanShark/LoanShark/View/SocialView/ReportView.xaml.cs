// <copyright file="ReportView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoanShark.View.SocialView
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Implementations;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;
    using LoanShark.API.Proxies;

    public sealed partial class ReportView : Window
    {
        public ReportViewModel ViewModel { get; }

        public ReportView(ISocialUserServiceProxy userService, IReportServiceProxy reportService, int reportedUserId, int messageId)
        {
            this.InitializeComponent();

            ViewModel = new ReportViewModel(userService, reportService, reportedUserId, messageId);
            ViewModel.ShowErrorDialog += OnShowErrorDialog;
            ViewModel.ShowSuccessDialog += OnShowSuccessDialog;
            ViewModel.CloseView += OnCloseView;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsOtherCategorySelected))
            {
                UpdateOtherReasonVisibility();
            }
        }

        private void UpdateOtherReasonVisibility()
        {
            if (ViewModel.IsOtherCategorySelected)
            {
                OtherReasonLabel.Visibility = Visibility.Visible;
                OtherReasonTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                OtherReasonLabel.Visibility = Visibility.Collapsed;
                OtherReasonTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private async void OnShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot,
            };
            await dialog.ShowAsync();
        }

        private async void OnShowSuccessDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot,
            };
            await dialog.ShowAsync();
            this.Close();
        }

        private void OnCloseView()
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // reportcommand
            this.ViewModel.SubmitCommand.Execute(null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.CancelCommand.Execute(null);
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                // Get the string content from the ComboBoxItem
                this.ViewModel.SelectedCategory = selectedItem.Content.ToString();
            }
        }

        private void OtherReasonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ViewModel.OtherReason = OtherReasonTextBox.Text;
        }
    }
}