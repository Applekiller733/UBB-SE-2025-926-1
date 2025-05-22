using System.Collections.Generic;

namespace LoanShark.MVC.Models
{
    public class AddNewMemberViewModel
    {
        public string ChatName { get; set; }
        public List<FriendDTO> CurrentChatMembers { get; set; }
        public List<FriendDTO> UnaddedFriends { get; set; }
        public List<FriendDTO> NewlyAddedFriends { get; set; }
        public string SearchQuery { get; set; }
        public int ChatId { get; set; } // Added to store chatId
    }

    public class FriendDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
    }
}