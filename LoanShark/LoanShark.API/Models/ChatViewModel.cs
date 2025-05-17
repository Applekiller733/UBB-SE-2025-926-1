namespace LoanShark.API.Models
{
    public class ChatViewModel
    {
        public int ChatID { get; set; }

        public List<int> UserIDs { get; set; }

        public string ChatName { get; set; }
    }
    public class RequestMoneyDto
    {
        public float Amount { get; set; }

        public string Currency { get; set; }

        public int ChatID { get; set; }

        public string Description { get; set; }
    }

    public class SendMoneyDto
    {
        public float Amount { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public int ChatID { get; set; }
    }

    public class AcceptRequestDto
    {
        public float Amount { get; set; }

        public string Currency { get; set; }

        public int AccepterID { get; set; }

        public int RequesterID { get; set; }

        public int ChatID { get; set; }
    }

    public class CreateChatDto
    {
        public List<int> ParticipantsID { get; set; }

        public string ChatName { get; set; }
    }
}
