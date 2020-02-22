using System;
using System.Collections.Generic;
using System.Linq;
using BankStatementImporter.Models;

namespace BankStatementImporter.StatementParsing.Optiline.Transactions
{
    public class OptilineStatementTransactionReader
    {
        private readonly OptilineTransactionLineParser _transactionLineParser;

        public OptilineStatementTransactionReader(OptilineTransactionLineParser transactionLineParser)
        {
            _transactionLineParser = transactionLineParser;
        }

        public IEnumerable<StatementTransaction> getTransactions(List<string> linesFromStatement, DateTime statementStartDate, DateTime statementEndDate)
        {
            bool isStatementTable = false;
            var transactionLines = new List<string>();
            foreach (var line in linesFromStatement)
            {
                if (line.Contains("NIEUW SALDO"))
                {
                    isStatementTable = false;
                }
                if (isStatementTable)
                {
                    if (_transactionLineParser.IsTransactionLine(line))
                    {
                        yield return _transactionLineParser.Parse(line, statementStartDate, statementEndDate);
                    }
                    
                }

                if (line.Contains("KAARTNUMMER"))
                {
                    isStatementTable = true;
                }

            }
        }

        private IEnumerable<string> getTransactionLinesFromStatement(IEnumerable<string> linesFromStatement)
        {
            return linesFromStatement.Where(isTransactionLine).ToList();
        }

        public bool isTransactionLine(string line)
        {
            return _transactionLineParser.IsTransactionLine(line);
        }
    }
}