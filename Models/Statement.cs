using System.Collections.Generic;

namespace BankStatementImporter.Models
{
    public class Statement
    {
        public StatementMetaData MetaData { get; private set; }
        public IEnumerable<StatementTransaction> Transactions { get; private set; }
        public List<string> TextFromStatement { get; private set; }

        public Statement(List<string> textFromStatement, StatementMetaData metaData, IEnumerable<StatementTransaction> transactions)
        {
            TextFromStatement = textFromStatement;
            MetaData = metaData;
            Transactions = transactions;
        }
    }
}