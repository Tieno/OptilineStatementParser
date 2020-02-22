using System;
using System.Linq;
using BankStatementImporter.Models;

namespace BankStatementImporter.StatementParsing.Optiline.Transactions
{
    public class OptilineTransactionLineParser
    {
        public StatementTransaction Parse(string lineFromStatement, DateTime statementStartDate, DateTime statementEndDate)
        {

            var parts = lineFromStatement.Split(' ');
            var amount = parts[parts.Length - 2];
            var transactionAmount = decimal.Parse(amount);
            var endOfReferenceParts = parts.Length - (2 + 3);
            var reference = String.Join(" ", parts.Skip(2).Take(endOfReferenceParts));
            var currency = parts.Reverse().Skip(2).First();
            var counterPart = parts.Skip(2).First();
            var description = String.Join(" ", parts.Skip(3).Take(endOfReferenceParts-1));
            var dateString = parts[0];
            var transactionDate = determineTransactionDate(dateString, statementStartDate, statementEndDate);
            
            return new StatementTransaction
            {
                CounterParty = counterPart, Country = currency, Description = description, AccountBalance = 0, Amount = transactionAmount, Date = transactionDate, Reference = reference
            };
        }

        private DateTime determineTransactionDate(string dateString, DateTime statementStartDate, DateTime statementEndDate)
        {
            var tryDate1 = DateTime.Parse(dateString + " " + statementStartDate.Year);
            var tryDate2 = DateTime.Parse(dateString + " " + statementEndDate.Year);
            
            return isDateBetweenStartAndEnd(tryDate1, statementStartDate, statementEndDate) ? tryDate1 : tryDate2;
        }

        private bool isDateBetweenStartAndEnd(DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }
        public bool IsTransactionLine(string line)
        {
            var parts = line.Split(' ').Count(x => x.Contains("/") == true);
            return parts >= 2;
        }
    }
}