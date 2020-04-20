using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient<IUserTokenStore, UserTokenStore>();
        }
    }
}
