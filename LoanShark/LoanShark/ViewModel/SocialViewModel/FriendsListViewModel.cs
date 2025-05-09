// <copyright file="FriendsListViewModel.cs" company="PlaceholderCompany">
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
    using System.Windows.Input;
    using Microsoft.UI.Xaml;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;

    public class FriendsListViewModel : INotifyPropertyChanged
    {
        public List<User> AllFriends { get; set; }

        public ObservableCollection<User> FriendsList { get; set; }

        public ISocialUserServiceProxy UserService { get; set; }

        public IChatServiceProxy ChatService { get; set; }

        public IMessageServiceProxy MessageService { get; set; }

        public ICommand RemoveFriend { get; }

        private Visibility noFriendsVisibility = Visibility.Collapsed;

        public Visibility NoFriendsVisibility
        {
            get
            {
                return this.noFriendsVisibility;
            }

            set
            {
                this.noFriendsVisibility = value;
                this.OnPropertyChanged(nameof(this.noFriendsVisibility));
            }
        }

        private string searchQuery;

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.FilterFriends();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FriendsListViewModel(IChatServiceProxy chat, ISocialUserServiceProxy user, IMessageServiceProxy message)
        {
            this.UserService = user;
            this.ChatService = chat;
            this.MessageService = message;
            LoadAllFriends();
            this.FriendsList = new ObservableCollection<User>();
            this.RemoveFriend = new RelayCommand<object>(this.RemoveFriendFromList);

            this.LoadFriends();
        }

        public async void LoadAllFriends()
        {
            var currentUser = await this.UserService.GetCurrentUser();
            this.AllFriends = await this.UserService.GetFriendsByUser(currentUser);
        }

        public async void RemoveFriendFromList(object user)
        {
            var friend = user as User;
            var currentUser = await this.UserService.GetCurrentUser();
            if (friend != null)
            {
                this.UserService.RemoveFriend(currentUser, friend.GetUserId());
            }

            LoadAllFriends();

            this.LoadFriends();
        }

        public void LoadFriends()
        {
            LoadAllFriends();
            this.FilterFriends();
        }

        public void FilterFriends()
        {
            this.FriendsList.Clear();
            if (this.AllFriends != null)
            {
                foreach (var friend in this.AllFriends.Where(f =>
                             string.IsNullOrEmpty(this.SearchQuery) ||
                             f.Username.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                             f.PhoneNumber.ToString().Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase)))
                {
                    this.FriendsList.Add(friend);
                }
            }

            this.UpdatenoFriendsVisibility();
        }

        private void UpdatenoFriendsVisibility()
        {
            this.noFriendsVisibility = (this.FriendsList == null || this.FriendsList.Count == 0)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}