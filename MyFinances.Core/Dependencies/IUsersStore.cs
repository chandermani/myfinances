using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core.Dependencies
{
    public interface IUsersStore
    {
        User GetUser(string userIdentifier);
    }
}
