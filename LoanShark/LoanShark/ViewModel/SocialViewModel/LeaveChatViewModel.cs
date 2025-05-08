using LoanShark.API.Proxies;

namespace LoanShark.ViewModel.SocialViewModel
{
    using System.ComponentModel;
    using System.Windows.Input;
    using LoanShark.Service.SocialService.Interfaces;

    public class LeaveChatViewModel : INotifyPropertyChanged
    {
        public ICommand LeaveChatCommand { get; set; }

        private ISocialUserServiceProxy userService;
        private IChatServiceProxy chatService;
        private ChatListViewModel lastViewModel;
        private int chatID;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LeaveChatViewModel(ISocialUserServiceProxy userService, IChatServiceProxy chatService, ChatListViewModel chatMessagesViewModel, int chatID)
        {
            this.LeaveChatCommand = new RelayCommand(this.LeaveChat);

            this.chatID = chatID;
            this.userService = userService;
            this.chatService = chatService;
            this.lastViewModel = chatMessagesViewModel;
        }

        public void LeaveChat()
        {
            this.userService.LeaveChat(this.userService.GetCurrentUser(), this.chatID);

            this.lastViewModel.LoadChats();
        }
    }
}
