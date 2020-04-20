using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinances.Core
{
    public interface IBankDataProvider
    {
        Task<IList<Account>> GetUserAccountsAsync(User user);
        Task<IList<Transaction>> GetAccountTransactionsAsync(Account account, DateTime from, DateTime to);
    }
}