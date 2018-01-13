using System;
using System.Collections.Generic;
using System.Linq;
using BankStatementImporter.Models;

namespace BankStatementImporter.StatementParsing.Transactions
{
    public class FnbStatementTransactionReader
    {
        private readonly FnbTransactionLineParser _transactionLineParser;

        public FnbStatementTransactionReader(FnbTransactionLineParser transactionLineParser)
        {
            _transactionLineParser = transactionLineParser;
        }

        public IEnumerable<StatementTransaction> getTransactions(List<string> linesFromStatement, DateTime statementStartDate, DateTime statementEndDate)
        {
            return getTransactionLinesFromStatement(linesFromStatement)
                .Select(lineFromStatement=>_transactionLineParser.Parse(lineFromStatement, statementStartDate, statementEndDate));
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