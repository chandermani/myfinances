using Microsoft.Extensions.DependencyInjection;
using MyFinances.Core;
using MyFinances.Core.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.DataStore
{
    public static class DependencySetup
    {
        public static void AddDataStoreDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IUserTokenStore, UserTokenStore>();
            services.AddSingleton<IUsersStore, UsersStore>();
            services.AddSingleton<ITransactionStore, TransactionStore>();
        }
    }
}
