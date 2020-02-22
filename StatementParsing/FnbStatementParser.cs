using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankStatementImporter.Models;
using BankStatementImporter.StatementParsing.Optiline.MetaData;
using BankStatementImporter.StatementParsing.Optiline.Transactions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace BankStatementImporter.StatementParsing
{
    public class FnbStatementParser
    {
        private readonly OptilineStatementMetaDataReader _optilineStatementMetaDataReader;
        private readonly OptilineStatementTransactionReader _optilineStatementTransactionReader;

        public FnbStatementParser(OptilineStatementMetaDataReader optilineStatementMetaDataReader, OptilineStatementTransactionReader optilineStatementTransactionReader)
        {
            _optilineStatementMetaDataReader = optilineStatementMetaDataReader;
            _optilineStatementTransactionReader = optilineStatementTransactionReader;
        }

        public Statement ParseStatement(string pdfFileName)
        {
            var textFromStatement = getLinesFromStatement(pdfFileName);

            var metaData = _optilineStatementMetaDataReader.getStatementMetaData(textFromStatement);
            var transactions = _optilineStatementTransactionReader.getTransactions(textFromStatement, metaData.StartDate, metaData.EndDate).ToArray();

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