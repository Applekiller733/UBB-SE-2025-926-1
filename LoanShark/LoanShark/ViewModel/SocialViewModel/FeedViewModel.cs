// <copyright file="FeedViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LoanShark.API.Proxies;

namespace LoanShark.ViewModel.SocialViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;

    public class FeedViewModel : INotifyPropertyChanged
    {
        private readonly IFeedServiceProxy feedService;
        private ObservableCollection<Post> posts;

        public ObservableCollection<Post> Posts
        {
            get
            {
                return this.posts;
            }

            set
            {
                this.posts = value;
                this.OnPropertyChanged(nameof(this.Posts));
            }
        }

        public FeedViewModel()
        {
            // Default constructor for XAML
        }

        public FeedViewModel(IFeedServiceProxy service)
        {
            this.feedService = service;
            this.LoadPosts();
        }

        public async void LoadPosts() // MIGHT NEED TO BE CHECKED
        {
            var feedContent = await this.feedService.GetFeedContent();
            this.Posts = new ObservableCollection<Post>(feedContent);
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}