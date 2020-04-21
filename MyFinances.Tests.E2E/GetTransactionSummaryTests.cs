using FluentAssertions;
using MyFinances.Api.Report.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFinances.Tests.E2E
{
    public class GetTransactionSummaryTests : AcceptanceTestsContext
    {
        public GetTransactionSummaryTests()
        {
            SetupInMemoryTestServerAndData();
        }

        [Fact]
        public async Task When_requesting_transaction_summary_for_a_user_last_one_week_transactions_are_returned()
        {
            // Arrange
            await PersistAccessCode(CommonUserIdentifier);

            // Act
            var result = await ApiClient.GetAsync($"users/{CommonUserIdentifier}/transactions/categorysummary");

            // Assert
            result.EnsureSuccessStatusCode();
            var summary = JsonConvert.DeserializeObject<TransactionsSummary>(await result.Content.ReadAsStringAsync());
            summary.From.Should().Be(DateTime.Now.AddDays(-6).Date);
            summary.To.Should().BeCloseTo(DateTime.Now, 300000);    // 5 mins
            summary.Categories.Count.Should().BeGreaterThan(0);
            summary.Categories.All(c => c.TotalSpend >= 0).Should().BeTrue();
            summary.Categories.SelectMany(c => c.Accounts).All(a => a.TotalSpend >= 0).Should().BeTrue();
        }
    }
}
