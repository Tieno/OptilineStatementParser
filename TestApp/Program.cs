using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using BankStatementImporter.Models;
using BankStatementImporter.StatementParsing;
using BankStatementImporter.StatementParsing.Optiline.MetaData;
using BankStatementImporter.StatementParsing.Optiline.Transactions;

namespace TestApp
{
    class Program
    {
        //This is the statement as e-mailed to me by FNB
        static void Main(string[] args)
        {

            //Setting up dependencies - you could use a DI Container
            //Separate for testing purposes (automated tests HAVE not been included in this repo)
            var fnbStatementMetaDataReader = new OptilineStatementMetaDataReader();
            var fnbTransactionLineParser = new OptilineTransactionLineParser();
            var fnbStatementTransactionReader = new OptilineStatementTransactionReader(fnbTransactionLineParser);
            var fnbStatementParser = new FnbStatementParser(fnbStatementMetaDataReader, fnbStatementTransactionReader);
            //Parse the statement
            string PdfFileName = "";
            string Dir = "";
            var statement1 = fnbStatementParser.ParseStatement(PdfFileName);
            var statements = new List<Statement>();
            foreach (var file in Directory.GetFiles(Dir))
            {
                var statement = fnbStatementParser.ParseStatement(file);
                statements.Add(statement);
                Debug.Assert(statement.IsConsistent);
            }

            foreach (var statement in statements)
            {
                statement.TextFromStatement.Clear();
            }

            var json = JsonSerializer.Serialize(statements);
            //use statement.MetaData to get opening and closing balances, statement dates, etc.
            //negative opening/closing balances indicate the account has gone into the red (overdraft), otherwise account has funds in it
            //use statement.Transactions to get all transactions
            //negative transactions indicate a debit on the account (money spent); positive indicate credit (money received)

            //the sum of all transaction amounts should be equal to the closing balance less the opening balance
            Console.WriteLine("Hello World!");

        }
    }
}
