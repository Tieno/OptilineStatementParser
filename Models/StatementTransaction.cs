using System;

namespace BankStatementImporter.Models
{
    public class StatementTransaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public decimal AccountBalance { get; set; }

        public string Description { get; set; }
        public string Country { get; set; }

        public string CounterParty { get; set; }

        
    }
}