using MyFinances.Core.Dependencies;
using System.Threading.Tasks;

namespace MyFinances.Core.Tokens
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IUserTokenStore userTokenStore;
        private readonly IBankAuthTokenProvider bankAuthTokenProvider;

        public UserTokenService(IUserTokenStore userTokenStore, IBankAuthTokenProvider bankAuthTokenProvider)
        {
            this.userTokenStore = userTokenStore;
            this.bankAuthTokenProvider = bankAuthTokenProvider;
        }

        public async Task<Token> GenerateTokenAsync(string userIdentifier, string code)
        {
            var token = await bankAuthTokenProvider.GetAccessToken(code);
            userTokenStore.UpdateToken(userIdentifier, token);
            return token;
        }

        public Token GetToken(string userIdentifier)
        {
            return userTokenStore.GetToken(userIdentifier);
        }
    }
}
