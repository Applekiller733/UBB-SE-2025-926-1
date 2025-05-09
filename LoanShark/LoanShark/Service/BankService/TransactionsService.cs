﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.EF.Repository.BankRepository;

namespace LoanShark.Service.BankService
{
    public class TransactionsService
    {
        private ITransactionsRepository transactionsRepository;

        public TransactionsService()
        {
            transactionsRepository = new TransactionsRepository(DataLink.Instance);
        }

        public TransactionsService(ITransactionsRepository transactionsRepository)
        {
            this.transactionsRepository = transactionsRepository;
        }

        public async Task<string> AddTransaction(string senderIban, string receiverIban, decimal amount, string transactionDescription = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(senderIban) || string.IsNullOrWhiteSpace(receiverIban))
                {
                    return "Receiver IBANs must be provided.";
                }

                if (amount <= 0)
                {
                    return "Invalid transaction amount. Must be greater than zero.";
                }

                if (senderIban == receiverIban)
                {
                    return "Cannot send money to the same account.";
                }

                Debug.WriteLine($"DEBUG: Processing transaction from {senderIban} to {receiverIban}, Amount: {amount}");

                BankAccount? senderAccount = await transactionsRepository.GetBankAccountByIBAN(senderIban);
                BankAccount? receiverAccount = await transactionsRepository.GetBankAccountByIBAN(receiverIban);

                if (senderAccount == null)
                {
                    return "Sender account does not exist.";
                }
                if (receiverAccount == null)
                {
                    return "Receiver account does not exist.";
                }

                if (senderAccount.Blocked)
                {
                    return "Sender account is blocked.";
                }
                if (receiverAccount.Blocked)
                {
                    return "Receiver account is blocked.";
                }

                if (senderAccount.Balance < amount)
                {
                    return "Insufficient funds.";
                }

                if (amount > senderAccount.MaximumPerTransaction)
                {
                    return $"Transaction exceeds maximum limit per transaction ({senderAccount.MaximumPerTransaction}).";
                }

                decimal receiverAmount = amount;

                if (senderAccount.Currency != receiverAccount.Currency)
                {
                    decimal exchangeRate = await transactionsRepository.GetExchangeRate(senderAccount.Currency, receiverAccount.Currency);
                    if (exchangeRate == -1)
                    {
                        return "Exchange rate not available.";
                    }

                    receiverAmount = Math.Round(amount * exchangeRate, 2);
                }

                Transaction transaction = new Transaction
                {
                    SenderIban = senderIban,
                    ReceiverIban = receiverIban,
                    TransactionDatetime = DateTime.UtcNow,
                    SenderCurrency = senderAccount.Currency,
                    ReceiverCurrency = receiverAccount.Currency,
                    SenderAmount = amount,
                    ReceiverAmount = receiverAmount,
                    TransactionType = "user to user",
                    TransactionDescription = transactionDescription
                };

                await transactionsRepository.AddTransaction(transaction);
                await transactionsRepository.UpdateBankAccountBalance(senderIban, senderAccount.Balance - amount);
                await transactionsRepository.UpdateBankAccountBalance(receiverIban, receiverAccount.Balance + receiverAmount);

                return "Transaction successful!";
            }
            catch (Exception ex)
            {
                return $"Error processing transaction: {ex.Message}";
            }
        }

        public async Task<string> TakeLoanTransaction(string iban, decimal loanAmount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                {
                    return "IBAN must be provided.";
                }

                if (loanAmount <= 0)
                {
                    return "Invalid loan amount. Must be greater than zero.";
                }

                BankAccount? userAccount = await transactionsRepository.GetBankAccountByIBAN(iban);

                if (userAccount == null)
                {
                    return "Bank account does not exist.";
                }
                if (userAccount.Blocked)
                {
                    return "Bank account is blocked.";
                }

                await transactionsRepository.UpdateBankAccountBalance(iban, userAccount.Balance + loanAmount);

                Transaction transaction = new Transaction
                {
                    SenderIban = "RO90BANK0000000000000005",
                    ReceiverIban = iban,
                    TransactionDatetime = DateTime.UtcNow,
                    SenderCurrency = userAccount.Currency,
                    ReceiverCurrency = userAccount.Currency,
                    SenderAmount = loanAmount,
                    ReceiverAmount = loanAmount,
                    TransactionType = "loan",
                    TransactionDescription = "Loan credited to account"
                };

                await transactionsRepository.AddTransaction(transaction);

                return "Loan credited to your account!";
            }
            catch (Exception ex)
            {
                return $"Error processing loan transaction: {ex.Message}";
            }
        }

        public async Task<string> PayLoanTransaction(string iban, decimal paymentAmount)
        {
            try
            {
                const string bankIban = "RO90BANK0000000000000005";

                if (string.IsNullOrWhiteSpace(iban))
                {
                    return "IBAN must be provided.";
                }

                if (paymentAmount <= 0)
                {
                    return "Invalid payment amount. Must be greater than zero.";
                }

                BankAccount? userAccount = await transactionsRepository.GetBankAccountByIBAN(iban);
                BankAccount? bankAccount = await transactionsRepository.GetBankAccountByIBAN(bankIban);

                if (userAccount == null)
                {
                    return "User account does not exist.";
                }
                if (userAccount.Blocked)
                {
                    return "User account is blocked.";
                }
                if (userAccount.Balance < paymentAmount)
                {
                    return "Insufficient funds.";
                }
                if (bankAccount == null)
                {
                    return "Bank account does not exist.";
                }

                await transactionsRepository.UpdateBankAccountBalance(iban, userAccount.Balance - paymentAmount);

                Transaction transaction = new Transaction
                {
                    SenderIban = iban,
                    ReceiverIban = bankIban,
                    TransactionDatetime = DateTime.UtcNow,
                    SenderCurrency = userAccount.Currency,
                    ReceiverCurrency = userAccount.Currency,
                    SenderAmount = paymentAmount,
                    ReceiverAmount = paymentAmount,
                    TransactionType = "loan",
                    TransactionDescription = "Loan payment deducted from account"
                };

                await transactionsRepository.AddTransaction(transaction);

                return "Loan payment successful!";
            }
            catch (Exception ex)
            {
                return $"Error processing loan payment: {ex.Message}";
            }
        }

        public async Task<List<CurrencyExchange>> GetAllCurrencyExchangeRates()
        {
            try
            {
                return await transactionsRepository.GetAllCurrencyExchangeRates();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving currency exchange rates: {ex.Message}", ex);
            }
        }
    }
}
