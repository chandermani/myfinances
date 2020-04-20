using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFinances.Tests.E2E
{
    public class LinkUserAccountTests: AcceptanceTestsContext
    {
        public LinkUserAccountTests()
        {
            SetupInMemoryTestServerAndData();
        }

        [Fact]
        public async Task When_access_code_is_returned_links_the_user_account()
        {
            // Arrange
            var responseUri = GetAccessCodeResponse();

            // Act
            var result = await ApiClient.GetAsync(responseUri.PathAndQuery);

            // Assert
            result.EnsureSuccessStatusCode();
        }
    }
}
