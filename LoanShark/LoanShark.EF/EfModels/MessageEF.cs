//using LoanShark.EF.EFModels;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LoanShark.EF.EfModels
//{
//    public class MessageEF
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int MessageId { get; set; }

//        public MessageTypeEF MessageType { get; set; }

//        public UserEF User { get; set; }

//        public ChatEF Chat { get; set; }   

//        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

//        public string Content
//    }
//}
