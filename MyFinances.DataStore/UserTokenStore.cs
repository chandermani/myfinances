using MyFinances.Core.Dependencies;
using MyFinances.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.DataStore
{
    public class UserTokenStore : IUserTokenStore
    {
        Dictionary<string, Token> tokenCache;
        public UserTokenStore()
        {
            tokenCache = new Dictionary<string, Token>();
        }

        public Token GetToken(string userIdentifier)
        {
            return tokenCache.ContainsKey(userIdentifier) ? null : tokenCache[userIdentifier];
        }

        public void UpdateToken(string userIdentifier, Token token)
        {
            tokenCache[userIdentifier] = token;
        }
    }
}
