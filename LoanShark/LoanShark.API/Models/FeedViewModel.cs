namespace LoanShark.API.Models
{
    public class FeedViewModel
    {
        public int PostID { get; set; }

        public string? Title { get; set; }

        public string? Category { get; set; }

        public string? Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}