using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanShark.EF.EFModels
{
    public class LoanEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public DateTime DateTaken { get; set; }

        [Required]
        public DateTime DateDeadline { get; set; }

        public DateTime? DatePaid { get; set; }

        [Required]
        public decimal TaxPercentage { get; set; }

        [Required]
        public int NumberMonths { get; set; }

        [Required]
        public string State { get; set; }

        // Not mapped, just a convenience getter (same as domain)
        [NotMapped]
        public decimal AmountToPay => Amount * (1 + (TaxPercentage / 100));
    }
}
