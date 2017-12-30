using System;

namespace BankStatementImporter
{
    public class StatementParseException : Exception
    {
        public StatementParseException(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}