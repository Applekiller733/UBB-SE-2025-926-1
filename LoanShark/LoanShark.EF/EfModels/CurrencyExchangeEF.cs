using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanShark.EF.EFModels
{
    public class CurrencyExchangeEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FromCurrency { get; set; }

        [Required]
        public string ToCurrency { get; set; }

        [Required]
        public decimal ExchangeRate { get; set; }
    }
}
