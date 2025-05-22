namespace LoanShark.MVC.Models
{
    public class AddFriendsViewModel
    {
        public string SearchQuery { get; set; }
        public List<User> UsersList { get; set; }

        public AddFriendsViewModel()
        {
            SearchQuery = string.Empty;
            UsersList = new List<User>();
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
    }
}
