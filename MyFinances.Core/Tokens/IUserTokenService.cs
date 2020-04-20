using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Core.Tokens
{
    public interface IUserTokenService
    {
        Task<Token> GenerateTokenAsync(string userIdentifier, string code);
        Token GetToken(string userIdentifier);
    }
}
