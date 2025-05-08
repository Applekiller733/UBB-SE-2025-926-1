// <copyright file="LeaveChatView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class LeaveChatView : Page
    {
        private IChatServiceProxy chatService;
        private ISocialUserServiceProxy userService;
        private Frame rightFrame;
        private LeaveChatViewModel leaveChatViewModel;
        private Page lastPage;
        private ChatListViewModel chatMessagesViewModel;

        public LeaveChatView(int chatID, ChatListViewModel chVm, Page chatM, Frame right, IChatServiceProxy chat, ISocialUserServiceProxy user)
        {
            this.InitializeComponent();
            this.chatMessagesViewModel = chVm;
            this.lastPage = chatM;
            this.rightFrame = right;
            this.userService = user;
            this.chatService = chat;

            this.leaveChatViewModel = new LeaveChatViewModel(this.userService, this.chatService, this.chatMessagesViewModel, chatID);
            this.DataContext = this.leaveChatViewModel;
        }

        public void LeaveChat_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = null;
        }

        public void CancelLeaving_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = this.lastPage;
        }
    }
}
