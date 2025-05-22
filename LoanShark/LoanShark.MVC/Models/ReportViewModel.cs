using System.Collections.Generic;

namespace LoanShark.MVC.Models
{
    public class ReportViewModel
    {
        public int ReportedUserId { get; set; }

        public int MessageId { get; set; }

        public string SelectedCategory { get; set; }

        public string OtherReason { get; set; }

        public List<string> Categories { get; set; } = new List<string>
        {
            "Spam",
            "Harassment",
            "Inappropriate content",
            "Misinformation",
            "Other",
        };
    }
}