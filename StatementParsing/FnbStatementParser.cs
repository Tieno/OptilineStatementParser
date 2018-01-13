using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankStatementImporter.Models;
using BankStatementImporter.StatementParsing.MetaData;
using BankStatementImporter.StatementParsing.Transactions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace BankStatementImporter.StatementParsing
{
    public class FnbStatementParser
    {
        private readonly FnbStatementMetaDataReader _fnbStatementMetaDataReader;
        private readonly FnbStatementTransactionReader _fnbStatementTransactionReader;

        public FnbStatementParser(FnbStatementMetaDataReader fnbStatementMetaDataReader, FnbStatementTransactionReader fnbStatementTransactionReader)
        {
            _fnbStatementMetaDataReader = fnbStatementMetaDataReader;
            _fnbStatementTransactionReader = fnbStatementTransactionReader;
        }

        public Statement ParseStatement(string pdfFileName)
        {
            var textFromStatement = getLinesFromStatement(pdfFileName);

            var metaData = _fnbStatementMetaDataReader.getStatementMetaData(textFromStatement);
            var transactions = _fnbStatementTransactionReader.getTransactions(textFromStatement, metaData.StartDate, metaData.EndDate);

            return new Statement(textFromStatement, metaData, transactions);
        }

        private List<string> getLinesFromStatement(string pdfFileName)
        {
            using (var reader = new PdfReader(pdfFileName))
            {
                var text = new StringBuilder();
                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                var linesFromStatement = text.ToString().Split('\n').ToList();
                return linesFromStatement;
            }
        }
    }
}