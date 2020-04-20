using MyFinances.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core.Dependencies
{
    public interface IUserTokenStore
    {
        void UpdateToken(string userIdentifier, Token token);
        Token GetToken(string userIdentifier);
    }
}
