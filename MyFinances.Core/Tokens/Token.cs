using System;

namespace MyFinances.Core.Tokens
{
    public class Token
    {
        public Token(string accessToken,  string refreshToken, DateTime expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }
        public DateTime ExpiresIn { get; }
        public string RefreshToken { get; }
    }
}