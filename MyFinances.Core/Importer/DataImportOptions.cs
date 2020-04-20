using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core.Importer
{
    public class DataImportOptions
    {
        public DataImportOptions(int maxHistoricalTransactionToRetrieveInYears)
        {
            MaxHistoricalTransactionToRetrieveInYears = maxHistoricalTransactionToRetrieveInYears;
        }

        public int MaxHistoricalTransactionToRetrieveInYears { get; }
    }
}
