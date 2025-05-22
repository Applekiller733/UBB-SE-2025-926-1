using System.ComponentModel.DataAnnotations;

namespace LoanShark.MVC.Models
{
    public class GenerateTransferViewModel
    {
        public int ChatId { get; set; }

        [Range(0, 2, ErrorMessage = "Please select a transfer type.")]
        public int TransferTypeIndex { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount must be a valid number with up to 2 decimal places.")]
        public string AmountText { get; set; }

        [Range(0, 2, ErrorMessage = "Please select a currency.")]
        public int CurrencyIndex { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string Description { get; set; }

        public bool HasSufficientFunds { get; set; } = true;

        public bool ShowInsufficientFundsError => !HasSufficientFunds && SelectedTransferType == "Transfer Money" && !string.IsNullOrWhiteSpace(AmountText) && float.TryParse(AmountText, out float amount) && amount > 0 && CurrencyIndex >= 0;

        public bool IsFormValid { get; set; }

        public string SelectedTransferType
        {
            get
            {
                return TransferTypeIndex switch
                {
                    0 => "Transfer Money",
                    1 => "Request Money",
                    2 => "Split Bill",
                    _ => null
                };
            }
        }

        public string Currency
        {
            get
            {
                return CurrencyIndex switch
                {
                    0 => "USD",
                    1 => "EUR",
                    2 => "RON",
                    _ => null
                };
            }
        }

        public void ValidateForm()
        {
            IsFormValid = TransferTypeIndex >= 0 && CurrencyIndex >= 0 && !string.IsNullOrWhiteSpace(AmountText) && float.TryParse(AmountText, out float amount) && amount > 0 && (SelectedTransferType != "Transfer Money" || HasSufficientFunds);
        }
    }
}