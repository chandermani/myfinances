using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFinances.Core.Transactions
{
    public class TransactionsSummary
    {
        public TransactionsSummary(DateTime from, DateTime to, IList<CategorySummary> categories)
        {
            From = from;
            To = to;
            Categories = categories;
        }

        public DateTime From { get; }
        public DateTime To { get; }
        public IList<CategorySummary> Categories { get; }
    }

    public class CategorySummary
    {
        public CategorySummary(string category, IList<AccountCategorySummary> accounts)
        {
            Category = category;
            Accounts = accounts;
        }

        public string Category { get; }
        public IList<AccountCategorySummary> Accounts { get; }
        public decimal TotalSpend => Accounts.Sum(a => a.TotalSpend);
    }

    public class AccountCategorySummary
    {
        public AccountCategorySummary(string accountIdentifier, string category, decimal totalSpend)
        {
            AccountIdentifier = accountIdentifier;
            Category = category;
            TotalSpend = totalSpend;
        }

        public string AccountIdentifier { get; }
        public string Category { get; }
        public decimal TotalSpend { get; }
    }
}