using System.Collections.Generic;
using System.Linq;

namespace BankStatementImporter.Models
{
    public class Statement
    {
        public StatementMetaData MetaData { get; private set; }
        public ICollection<StatementTransaction> Transactions { get; private set; }
        public List<string> TextFromStatement { get; private set; }

        public Statement(List<string> textFromStatement, StatementMetaData metaData, ICollection<StatementTransaction> transactions)
        {
            TextFromStatement = textFromStatement;
            MetaData = metaData;
            Transactions = transactions;
            
        }

        public bool IsConsistent
        {
            get { return this.MetaData.TotalAmount == this.Transactions.Sum(x => x.Amount); }
        }
    }
}