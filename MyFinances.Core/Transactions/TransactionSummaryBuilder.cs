using MyFinances.Core.Dependencies;
using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Core.Transactions
{
    public class TransactionSummaryBuilder : ITransactionSummaryBuilder
    {
        private readonly ITransactionStore transactionStore;
        private readonly IUsersStore usersStore;
        private readonly ITransactionsImporter transactionsImporter;

        public TransactionSummaryBuilder(ITransactionStore transactionStore, 
                                        IUsersStore usersStore,
                                        ITransactionsImporter transactionsImporter)
        {
            this.transactionStore = transactionStore;
            this.usersStore = usersStore;
            this.transactionsImporter = transactionsImporter;
        }

        public async Task<TransactionsSummary> BuildAsync(string userIdentifier, DateTime from, DateTime to)
        {
            var accountTransactions = transactionStore.GetTransactions(usersStore.GetUser(userIdentifier), from, to);

            if (accountTransactions.Count == 0)
            {
                // TODO: Should this class do the import? Currently it does.
                await transactionsImporter.ImportTransactionsAsync(userIdentifier);
                accountTransactions = transactionStore.GetTransactions(usersStore.GetUser(userIdentifier), from, to);
            }

            // TODO: Need to improve it and make it more readable.
            var categories = accountTransactions.SelectMany(at => at.Value, (at, t) => new { AccountIdentifier = at.Key, Transaction = t })
               .GroupBy(at => new { at.AccountIdentifier, at.Transaction.Category })    // Calcuate summary based on account and category
               .Select(g => new { g.Key.AccountIdentifier, g.Key.Category, Summary = BuildAccountSummary(g.Key.AccountIdentifier, g.Key.Category, g.Select(at => at.Transaction)) })
               .GroupBy(accountSummary => accountSummary.Category)  // Group again on category
               .Select(g => new CategorySummary(g.Key, g.Select(accountSummary => accountSummary.Summary).ToList()))
               .ToList();

            return new TransactionsSummary(from, to, categories);
        }

        private AccountCategorySummary BuildAccountSummary(string accountIdentifier, string category, IEnumerable<Transaction> transactions)
        {
            return new AccountCategorySummary(accountIdentifier,
                                                category,
                                                -transactions.Where(t => t.Amount < 0).Sum(t => t.Amount));     // Only consider spends
        }
    }
}
