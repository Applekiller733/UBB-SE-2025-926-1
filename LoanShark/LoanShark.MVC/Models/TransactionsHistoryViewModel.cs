using System.Collections.ObjectModel;

namespace LoanShark.MVC.Models
{
    public class TransactionsHistoryViewModel
    {
        public List<TransactionsHistoryDTO>? Transactions { get; set; }

        public string? Filter { get; set; }
    }

    public class TransactionsHistoryDTO
    {
        public string SenderIBAN { get; set; }

        public string ReceiverIBAN { get; set; }

        public string SentAmount { get; set; }

        public string ReceivedAmount { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }
    }
}


    //"Sender IBAN: " + SenderIban + "\n" +
    //               "Receiver IBAN: " + ReceiverIban + "\n\n" +
    //               "Sent Amount: " + SenderAmount + " " + SenderCurrency + "\n" +
    //               "Received Amount: " + ReceiverAmount + " " + ReceiverCurrency + "\n\n" +
    //               "Date: " + TransactionDatetime + "\n\n" +
    //               "Type: " + TransactionType;
