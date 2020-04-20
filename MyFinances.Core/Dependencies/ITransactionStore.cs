using MyFinances.Core.Model;
using System.Collections.Generic;

namespace MyFinances.Core
{
    public interface ITransactionStore
    {
        void AddAccount(User user, Account account);
        void AddTransactions(Account account, IList<Transaction> transactions);
    }
}