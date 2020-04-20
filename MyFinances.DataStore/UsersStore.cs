using MyFinances.Core.Dependencies;
using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.DataStore
{
    public class UsersStore : IUsersStore
    {
        private Dictionary<string, User> userCache;
        public UsersStore()
        {
            userCache = new Dictionary<string, User>();
            InitializeStoreWithSomeUsers();
        }

        private void InitializeStoreWithSomeUsers()
        {
            userCache["john@doe.com"] = new User("john@doe.com", "john doe", "john@doe.com");
            userCache["john1@doe1.com"] = new User("john1@doe1.com", "john1 doe1", "john1@doe1.com");
            userCache["john2@doe2.com"] = new User("john2@doe2.com", "john2 doe2", "john2@doe2.com");
        }

        public User GetUser(string userIdentifier)
        {
            return userCache.ContainsKey(userIdentifier) ? userCache[userIdentifier] : null;
        }
    }
}