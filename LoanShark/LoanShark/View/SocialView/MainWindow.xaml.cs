// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using LoanShark.API.Proxies;
using LoanShark.View.SocialView;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml;
    using LoanShark.EF.Repository.SocialRepository;
    using LoanShark.Service.SocialService.Implementations;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.View.SocialView;
    using LoanShark.ViewModel.SocialViewModel;

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private Window mainWindow;
        private ISocialUserServiceProxy userService;
        private IChatServiceProxy chatService;
        private IMessageServiceProxy messageService;
        private IFeedServiceProxy feedService;
        private IReportServiceProxy reportService;
        private INotificationServiceProxy notificationService;
        private IRepository repo;

        //public MainWindow( IRepository r, ISocialUserServiceProxy us, IChatServiceProxy cs, IMessageServiceProxy ms, IFeedServiceProxy fs, IReportServiceProxy rs, INotificationServiceProxy ns)
        //{
        //    this.InitializeComponent();

        //    this.mainWindow = this;
        //    this.repo = r;
        //    this.notificationService = ns;
        //    this.userService = us;
        //    this.chatService = cs;
        //    this.messageService = ms;
        //    this.feedService = fs;
        //    this.reportService =rs;

        //    if (this.LeftFrame.Content == null || !(this.LeftFrame.Content is ChatListView))
        //    {
        //        var chatListView = new ChatListView(this, this.chatService, this.userService, this.reportService, this.messageService, this.RightFrame);
        //        this.LeftFrame.Content = chatListView;
        //    }

        //    if (this.RightFrame.Content == null || !(this.RightFrame.Content is FeedView))
        //    {
        //        var feedViewModel = new FeedViewModel(this.feedService);
        //        var feedView = new FeedView(feedViewModel, this.userService, this.feedService);
        //        this.RightFrame.Content = feedView;
        //    }
        //}

        public MainWindow(ISocialUserServiceProxy us, IChatServiceProxy cs, IMessageServiceProxy ms, IFeedServiceProxy fs, IReportServiceProxy rs, INotificationServiceProxy ns)
        {
            this.InitializeComponent();

            this.mainWindow = this;
            //this.repo = r;
            this.notificationService = ns;
            this.userService = us;
            this.chatService = cs;
            this.messageService = ms;
            this.feedService = fs;
            this.reportService = rs;

            if (this.LeftFrame.Content == null || !(this.LeftFrame.Content is ChatListView))
            {
                var chatListView = new ChatListView(this, this.chatService, this.userService, this.reportService, this.messageService, this.RightFrame);
                this.LeftFrame.Content = chatListView;
            }

            if (this.RightFrame.Content == null || !(this.RightFrame.Content is FeedView))
            {
                var feedViewModel = new FeedViewModel(this.feedService);
                var feedView = new FeedView(feedViewModel, this.userService, this.feedService);
                this.RightFrame.Content = feedView;
            }
        }

        private void Chat_Click(object sender, RoutedEventArgs e)
        {
            if (this.LeftFrame.Content == null || !(this.LeftFrame.Content is ChatListView))
            {
                var chatListView = new ChatListView(this, this.chatService, this.userService, this.reportService, this.messageService, this.RightFrame);
                this.LeftFrame.Content = chatListView;
            }
        }

        // private void Feed_Click(object sender, RoutedEventArgs e)
        // {
        //    RightFrame.Navigate(typeof(FeedView));
        // }
        private void Friends_Click(object sender, RoutedEventArgs e)
        {
            if (this.LeftFrame.Content == null || !(this.LeftFrame.Content is FriendsListView))
            {
                var friendsListView = new FriendsListView(this.chatService, this.userService, this.messageService, this.RightFrame);
                this.LeftFrame.Content = friendsListView;
            }
        }

        private void Feed_Click(object sender, RoutedEventArgs e)
        {
            if (this.RightFrame.Content == null || !(this.RightFrame.Content is FeedView))
            {
                var feedViewModel = new FeedViewModel(this.feedService);
                var feedView = new FeedView(feedViewModel, this.userService, this.feedService);
                this.RightFrame.Content = feedView;
            }
        }

        //private void Notifications_click(object sender, RoutedEventArgs e)
        //{
        //    if (this.RightFrame.Content == null || !(this.RightFrame.Content is NotificationView))
        //    {
        //        var notificationView = new NotificationView(repo, notificationService, userService);
        //        this.RightFrame.Content = notificationView;
        //    }
        //}
        private void Notifications_click(object sender, RoutedEventArgs e)
        {
            if (this.RightFrame.Content == null || !(this.RightFrame.Content is NotificationView))
            {
                var notificationView = new NotificationView(chatService, notificationService, userService);
                this.RightFrame.Content = notificationView;
            }
        }
    }
}
