// <copyright file="FriendsListView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FriendsListView : Page
    {
        private FriendsListViewModel friendsListViewModel;
        private IChatServiceProxy chatService;
        private ISocialUserServiceProxy userService;
        private IMessageServiceProxy messageService;
        private Frame rightFrame;
        private Page addFriendsPage;

        public FriendsListView(IChatServiceProxy chatService, ISocialUserServiceProxy userService, IMessageServiceProxy messageService, Frame rightFrame)
        {
            this.InitializeComponent();
            this.chatService = chatService;
            this.userService = userService;
            this.messageService = messageService;
            this.rightFrame = rightFrame;
            this.friendsListViewModel = new FriendsListViewModel(chatService, userService, messageService);

            this.DataContext = this.friendsListViewModel;
        }

        private void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = new AddFriendsView(this.friendsListViewModel, this.userService);
        }
    }
}