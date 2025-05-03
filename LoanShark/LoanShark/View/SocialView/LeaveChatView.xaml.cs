// <copyright file="LeaveChatView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class LeaveChatView : Page
    {
        private IChatService chatService;
        private IUserService userService;
        private Frame RightFrame;
        private LeaveChatViewModel leaveChatViewModel;
        private Page lastPage;
        private ChatListViewModel chatMessagesViewModel;

        public LeaveChatView(int ChatID, ChatListViewModel chVm, Page chatM, Frame right, IChatService chat, IUserService user)
        {
            this.InitializeComponent();
            this.chatMessagesViewModel = chVm;
            this.lastPage = chatM;
            this.RightFrame = right;
            this.userService = user;
            this.chatService = chat;

            this.leaveChatViewModel = new LeaveChatViewModel(this.userService, this.chatService, this.chatMessagesViewModel, ChatID);
            this.DataContext = this.leaveChatViewModel;
        }

        public void LeaveChat_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = null;
        }

        public void CancelLeaving_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = this.lastPage;
        }
    }
}
