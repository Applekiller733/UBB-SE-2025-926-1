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
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using LoanShark.Service.SocialService.Interfaces;

    public sealed partial class NotificationView : Page
    {
        private NotificationViewModel viewModel;

        //public NotificationView(IRepository r, INotificationServiceProxy ns, ISocialUserServiceProxy us)
        //{
        //    this.InitializeComponent();
        //    var repo = r;
        //    var notificationService = ns;
        //    var userService = us;
        //    int currentUserID = repo.GetLoggedInUserID();
        //    this.DataContext = new NotificationViewModel(notificationService, currentUserID);
        //}

        public NotificationView(IChatServiceProxy chatServiceProxy, INotificationServiceProxy ns, ISocialUserServiceProxy us)
        {
            this.InitializeComponent();
            //var repo = r;
            var notificationService = ns;
            var userService = us;
            LoadComponent();
        }

        private async void LoadComponent()
        {
            var notificationService = new NotificationServiceProxy(new System.Net.Http.HttpClient());
            var userService = new SocialUserServiceProxy(new System.Net.Http.HttpClient());
            var chatServiceProxy = new ChatServiceProxy(new System.Net.Http.HttpClient());
            int currentUserID = await chatServiceProxy.GetCurrentUserID();
            this.DataContext = new NotificationViewModel(notificationService, currentUserID);
        }

    }
}