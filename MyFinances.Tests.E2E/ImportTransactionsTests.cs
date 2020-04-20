using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFinances.Tests.E2E
{
    public class ImportTransactionsTests: AcceptanceTestsContext
    {
        public ImportTransactionsTests()
        {
            SetupInMemoryTestServerAndData();
        }

        [Fact]
        public async Task Should_import_transactions_for_john_doe()
        {
            // Arrange
            await PersistAccessCode();

            // Act
            var result = await ApiClient.PostAsync("users/john@doe.com/import", null);

            // Assert
            result.EnsureSuccessStatusCode();
        }
    }
}
