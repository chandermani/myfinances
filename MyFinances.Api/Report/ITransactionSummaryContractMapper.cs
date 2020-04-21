using MyFinances.Api.Report.Contracts;
using MyFinances.Core.Transactions;
using System;

namespace MyFinances.Api.Report
{
    public interface ITransactionSummaryContractMapper
    {
        Contracts.TransactionsSummary MapToContract(Core.Transactions.TransactionsSummary summaryModel, DateTime from, DateTime to);
    }
}