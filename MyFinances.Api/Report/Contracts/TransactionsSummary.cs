using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinances.Api.Report.Contracts
{
    public class TransactionsSummary
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<CategorySummary> Categories { get; set; }
    }

    public class CategorySummary
    {
        public string Category { get; set; }
        public List<AccountCategorySummary> Accounts { get; set; }
        public decimal TotalSpend { get; set; }
    }

    public class AccountCategorySummary
    {
        public string AccountIdentifier { get; set; }
        public string Category { get; set; }
        public decimal TotalSpend { get; set; }
    }
}
