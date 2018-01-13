using System;
using System.Text.RegularExpressions;
using BankStatementImporter.Models;
using Org.BouncyCastle.Utilities.Date;

namespace BankStatementImporter.StatementParsing.Transactions
{
    public class FnbTransactionLineParser
    {
        public StatementTransaction Parse(string lineFromStatement, DateTime statementStartDate, DateTime statementEndDate)
        {
            //values in format ###,###.## with optional Cr following
            //e.g. 1,000,000.00 Cr or 100.00 both accepted
            const string pattern = @"\d{1,3}(,\d{3})*[.]\d{2}( (C|c)(r|R)){0,1}";

            //up to three decimals at the end of the line: transaction amount, account balance, accrued bank charges (ignore)
            var amountMatches = Regex.Matches(lineFromStatement, pattern);
            var transactionAmount = extractValueAndApplyCrDr(amountMatches[0].Value);
            var accountBalance = extractValueAndApplyCrDr(amountMatches[1].Value);

            //strip out the date (first 6 chars) and delete all amounts - the remaining middle bit is the reference
            var indexOfFirstAmount = amountMatches[0].Index;
            var dateString = lineFromStatement.Substring(0, 6);
            var transactionDate = determineTransactionDate(dateString, statementStartDate, statementEndDate);
            var transactionReference = lineFromStatement.Remove(indexOfFirstAmount).Remove(0, dateString.Length).Trim();
            
            return new StatementTransaction(transactionDate, transactionAmount, transactionReference, accountBalance);
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

        private decimal extractValueAndApplyCrDr(string valueWithOptionalSign)
        {
            var partsWithoutSpaces = valueWithOptionalSign.Split(' ');
            var amountAsDecimal = Convert.ToDecimal(partsWithoutSpaces[0]);

            var isCredit = valueWithOptionalSign.ToUpper().EndsWith("CR");
            return isCredit ? amountAsDecimal : decimal.Negate(amountAsDecimal);
        }

        public bool IsTransactionLine(string line)
        {
            const string patternForDayAndMonth = @"^([0-3]{1}[0-9]{1})\s(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec).+";
            return Regex.IsMatch(line, patternForDayAndMonth);
        }
    }
}