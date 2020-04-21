using System;
using System.Threading.Tasks;
using MyFinances.Core.Model;

namespace MyFinances.Core.Transactions
{
    public interface ITransactionSummaryBuilder
    {
        Task<TransactionsSummary> BuildAsync(string userIdentifier, DateTime from, DateTime to);
    }
}