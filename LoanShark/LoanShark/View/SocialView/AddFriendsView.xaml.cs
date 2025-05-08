// <copyright file="AddFriendsView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;
    using LoanShark.API.Proxies;

    public sealed partial class AddFriendsView : Page
    {
        private ISocialUserServiceProxy userService;
        private FriendsListViewModel friendsListViewModel;
        private AddFriendsViewModel addFriendsViewModel;

        public AddFriendsView(FriendsListViewModel friendsListViewModel, ISocialUserServiceProxy userService)
        {
            this.InitializeComponent();

            this.friendsListViewModel = friendsListViewModel;
            this.userService = userService;
            this.addFriendsViewModel = new AddFriendsViewModel(friendsListViewModel, userService);

            this.DataContext = this.addFriendsViewModel;
        }
    }
}
