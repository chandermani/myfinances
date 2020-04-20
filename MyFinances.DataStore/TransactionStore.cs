using MyFinances.Core;
using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.DataStore
{
    public class TransactionStore : ITransactionStore
    {
        private Dictionary<string, List<Account>> userAccounts;
        private Dictionary<string, List<Transaction>> accountTransactions;
        public TransactionStore()
        {
            userAccounts = new Dictionary<string, List<Account>>();
            accountTransactions = new Dictionary<string, List<Transaction>>();
        }

        public void AddAccount(User user, Account account)
        {
            // TODO: Skeleton implementation, no validation done
            if (!userAccounts.ContainsKey(user.Identifier)) userAccounts[user.Identifier] = new List<Account>();
            userAccounts[user.Identifier].Add(account);
        }

        public void AddTransactions(Account account, IList<Transaction> transactions)
        {
            // TODO: Skeleton implementation, no validation done
            if (!accountTransactions.ContainsKey(account.Identifier)) accountTransactions[account.Identifier] = new List<Transaction>();
            accountTransactions[account.Identifier].AddRange(transactions);
        }
    }
}
