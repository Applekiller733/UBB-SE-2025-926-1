﻿// <copyright file="AddNewMemberViewModel.cs" company="PlaceholderCompany">
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
    using Microsoft.UI.Xaml.Controls;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;

    public class AddNewMemberViewModel : INotifyPropertyChanged
    {
        private List<User> allUnaddedFriends;
        private ISocialUserServiceProxy userService;
        private IChatServiceProxy chatService;
        private Page lastChat;
        private string searchQuery;
        private int chatID;
        private ChatMessagesViewModel chatMessagesViewModel;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<User> UnaddedFriends { get; set; }

        public ObservableCollection<User> CurrentChatMembers { get; set; }

        public ObservableCollection<User> NewlyAddedFriends { get; set; }

        public string ChatName { get; set; }

        public ICommand AddToSelectedCommand { get; set; }

        public ICommand RemoveFromSelectedCommand { get; set; }

        public ICommand AddUsersToChatCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddNewMemberViewModel(ChatMessagesViewModel chatMessagesViewModel, Page lastChat, int chatID, IChatServiceProxy chat, ISocialUserServiceProxy user)
        {
            this.chatMessagesViewModel = chatMessagesViewModel;
            this.chatID = chatID;
            this.lastChat = lastChat;
            this.userService = user;
            this.chatService = chat;
            this.LoadChatName();

            this.UnaddedFriends = new ObservableCollection<User>();
            this.CurrentChatMembers = new ObservableCollection<User>();
            this.NewlyAddedFriends = new ObservableCollection<User>();

            this.AddToSelectedCommand = new RelayCommand<User>(this.AddToSelected);
            this.RemoveFromSelectedCommand = new RelayCommand<User>(this.RemoveFromSelected);
            this.AddUsersToChatCommand = new RelayCommand(this.AddUsersToChat);

            this.UpdateObservableLists();
        }

        public async void LoadChatName()
        {
            this.ChatName = await this.chatService.GetChatNameByID(chatID);
        }

        public async void AddUsersToChat()
        {
            foreach (User user in this.NewlyAddedFriends)
            {
                this.chatService.AddUserToChat(user.GetUserId(), this.chatID);
            }

            this.NewlyAddedFriends.Clear();
            this.UpdateObservableLists();

            this.chatMessagesViewModel.CurrentChatParticipants = await this.chatService.GetChatParticipantsStringList(chatID);
        }

        public void AddToSelected(User user)
        {
            this.NewlyAddedFriends.Add(user);
            this.UnaddedFriends.Remove(user);
        }

        public void RemoveFromSelected(User user)
        {
            this.NewlyAddedFriends.Remove(user);
            this.UnaddedFriends.Add(user);
        }

        public string SearchQuery
        {
            get
            {
                return this.searchQuery;
            }

            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.UpdateFilteredFriends();
                }
            }
        }

        public async void LoadAllUnaddedFriendsList()
        {
            var currentUser = await this.userService.GetCurrentUser();
            var allFriends = await this.userService.GetFriendsByUser(currentUser);
            var currentChatParticipants = await this.chatService.GetChatParticipantsList(this.chatID);
            this.allUnaddedFriends = allFriends.Where(friend => !currentChatParticipants.Any(participant => participant.GetUserId() == friend.GetUserId())).ToList();
        }

        public async void UpdateObservableLists()
        {
            this.LoadAllUnaddedFriendsList();

            this.CurrentChatMembers.Clear();
            var chatParticipantList = await this.chatService.GetChatParticipantsList(this.chatID);
            foreach (var participant in chatParticipantList)
            {
                this.CurrentChatMembers.Add(participant);
            }

            this.UpdateFilteredFriends();
        }

        public void UpdateFilteredFriends()
        {
            this.UnaddedFriends.Clear();
            foreach (var friend in this.allUnaddedFriends.Where(f =>
                 string.IsNullOrEmpty(this.SearchQuery) ||
                 f.GetUsername()?.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase) == true ||
                 f.GetPhoneNumber()?.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase) == true))
            {
                this.UnaddedFriends.Add(friend);
            }
        }
    }
}