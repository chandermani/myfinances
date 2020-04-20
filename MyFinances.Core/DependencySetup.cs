using Microsoft.Extensions.DependencyInjection;
using MyFinances.Core.Importer;
using MyFinances.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core
{
    public static class DependencySetup
    {
        public static void AddCoreDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUserTokenService, UserTokenService>();
            services.AddTransient<ITransactionsImporter, TransactionsImporter>();
            services.AddTransient<IClock, Clock>();
        }
    }
}
