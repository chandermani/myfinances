using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFinances.Core;
using MyFinances.Core.Dependencies;
using MyFinances.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Integrations.TrueLayer
{
    public class TrueLayerBankDataProvider : IBankDataProvider
    {
        private readonly ITrueLayerDataClientBuilder dataClientBuilder;
        private readonly ILogger<TrueLayerBankDataProvider> logger;

        public TrueLayerBankDataProvider(ITrueLayerDataClientBuilder dataClientBuilder,
            ILogger<TrueLayerBankDataProvider> logger)
        {
            this.dataClientBuilder = dataClientBuilder;
            this.logger = logger;
        }

        public async Task<IList<Transaction>> GetAccountTransactionsAsync(User user, Account account, DateTime from, DateTime to)
        {
            var client = dataClientBuilder.Build(user);
            var result = await client.GetAsync($"data/v1/accounts/{account.Identifier}/transactions?from={from}&to={to}");
            result.EnsureSuccessStatusCode();

            // TODO: Any better way to read as stream
            return MapResponseToTransactions(await result.Content.ReadAsStringAsync());
        }

        public async Task<IList<Account>> GetUserAccountsAsync(User user)
        {
            var client = dataClientBuilder.Build(user);
            var result = await client.GetAsync("data/v1/accounts");
            result.EnsureSuccessStatusCode();
            return MapResponseToAccounts(await result.Content.ReadAsStringAsync());
        }

        private IList<Account> MapResponseToAccounts(string content)
        {
            // TODO: Content parsing can be improved and moved to an independent class;
            dynamic accounts = JsonConvert.DeserializeObject(content);
            return ((IEnumerable<dynamic>)accounts.results)
                    .Select(a => new Account(a.account_id.Value, a.display_name.Value, a.account_type.Value))
                    .ToList();
        }

        private IList<Transaction> MapResponseToTransactions(string content)
        {
            // TODO: Content parsing can be improved and moved to an independent class;
            dynamic transactions = JsonConvert.DeserializeObject(content);
            return ((IEnumerable<dynamic>)transactions.results)
                    .Select(t => (Transaction)BuildTransaction(t))
                    .Where(t => t != null)
                    .ToList();
        }

        private Transaction BuildTransaction(dynamic transaction)
        {
            try
            {
                return new Transaction(transaction.transaction_id.Value, transaction.timestamp.Value, transaction.transaction_category.Value, (decimal)transaction.amount.Value, transaction.currency.Value);
            }
            catch(Exception ex)
            {
                // TODO: Handled failed transaction
                logger.LogError(ex.ToString());
            }

            return null;
        }
    }
}
