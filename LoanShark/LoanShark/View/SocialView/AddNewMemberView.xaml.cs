// <copyright file="AddNewMemberView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class AddNewMemberView : Page
    {
        private IChatServiceProxy chatService;
        private ISocialUserServiceProxy userService;
        private AddNewMemberViewModel addNewMemberViewModel;
        private Page lastChat;
        private Frame rightFrame;
        private ChatMessagesViewModel chatMessagesViewModel;

        public AddNewMemberView(ChatMessagesViewModel chatMessagesViewModel, Page lastChat, Frame rightFrame, int chatID, IChatServiceProxy chatService, ISocialUserServiceProxy userService)
        {
            this.InitializeComponent();
            this.chatMessagesViewModel = chatMessagesViewModel;
            this.lastChat = lastChat;
            this.rightFrame = rightFrame;
            this.chatService = chatService;
            this.userService = userService;
            this.addNewMemberViewModel = new AddNewMemberViewModel(chatMessagesViewModel, lastChat, chatID, chatService, userService);

            this.DataContext = this.addNewMemberViewModel;
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = this.lastChat;
        }
    }
}
