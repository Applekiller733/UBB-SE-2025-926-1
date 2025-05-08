using LoanShark.EF.EfModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanShark.EF.EFModels
{
    public class ReportEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // EF primary key (doesn't exist in the domain model)

        [Required]
        public int MessageID { get; set; }

        [Required]
        public int ReporterUserID { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("MessageID")]
        public MessageEF MessageEF { get; set; }
    }
}
