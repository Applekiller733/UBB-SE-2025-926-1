using LoanShark.EF.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.EfModels
{
    // description = content ????????
    public class MessageEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }

        public int? TypeID { get; set; }

        public int UserID { get; set; }

        public int ChatID { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "NVARCHAR(260)")]
        [StringLength(260)]
        public string Content { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        [StringLength(255)]
        public string? Status { get; set; }

        public float? Amount { get; set; }

        [Column(TypeName = "VARCHAR(10)")]
        [StringLength(10)]
        public string? Currency { get; set; }

        public string? ImageUrl { get; set; }

        //public string? Description { get; set; }

        public string? SerializedUserIDs { get; set; }  // users who reported or users who got the transfer message

        [ForeignKey("TypeID")]
        public MessageTypeEF? MessageType { get; set; }

        [ForeignKey("UserID")]
        public UserEF User { get; set; }

        [ForeignKey("ChatID")]
        public ChatEF Chat { get; set; }
    }
}
