using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core.Model
{
    public class User
    {
        public User(string identifier, string name, string email)
        {
            Identifier = identifier;
            Name = name;
            Email = email;
        }

        public string Identifier { get; }
        public string Name { get; }
        public string Email { get; }
    }
}
