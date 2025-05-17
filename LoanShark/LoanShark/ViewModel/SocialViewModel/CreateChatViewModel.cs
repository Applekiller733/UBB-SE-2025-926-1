// <copyright file="CreateChatViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.ViewModel.SocialViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;
    using Microsoft.UI.Xaml;

    public class CreateChatViewModel : INotifyPropertyChanged
    {
        private string groupName;
        private string searchQuery;
        private ISocialUserServiceProxy userService;
        private IChatServiceProxy chatService;
        private ChatListViewModel chatListViewModel;

        public ICommand AddToSelectedList { get; }

        public ICommand CreateGroupChat { get; }

        public ObservableCollection<User> Friends { get; set; }

        private List<User> allFriends;

        public ObservableCollection<User> SelectedFriends { get; set; }

        public string GroupName
        {
            get => this.groupName;
            set
            {
                if (this.groupName != value)
                {
                    this.groupName = value;
                    this.OnPropertyChanged(nameof(this.GroupName));
                }
            }
        }

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.searchQuery));
                    this.FilterFriends();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateChatViewModel(ChatListViewModel chatListViewModel, IChatServiceProxy chatService, ISocialUserServiceProxy userService)
        {
            this.chatListViewModel = chatListViewModel;
            this.AddToSelectedList = new RelayCommand<object>(this.AddFriendToSelectedList);
            this.CreateGroupChat = new RelayCommand(this.AddNewGroupChat);
            this.Friends = new ObservableCollection<User>();
            this.SelectedFriends = new ObservableCollection<User>();
            this.chatService = chatService;
            this.userService = userService;
            LoadAllFriends();

            //this.LoadFriends();
        }

        public async Task LoadAllFriends()
        {
            var currentUser = await this.userService.GetCurrentUser();
            this.allFriends = await this.userService.GetFriendsByUser(currentUser);
            this.Friends.Clear();

            foreach (var friend in allFriends)
            {
                Friends.Add(friend);
            }

        }

        private async void AddNewGroupChat()
        {
            List<int> selectedFriendsIDs = new List<int>();
            var currentUser = await this.userService.GetCurrentUser();
            selectedFriendsIDs.Add(currentUser);
            foreach (User friend in this.SelectedFriends)
            {
                selectedFriendsIDs.Add(friend.GetUserId());
            }

            await this.chatService.CreateChat(selectedFriendsIDs, this.GroupName);
            this.chatListViewModel.LoadChats();
        }

        private void AddFriendToSelectedList(object parameter)
        {
            var friend = parameter as User;
            this.SelectedFriends = this.Friends;
            if (friend != null && !this.SelectedFriends.Contains(friend))
            {
                this.SelectedFriends.Add(friend);
                this.FilterFriends();
            }
        }

        private void LoadFriends()
        {
            this.FilterFriends();
        }

        private void FilterFriends()
        {
            Friends.Clear();

            var filteredFriends = this.allFriends
                ?.Where(f => (string.IsNullOrEmpty(SearchQuery) ||
                             (f.Username?.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ?? false)) &&
                             !SelectedFriends.Contains(f))
                .ToList() ?? new List<User>();

            foreach (var friend in filteredFriends)
            {
                Friends.Add(friend);
            }
        }

    }
}
