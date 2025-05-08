// <copyright file="NotificationViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.ViewModel.SocialViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;

    public class NotificationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ClearNotificationCommand { get; }

        public ICommand ClearAllNotificationsCommand { get; }

        private ObservableCollection<Notification> notifications;

        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return notifications;
            }

            set
            {
                this.notifications = value;
                this.OnPropertyChanged(nameof(this.Notifications));
            }
        }

        private readonly int currentUserID;

        private readonly INotificationServiceProxy notificationService;

        public NotificationViewModel(INotificationServiceProxy service, int userID)
        {
            this.notificationService = service;
            this.currentUserID = userID;
            this.Notifications = new ObservableCollection<Notification>();
            this.ClearNotificationCommand = new RelayCommand<int>(this.ClearNotification);
            this.ClearAllNotificationsCommand = new RelayCommand(this.ClearAllNotifications);
            this.LoadNotifications();
        }

        public void LoadNotifications()
        {
            var notificationsList = this.notificationService.GetNotifications(this.currentUserID);
            this.Notifications = new ObservableCollection<Notification>(notificationsList.Result);
        }

        public void ClearNotification(int notificationID)
        {
            this.notificationService.ClearNotification(notificationID);
            this.LoadNotifications();
        }

        public void ClearAllNotifications()
        {
            this.notificationService.ClearAllNotifications(this.currentUserID);
            this.LoadNotifications();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}