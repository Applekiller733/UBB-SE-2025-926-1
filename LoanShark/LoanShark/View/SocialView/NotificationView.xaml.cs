// <copyright file="NotificationView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.EF.Repository.SocialRepository;
    using LoanShark.Service.SocialService.Implementations;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class NotificationView : Page
    {
        private NotificationViewModel viewModel;

        public NotificationView(IRepository r, INotificationServiceProxy ns, ISocialUserServiceProxy us)
        {
            this.InitializeComponent();
            var repo = r;
            var notificationService = ns;
            var userService = us;
            int currentUserID = repo.GetLoggedInUserID();
            this.DataContext = new NotificationViewModel(notificationService, currentUserID);
        }
    }
}