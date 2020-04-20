using FluentAssertions;
using Moq;
using MyFinances.Core.Dependencies;
using MyFinances.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFinances.Core.Tests.Tokens
{
    public class UserTokenServiceTests
    {
        private const string UserIdentifier = "john@doe.com";
        private Mock<IUserTokenStore> userTokenStore;
        private Mock<IBankAuthTokenProvider> bankAuthTokenProvider;

        [Fact]
        public async Task When_access_token_generation_is_requested_for_user_gets_and_saves_access_token()
        {
            // Arrange
            SetupDependencies();
            UserTokenService target = BuildTarget();

            //Act
            var result = await target.GenerateTokenAsync(UserIdentifier, "dummycode");

            // Assert
            result.Should().NotBeNull();
            bankAuthTokenProvider.Verify(p => p.GetAccessToken("dummycode"), Times.Once);
            userTokenStore.Verify(s => s.UpdateToken(UserIdentifier, result), Times.Once);
        }

        [Fact]
        public void When_access_token_is_requested_loads_access_token_from_store()
        {
            // Arrange
            SetupDependencies();
            UserTokenService target = BuildTarget();

            //Act
            var result = target.GetToken(UserIdentifier);

            //Assert
            result.Should().NotBeNull();
            userTokenStore.Verify(s => s.GetToken(UserIdentifier), Times.Once);
        }

        private UserTokenService BuildTarget()
        {
            return new UserTokenService(userTokenStore.Object, bankAuthTokenProvider.Object);
        }

        private void SetupDependencies()
        {
            userTokenStore = new Mock<IUserTokenStore>();
            bankAuthTokenProvider = new Mock<IBankAuthTokenProvider>();

            bankAuthTokenProvider.Setup(p => p.GetAccessToken(It.IsAny<string>())).ReturnsAsync(new Token(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.UtcNow));

            userTokenStore.Setup(s => s.GetToken(It.IsAny<string>())).Returns(new Token(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.UtcNow));
        }
    }
}
