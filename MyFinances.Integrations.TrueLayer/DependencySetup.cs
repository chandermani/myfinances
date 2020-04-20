using Microsoft.Extensions.DependencyInjection;
using MyFinances.Core.Dependencies;
using MyFinances.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Integrations.TrueLayer
{
    public static class DependencySetup
    {
        public static void AddTrueLayerDependencies(this IServiceCollection services)
        {
            services.AddTransient<IBankAuthTokenProvider, BankAuthTokenProvider>();
        }
    }
}
