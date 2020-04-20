using MyFinances.Core.Model;
using System;
using System.Threading.Tasks;

namespace MyFinances.Core
{
    public interface ITransactionsImporter
    {
        Task<TransactionsImportResult> ImportTransactionsAsync(string userIdentifier);
    }
}