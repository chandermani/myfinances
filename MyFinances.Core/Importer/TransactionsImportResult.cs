using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core.Model
{
    public class TransactionsImportResult
    {
        public TransactionsImportResult(IList<AccountTransactionImportResult> accountsImported=null)
        {
            AccountsImported = accountsImported?? new List<AccountTransactionImportResult>();
        }

        public IList<AccountTransactionImportResult> AccountsImported { get; }
    }

    public class AccountTransactionImportResult
    {
        public AccountTransactionImportResult(string accountIdentifier, int transactionsImported)
        {
            AccountIdentifier = accountIdentifier;
            TransactionsImported = transactionsImported;
        }

        public string AccountIdentifier { get; }
        public int TransactionsImported { get; }
    }
}
