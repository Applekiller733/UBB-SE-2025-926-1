using LoanShark.Domain;
namespace LoanShark.MVC.Models
{
    public class FriendsListViewModel
    {
        public string SearchQuery { get; set; }
        public List<LoanShark.Domain.User> FriendsList { get; set; }
        public bool NoFriendsVisibility => FriendsList == null || FriendsList.Count == 0;
        public bool ShowAddFriends { get; set; }

        public FriendsListViewModel()
        {
            SearchQuery = string.Empty;
            FriendsList = new List<LoanShark.Domain.User>();
        }
    }
}
