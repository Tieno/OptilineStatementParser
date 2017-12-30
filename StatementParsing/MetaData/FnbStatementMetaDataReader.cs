using System;
using System.Collections.Generic;
using System.Linq;
using BankStatementImporter.Models;

namespace BankStatementImporter.StatementParsing.MetaData
{
    public class FnbStatementMetaDataReader
    {
        public StatementMetaData getStatementMetaData(IList<string> linesFromStatement)
        {
            var statementDate = getStatementDate(linesFromStatement);
            var statementPeriodDates = getStatementPeriodDates(linesFromStatement);
            var startDate = statementPeriodDates.First();
            var endDate = statementPeriodDates.Last();
            var openingBalance = getOpeningBalance(linesFromStatement);
            var closingBalance = getClosingBalance(linesFromStatement);
            var metaData = new StatementMetaData(statementDate, startDate, endDate, openingBalance, closingBalance);
            return metaData;
        }

        private DateTime getStatementDate(IEnumerable<string> linesFromStatement)
        {
            var dateString = linesFromStatement.First(line => line.StartsWith("Statement Date"));
            return extractStatementPeriodDates(dateString).First();
        }

        private List<DateTime> getStatementPeriodDates(IEnumerable<string> linesFromStatement)
        {
            var dateString = linesFromStatement.First(line => line.StartsWith("Statement Period"));
            return extractStatementPeriodDates(dateString);
        }

        private decimal getOpeningBalance(IEnumerable<string> linesFromStatement)
        {
            var openingBal = linesFromStatement.First(line => line.StartsWith("Opening Balance"));
            return getBalanceFromString(openingBal);
        }

        private decimal getClosingBalance(IList<string> linesFromStatement)
        {
            var closingBal = linesFromStatement.First(line => line.StartsWith("Closing Balance"));
            return getBalanceFromString(closingBal);
        }

        private decimal getBalanceFromString(string openingBal)
        {
            var numbersFromString = parseDecimalValue(openingBal);
            var isDr = openingBal.ToUpper().EndsWith("DR");
            return applySignChange(numbersFromString, isDr);
        }

        private decimal applySignChange(decimal number, bool isDr)
        {
            return isDr ? decimal.Negate(number) : number;
        }

        private List<DateTime> extractStatementPeriodDates(string textToSearch)
        {
            var partsWithNoSpaces = textToSearch.Split(' ');
            var datesFoundInString = new List<DateTime>();
            for (var i = 0; i < partsWithNoSpaces.Length - 2; i++)
            {
                var tryDate = $"{partsWithNoSpaces[i]} {partsWithNoSpaces[i + 1]} {partsWithNoSpaces[i + 2]}";
                DateTime date;
                if (DateTime.TryParse(tryDate, out date))
                {
                    datesFoundInString.Add(date);
                }
            }
            return datesFoundInString;
        }

        private decimal parseDecimalValue(string textToSearch)
        {
            var numbersInString = string.Join("", textToSearch.Where(c => ".0123456789".Contains(c)));
            if (numbersInString.Length == 0)
                throw new StatementParseException("Could not locate number in line '" +
                                                  textToSearch + "'");
            return decimal.Parse(numbersInString);
        }
    }
}