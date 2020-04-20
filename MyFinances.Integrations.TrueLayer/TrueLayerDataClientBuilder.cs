using Microsoft.Extensions.Options;
using MyFinances.Core.Dependencies;
using MyFinances.Core.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MyFinances.Integrations.TrueLayer
{
    public class TrueLayerDataClientBuilder: ITrueLayerDataClientBuilder
    {
        private readonly IUserTokenStore userTokenStore;
        private TrueLayerOAuthClientOptions truelayerOAuthClientOptions;

        public TrueLayerDataClientBuilder(IUserTokenStore userTokenStore,
                                IOptionsMonitor<TrueLayerOAuthClientOptions> truelayerOAuthClientOptions)
        {
            this.userTokenStore = userTokenStore;
            this.truelayerOAuthClientOptions = truelayerOAuthClientOptions.CurrentValue;
        }

        public HttpClient Build(User user)
        {
            var token = userTokenStore.GetToken(user.Identifier);
            var client = new HttpClient() { BaseAddress = new Uri(truelayerOAuthClientOptions.ApiUri) };

            // TODO: checks for expired header missing
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return client;
        }
    }
}
