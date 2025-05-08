using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class LoanDTO
{
    public int LoanID { get; set; }

    public int UserID { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public DateTime DateTaken { get; set; }

    public DateTime DateDeadline { get; set; }

    public DateTime? DatePaid { get; set; }

    public decimal TaxPercentage { get; set; }

    public int NumberMonths { get; set; }

    public string State { get; set; }
}

public class TakeLoanDTO
{
    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string AccountIBAN { get; set; }

    public int Months { get; set; }
}

public class PayLoanDTO
{
    public int UserId { get; set; }

    public int LoanId { get; set; }

    public string AccountIBAN { get; set; }
}

public class ConvertCurrencyDTO
{
    public decimal Amount { get; set; }

    public string FromCurrency { get; set; }

    public string ToCurrency { get; set; }
}
