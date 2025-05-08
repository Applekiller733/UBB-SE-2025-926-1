using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Diagnostics;
using LoanShark.Data;
using LoanShark.Domain;
using Microsoft.Data.SqlClient;

namespace LoanShark.EF.Repository.BankRepository
{
    public interface ITransactionHistoryRepository
    {
        Task<ObservableCollection<Transaction>> GetTransactionsNormal();
        Task<ObservableCollection<string>> GetTransactionsForMenu();
        Task<ObservableCollection<string>> GetTransactionsDetailed();
        Task UpdateTransactionDescription(int transactionId, string newDescription);
    }
    // repo class is static but it should be fine as long as the methods are static, you dont need to change anything
    // about the functions
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        public TransactionHistoryRepository()
        {
        }

        // this retrieves all the transactions from the database, creeates a hashmap
        // for each row and then creates a transaction object from the hashmap
        // that is what the transaction has a constructor that takes a hashmap
        public async Task<ObservableCollection<Transaction>> GetTransactionsNormal()
        {
            ObservableCollection<Transaction> transactionsNormal = new ObservableCollection<Transaction>();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Iban", UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty)
                };
                DataTable result = await DataLink.Instance.ExecuteReader("GetAllTransactionsByIban", parameters);
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Debug.WriteLine("hashMap: " + hashMap);

                    Transaction transaction = new Transaction(hashMap);
                    transactionsNormal.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return transactionsNormal;
        }

        // gets transactions formatted for the menu
        public async Task<ObservableCollection<string>> GetTransactionsForMenu()
        {
            ObservableCollection<string> transactionsForMenu = new ObservableCollection<string>();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Iban", UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty)
                };
                DataTable result = await DataLink.Instance.ExecuteReader("GetAllTransactionsByIban", parameters);
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Transaction transaction = new Transaction(hashMap);
                    transactionsForMenu.Add(transaction.TostringForMenu());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return transactionsForMenu;
        }

        // gets transactions formatted for the detailed view
        public async Task<ObservableCollection<string>> GetTransactionsDetailed()
        {
            ObservableCollection<string> transactionsDetailed = new ObservableCollection<string>();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Iban", UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty)
                };
                DataTable result = await DataLink.Instance.ExecuteReader("GetAllTransactionsByIban", parameters);
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Transaction transaction = new Transaction(hashMap);
                    transactionsDetailed.Add(transaction.TostringDetailed());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return transactionsDetailed;
        }

        public Task UpdateTransactionDescription(int transactionId, string newDescription)
        {
            throw new NotImplementedException();
        }
    }
}
