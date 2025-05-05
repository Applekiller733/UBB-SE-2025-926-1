using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanShark.EF.EFModels
{
    public class TransactionEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        public string SenderIban { get; set; }

        [Required]
        public string ReceiverIban { get; set; }

        [Required]
        public DateTime TransactionDatetime { get; set; }

        [Required]
        public string SenderCurrency { get; set; }

        [Required]
        public string ReceiverCurrency { get; set; }

        [Required]
        public decimal SenderAmount { get; set; }

        [Required]
        public decimal ReceiverAmount { get; set; }

        [Required]
        public string TransactionType { get; set; }

        public string TransactionDescription { get; set; }
    }
}
