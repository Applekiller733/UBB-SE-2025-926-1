// <copyright file="ChatListViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;
using LoanShark.View.SocialView;

namespace LoanShark.ViewModel.SocialViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.View.SocialView;

    public class ChatListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string searchQuery = string.Empty;

        public ObservableCollection<Chat> ChatList { get; set; }

        public List<Chat> CurrentUserChats;
        public IChatServiceProxy ChatService;
        public ISocialUserServiceProxy UserService;

        public CountToVisibilityConverter CountToVisibilityConverter { get; set; }

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.FilterChats();
                }
            }
        }

        public ChatListViewModel(IChatServiceProxy chatS, ISocialUserServiceProxy userS)
        {
            this.ChatList = new ObservableCollection<Chat>();
            this.ChatService = chatS;
            this.UserService = userS;
            this.CountToVisibilityConverter = new CountToVisibilityConverter();

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadCurrentUserChats();
            await LoadChats();
        }

        public async Task LoadCurrentUserChats()
        {
            this.CurrentUserChats = await this.UserService.GetCurrentUserChats();
        }

        public async Task LoadChats()
        {
            await FilterChats();
        }

        public async Task FilterChats()
        {
            this.ChatList.Clear();
            await LoadCurrentUserChats();
            foreach (var chat in this.CurrentUserChats)
            {
                if (string.IsNullOrEmpty(this.SearchQuery) ||
                    chat.getChatName().IndexOf(this.SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    this.ChatList.Add(chat);
                }
            }

            // sort chats by last message time
            this.ChatList = new ObservableCollection<Chat>(this.ChatList.OrderByDescending(async chat => await this.ChatService.GetLastMessageTimeStamp(chat.getChatID())));
        }
    }
}
