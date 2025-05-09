// <copyright file="CreateChatView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class CreateChatView : Page
    {
        public IChatServiceProxy ChatService;
        public ISocialUserServiceProxy UserService;

        public CreateChatView(ChatListViewModel chatListViewModel, IChatServiceProxy chatService, ISocialUserServiceProxy userService)
        {
            this.InitializeComponent();
            this.ChatService = chatService;
            this.UserService = userService;

            this.MainGrid.DataContext = new CreateChatViewModel(chatListViewModel, chatService, userService);
        }
    }
}
