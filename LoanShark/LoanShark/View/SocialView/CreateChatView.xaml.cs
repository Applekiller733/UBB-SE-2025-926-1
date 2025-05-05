// <copyright file="CreateChatView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoanShark.View.SocialView
{
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.ViewModel.SocialViewModel;

    public sealed partial class CreateChatView : Page
    {
        public IChatService ChatService;
        public IUserService UserService;

        public CreateChatView(ChatListViewModel chatListViewModel, IChatService chatService, IUserService userService)
        {
            this.InitializeComponent();
            this.ChatService = chatService;
            this.UserService = userService;

            this.MainGrid.DataContext = new CreateChatViewModel(chatListViewModel, chatService, userService);
        }
    }
}
