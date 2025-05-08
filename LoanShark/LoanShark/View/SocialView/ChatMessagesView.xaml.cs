// <copyright file="ChatMessagesView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class ChatMessagesView : Page
    {
        public int SelectedChat { get; set; }

        private ChatMessagesViewModel chatMessagesViewModel;
        private Frame rightFrame;
        private ISocialUserServiceProxy userService;
        private IChatServiceProxy chatService;
        private IReportServiceProxy reportService;
        private ChatListViewModel chatListViewModel;
        private GenerateTransferViewModel generateTransferViewModel;

        public ChatMessagesView(ChatListViewModel chatListViewModel, Window mainWindow, Frame rightFrame, int chatID, IUserService userService, IChatService chatService, IMessageService messageService, IReportService reportService)
        {
            this.InitializeComponent();
            this.SelectedChat = chatID;
            this.chatListViewModel = chatListViewModel;
            this.userService = userService;
            this.chatService = chatService;
            this.reportService = reportService;
            this.rightFrame = rightFrame;
            this.chatMessagesViewModel = new ChatMessagesViewModel(mainWindow, rightFrame, chatID, messageService, chatService, userService, reportService);
            this.generateTransferViewModel = new GenerateTransferViewModel(chatService, chatID);
            this.chatMessagesViewModel.ChatListView = this.ChatListView;
            this.chatMessagesViewModel.SetupMessageTracking();

            this.DataContext = this.chatMessagesViewModel;
        }

        public void AddNewMember_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = new AddNewMemberView(this.chatMessagesViewModel, this, this.rightFrame, this.SelectedChat, this.chatService, this.userService);
        }

        public void LeaveChat_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = new LeaveChatView(this.SelectedChat, this.chatListViewModel, this, this.rightFrame, this.chatService, this.userService);
        }

        public void SendTransfer_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = new GenerateTransferView(this.generateTransferViewModel, this, this.rightFrame, this.SelectedChat, this.chatService);
        }
    }
}