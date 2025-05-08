// <copyright file="ChatListView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using LoanShark.API.Proxies;
using Microsoft.Identity.Client;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatListView : Page
    {
        private ChatListViewModel chatListViewModel;
        public ISocialUserServiceProxy UserService;
        public IChatServiceProxy ChatService;
        public IMessageServiceProxy MessageService;
        public IReportServiceProxy ReportService;
        public Frame RightFrame;
        public Window MainFrame;

        public ChatListView(Window mainFrame, IChatServiceProxy chatService, ISocialUserServiceProxy userService, IReportServiceProxy reportService, IMessageServiceProxy messageService, Frame rightFrame)
        {
            this.InitializeComponent();

            this.MainFrame = mainFrame;
            this.UserService = userService;
            this.ChatService = chatService;
            this.MessageService = messageService;
            this.ReportService = reportService;
            this.RightFrame = rightFrame;
            this.chatListViewModel = new ChatListViewModel(ChatService, UserService);
            this.MainGrid.DataContext = this.chatListViewModel;
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = new CreateChatView(this.chatListViewModel, this.ChatService, this.UserService);
        }

        private void ChatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ChatList.SelectedItem is Chat selectedChat)
            {
                this.RightFrame.Content = new ChatMessagesView(this.chatListViewModel, this.MainFrame, this.RightFrame, selectedChat.getChatID(), this.UserService, this.ChatService, this.MessageService, this.ReportService);
            }
        }
    }
}
