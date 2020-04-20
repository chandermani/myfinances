using Microsoft.Extensions.Options;
using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Core.Importer
{
    public class TransactionsImporter : ITransactionsImporter
    {
        private readonly ITransactionStore transactionsStore;
        private readonly IBankDataProvider bankDataProvider;
        private readonly IClock clock;
        private readonly DataImportOptions dataImportOptions;

        public TransactionsImporter(ITransactionStore transactionsStore,
                                    IBankDataProvider bankDataProvider,
                                    IOptionsMonitor<DataImportOptions> dataImportOptions,
                                    IClock clock)
        {
            this.transactionsStore = transactionsStore;
            this.bankDataProvider = bankDataProvider;
            this.clock = clock;
            this.dataImportOptions = dataImportOptions.CurrentValue;
        }

        public async Task<TransactionsImportResult> ImportTransactionsAsync(User user)
        {
            TransactionsImportResult result = new TransactionsImportResult();
            var accounts = await bankDataProvider.GetUserAccountsAsync(user);

            foreach (var account in accounts)
            {
                transactionsStore.AddAccount(user, account);
                result.AccountsImported.Add(await ImportAcountTransactions(account));
            }

            return result;
        }

        private async Task<AccountTransactionImportResult> ImportAcountTransactions(Account account)
        {
            int transactionsRetreived = 0;
            // Importing last X years transactions by default. As the current API does not provide enough information around how old is the account.
            for (int i = 1; i <= dataImportOptions.MaxHistoricalTransactionToRetrieveInYears; i++)
            {
                var transactions = await bankDataProvider.GetAccountTransactionsAsync(account, clock.Now.Date.AddYears(-i).AddDays(1), clock.Now.Date.AddYears(-i + 1));
                transactionsRetreived += transactions.Count;
                transactionsStore.AddTransactions(account, transactions);
            }
            return new AccountTransactionImportResult(account.Identifier, transactionsRetreived);
        }
    }
}
