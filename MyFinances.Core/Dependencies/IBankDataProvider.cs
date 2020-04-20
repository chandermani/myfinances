using MyFinances.Core.Model;
using MyFinances.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinances.Core
{
    public interface IBankDataProvider
    {
        Task<IList<Account>> GetUserAccountsAsync(User user);
        Task<IList<Transaction>> GetAccountTransactionsAsync(User user, Account account, DateTime from, DateTime to);
    }
}