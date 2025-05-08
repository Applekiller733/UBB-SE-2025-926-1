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

        public IUserServiceProxy UserService { get; set; }

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

        public FriendsListViewModel(IChatServiceProxy chat, IUserServiceProxy user, IMessageServiceProxy message)
        {
            this.UserService = user;
            this.ChatService = chat;
            this.MessageService = message;
            this.AllFriends = this.UserService.GetFriendsByUser(this.UserService.GetCurrentUser());
            this.FriendsList = new ObservableCollection<User>();
            this.RemoveFriend = new RelayCommand<object>(this.RemoveFriendFromList);

            this.LoadFriends();
        }

        public void RemoveFriendFromList(object user)
        {
            var friend = user as User;
            if (friend != null)
            {
                this.UserService.RemoveFriend(this.UserService.GetCurrentUser(), friend.GetUserId());
            }

            this.AllFriends = this.UserService.GetFriendsByUser(this.UserService.GetCurrentUser());

            this.LoadFriends();
        }

        public void LoadFriends()
        {
            this.AllFriends = this.UserService.GetFriendsByUser(this.UserService.GetCurrentUser());
            this.FilterFriends();
        }

        public void FilterFriends()
        {
            this.FriendsList.Clear();

            foreach (var friend in this.AllFriends.Where(f =>
                         string.IsNullOrEmpty(this.SearchQuery) ||
                         f.Username.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                         f.PhoneNumber.ToString().Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                this.FriendsList.Add(friend);
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