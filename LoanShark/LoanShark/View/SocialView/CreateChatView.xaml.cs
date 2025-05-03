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
        public IChatService chatService;
        public IUserService userService;

        public CreateChatView(ChatListViewModel chatListViewModel, IChatService chatService, IUserService userService)
        {
            this.InitializeComponent();
            this.chatService = chatService;
            this.userService = userService;

            this.MainGrid.DataContext = new CreateChatViewModel(chatListViewModel, chatService, userService);
        }
    }
}
