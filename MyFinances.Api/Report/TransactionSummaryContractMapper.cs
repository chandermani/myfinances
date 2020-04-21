using MyFinances.Api.Report.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinances.Api.Report
{
    public class TransactionSummaryContractMapper : ITransactionSummaryContractMapper
    {
        public TransactionsSummary MapToContract(Core.Transactions.TransactionsSummary summaryModel, DateTime from, DateTime to)
        {
            return new Contracts.TransactionsSummary()
            {
                From = from,
                To = to,
                Categories = summaryModel.Categories.Select(c => new CategorySummary()
                {
                    Category = c.Category,
                    TotalSpend = c.TotalSpend,
                    Accounts = c.Accounts.Select(a => new AccountCategorySummary()
                    {
                        AccountIdentifier = a.AccountIdentifier,
                        Category = a.Category,
                        TotalSpend = a.TotalSpend
                    }).ToList()
                }).ToList()
            };
        }
    }
}
