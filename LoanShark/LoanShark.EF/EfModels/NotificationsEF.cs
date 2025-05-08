using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanShark.EF.EFModels
{
    public class NotificationEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int UserReceiverID { get; set; }

        [ForeignKey("UserReceiverID")]
        public UserEF User { get; set; }
    }
}
