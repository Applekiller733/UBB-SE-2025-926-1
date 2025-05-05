using System.ComponentModel.DataAnnotations;

namespace LoanShark.EF.EFModels
{
    public class BankAccountEF
    {
        [Key]
        public int Id { get; set; }
        public string Iban { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public bool Blocked { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal MaximumPerTransaction { get; set; }
        public int MaximumNrTransactions { get; set; }
    }
}
