using Microsoft.Extensions.Options;
using MyFinances.Core.Dependencies;
using MyFinances.Core.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Integrations.TrueLayer
{
    public class BankAuthTokenProvider : IBankAuthTokenProvider
    {
        private readonly TrueLayerOAuthClientOptions truelayerOAuthClientOptions;

        public BankAuthTokenProvider(IOptionsMonitor<TrueLayerOAuthClientOptions> truelayerOAuthClientOptions)
        {
            this.truelayerOAuthClientOptions = truelayerOAuthClientOptions.CurrentValue;
        }

        public async Task<Token> GetAccessTokenAsync(string code)
        {
            HttpClient client = new HttpClient() { BaseAddress = new Uri(truelayerOAuthClientOptions.AuthUri) };
            var response= await client.PostAsync("connect/token", BuildPostParameters(code));
            response.EnsureSuccessStatusCode();

            return MapToToken(await response.Content.ReadAsStringAsync());
        }

        private Token MapToToken(string tokenResponse)
        {
            dynamic token = JsonConvert.DeserializeObject(tokenResponse);
            return new Token(token.access_token.Value, token.refresh_token.Value, DateTime.Now.AddSeconds(token.expires_in.Value));
        }

        public Task<Token> GetAccessTokenWithRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        private FormUrlEncodedContent BuildPostParameters(string code)
        {
            return new FormUrlEncodedContent(new[]
                        {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", truelayerOAuthClientOptions.ClientId),
                new KeyValuePair<string, string>("client_secret", truelayerOAuthClientOptions.ClientSecret),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", truelayerOAuthClientOptions.RedirectUri)

            });
        }
    }
}
