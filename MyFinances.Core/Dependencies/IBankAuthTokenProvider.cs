using MyFinances.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Core.Dependencies
{
    public interface IBankAuthTokenProvider
    {
        Task<Token> GetAccessTokenAsync(string code);
        Task<Token> GetAccessTokenWithRefreshToken(string refreshToken);
    }
}
