using Coypu;
using Coypu.Drivers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MyFinances.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFinances.Tests.E2E
{
    public class AcceptanceTestsContext
    {
        protected HttpClient ApiClient;
        protected void SetupInMemoryTestServerAndData()
        {
            var clientFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(config =>
            {
                config.UseEnvironment("Development");

                config.ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.E2E.json");
                });
            });

            ApiClient = clientFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false,
                BaseAddress = new Uri("http://localhost:5000")
            });

        }

        protected async Task PersistAccessCode()
        {
            var query= QueryHelpers.ParseQuery(GenerateAccessCodeCallbackUrl().Query);

            var result = await ApiClient.GetAsync($"integration/truelayer/callback?code={query["code"]}&scope={query["scope"]}&state={query["state"]}");
        }

        protected Uri GetAccessCodeResponse()
        {
            return GenerateAccessCodeCallbackUrl();
        }

        private Uri GenerateAccessCodeCallbackUrl()
        {
            using (var browser = new BrowserSession(new HeadlessChromeWebDriverProfile(Browser.Chrome)))
            {
                Options options = new Options() { Timeout = TimeSpan.FromSeconds(3) };
                browser.Visit("https://auth.truelayer-sandbox.com/?response_type=code&client_id=sandbox-chandermani-982283&scope=info%20accounts%20balance%20cards%20transactions%20direct_debits%20standing_orders%20offline_access&redirect_uri=http://localhost:5000/integration/truelayer/callback&providers=uk-ob-all%20uk-oauth-all%20uk-cs-mock&state=john@doe.com");
                browser.FindCss("button[type=submit]", options).Click();
                browser.FindCss("#mock", options).Click();
                browser.FindCss("input[type=text]", options).FillInWith("john");
                browser.FindCss("input[type=password]", options).FillInWith("doe");
                browser.FindCss("button[type=submit]", options).Click();
                // Wait for some time to post the code.
                Thread.Sleep(5000);
                return browser.Location;
            }
        }
    }
}
