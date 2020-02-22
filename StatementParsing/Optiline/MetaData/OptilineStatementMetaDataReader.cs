using System;
using System.Collections.Generic;
using System.Linq;
using BankStatementImporter.Models;

namespace BankStatementImporter.StatementParsing.Optiline.MetaData
{
    public class OptilineStatementMetaDataReader
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
            var dateString = linesFromStatement.First(line => line.Contains("Uw overzicht op"));

            return extractStatementPeriodDates(dateString).First();
        }

        private List<DateTime> getStatementPeriodDates(IEnumerable<string> linesFromStatement)
        {
            var dateString = linesFromStatement.First(line => line.Contains("Uw overzicht op"));
            return extractStatementPeriodDates(dateString);
        }

        private decimal getOpeningBalance(IEnumerable<string> linesFromStatement)
        {
            return new decimal(5000);
        }

        private decimal getClosingBalance(IList<string> linesFromStatement)
        {
            var index = linesFromStatement.IndexOf("Uw beschikbaar bedrag :");
            var closingBal = linesFromStatement[index+1];
            return getBalanceFromString(closingBal);
        }

        private decimal getBalanceFromString(string openingBal)
        {
            openingBal = openingBal.Replace(" \u0080", "");
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
            for (var i = 0; i < partsWithNoSpaces.Length ; i++)
            {
                var tryDate = partsWithNoSpaces[i];
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
            var dec = decimal.Parse(textToSearch.Replace(".",""));
            return dec;
        }
    }
}