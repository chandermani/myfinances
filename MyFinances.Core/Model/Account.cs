using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core.Model
{
    public class Account
    {
        public Account(string identifier, string name, string accountType)
        {
            Identifier = identifier;
            Name = name;
            AccountType = accountType;
        }

        public string Identifier { get; }
        public string Name { get; }
        public string AccountType { get; }
    }
}
