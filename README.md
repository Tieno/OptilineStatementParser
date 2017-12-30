# BankStatementImporter
## Reads PDF bank statements

<i>I haven't tested this thoroughly - it works for one of my statements from my FNB check account.</i>

<pre><code>//This is the statement as e-mailed to me by FNB
private const string PdfFileName = @"C:\Bank Statements\statement_file_name.pdf";</pre></code>

<pre><code>//Setting up dependencies - you could use a DI Container
//Separate for testing purposes (automated tests HAVE not been included in this repo)
var fnbStatementMetaDataReader = new FnbStatementMetaDataReader();
var fnbTransactionLineParser = new FnbTransactionLineParser();
var fnbStatementTransactionReader = new FnbStatementTransactionReader(fnbTransactionLineParser);
var fnbStatementParser = new FnbStatementParser(fnbStatementMetaDataReader, fnbStatementTransactionReader);</pre></code>

<pre><code>//Parse the statement
var statement = fnbStatementParser.ParseStatement(PdfFileName);

//use statement.MetaData to get opening and closing balances, statement dates, etc.
//negative opening/closing balances indicate the account has gone into the red (overdraft), otherwise account has funds in it
//use statement.Transactions to get all transactions
//negative transactions indicate a debit on the account (money spent); positive indicate credit (money received)

//the sum of all transaction amounts should be equal to the closing balance less the opening balance
</pre></code>
