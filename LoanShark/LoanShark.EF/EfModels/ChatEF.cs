using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanShark.EF.EFModels
{
    /// <summary>
    /// Entity Framework model representing a Chat.
    /// </summary>
    public class ChatEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ChatName { get; set; }

        // This is a simplified approach — ideally you'd have a proper User entity and a join table
        public ICollection<ChatUserEF> ChatUsers { get; set; } = new List<ChatUserEF>();
    }

    /// <summary>
    /// Join entity representing a many-to-many between Chat and User IDs.
    /// </summary>
    public class ChatUserEF
    {
        [Key]
        public int Id { get; set; }

        public int ChatId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("ChatId")]
        public ChatEF Chat { get; set; }

        [ForeignKey("UserId")]
        public UserEF User { get; set; }
    }
}
