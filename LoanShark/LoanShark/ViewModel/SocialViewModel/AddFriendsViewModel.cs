// <copyright file="AddFriendsViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.CodeAnalysis.CSharp;

namespace LoanShark.ViewModel.SocialViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;
    using LoanShark.API.Proxies;

    public class AddFriendsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<User> UsersList { get; set; }

        private FriendsListViewModel friendsListViewModel;

        private string searchQuery;

        public List<User> AllUsers { get; set; }

        public ISocialUserServiceProxy UserService { get; set; }

        public ICommand AddFriendCommand { get; set; }

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.FilterUsers();
                }
            }
        }

        public AddFriendsViewModel(FriendsListViewModel friendsListViewModel, ISocialUserServiceProxy userService)
        {
            this.UserService = userService;
            this.friendsListViewModel = friendsListViewModel;
            this.LoadAllUsers();
            this.UsersList = new ObservableCollection<User>();
            this.AddFriendCommand = new RelayCommand<object>(AddFriend);

            this.LoadUsers();
        }

        public async void LoadAllUsers()
        {
            var currentUser = await (UserService.GetCurrentUser());
            this.AllUsers = await (UserService.GetNonFriendsUsers(currentUser));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void AddFriend(object user)
        {
            var friend = user as User;

            var currentUser = await this.UserService.GetCurrentUser();
            this.UserService.AddFriend(currentUser, friend!.GetUserId());
            this.friendsListViewModel.LoadFriends();
            this.LoadUsers();
        }

        private async void LoadUsers()
        {
            var currentUser = await(UserService.GetCurrentUser());
            this.AllUsers = await(UserService.GetNonFriendsUsers(currentUser));
            this.FilterUsers();
        }

        private void FilterUsers()
        {
            this.UsersList.Clear();

            foreach (var friend in this.AllUsers.Where(f =>
                         string.IsNullOrEmpty(this.SearchQuery) ||
                         f.Username.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                this.UsersList.Add(friend);
            }
        }
    }
}
