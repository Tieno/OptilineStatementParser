using System;

namespace BankStatementImporter.Models
{
    public class StatementMetaData
    {
        public DateTime StatementDate { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public decimal OpeningBalance { get; private set; }
        public decimal ClosingBalance { get; }

        public decimal TotalAmount
        {
            get { return this.OpeningBalance - ClosingBalance; }
        }

        public StatementMetaData(DateTime statementDate, DateTime startDate, DateTime endDate, decimal openingBalance, decimal closingBalance)
        {
            StatementDate = statementDate;
            StartDate = startDate;
            EndDate = endDate;
            OpeningBalance = openingBalance;
            ClosingBalance = closingBalance;
        }
    }
}