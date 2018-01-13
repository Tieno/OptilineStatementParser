using System;

namespace BankStatementImporter.Models
{
    public class StatementTransaction
    {
        public DateTime Date { get; }
        public decimal Amount { get; }
        public string Reference { get; }
        public decimal AccountBalance { get; }

        public StatementTransaction(DateTime date, decimal amount, string reference, decimal accountBalance)
        {
            Date = date;
            Amount = amount;
            Reference = reference;
            AccountBalance = accountBalance;
        }
    }
}