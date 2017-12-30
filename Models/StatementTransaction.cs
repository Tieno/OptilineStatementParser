using NUnit.Framework.Constraints;

namespace BankStatementImporter.Models
{
    public class StatementTransaction
    {
        public string Reference { get; }
        public decimal AccountBalance { get; }
        public decimal Amount { get; }

        public StatementTransaction(decimal amount, decimal accountBalance, string reference)
        {
            Amount = amount;
            AccountBalance = accountBalance;
            Reference = reference;
        }
    }
}