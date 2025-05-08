// <copyright file="FeedView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class FeedView : Page
    {
        private FeedViewModel feedViewModel;
        private ISocialUserServiceProxy userService;
        private IFeedServiceProxy feedService;

        public FeedView(FeedViewModel feedViewModel, ISocialUserServiceProxy userService, IFeedServiceProxy feedService)
        {
            this.InitializeComponent();
            this.userService = userService;
            this.feedService = feedService;
            this.feedViewModel = feedViewModel;
            this.DataContext = feedViewModel;
        }
    }
}
