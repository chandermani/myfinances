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
        protected const string CommonUserIdentifier = "john@doe.com";
        protected HttpClient ApiClient;
        protected IConfigurationRoot config;
        protected void SetupInMemoryTestServerAndData()
        {
            var clientFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(config =>
            {
                config.UseEnvironment("E2E");
            });

            ApiClient = clientFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false,
                BaseAddress = new Uri("http://localhost:5000")
            });

            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        }

        protected async Task PersistAccessCode(string userIdentifier)
        {
            var query= QueryHelpers.ParseQuery(GenerateAccessCodeCallbackUrl(userIdentifier).Query);

            var result = await ApiClient.GetAsync($"integration/truelayer/callback?code={query["code"]}&scope={query["scope"]}&state={query["state"]}");

            result.EnsureSuccessStatusCode();
        }

        protected Uri GetAccessCodeResponse(string userIdentifier)
        {
            return GenerateAccessCodeCallbackUrl(userIdentifier);
        }

        private Uri GenerateAccessCodeCallbackUrl(string userIdentifier)
        {
            using (var browser = new BrowserSession(new HeadlessChromeWebDriverProfile(Browser.Chrome)))
            {
                Options options = new Options() { Timeout = TimeSpan.FromSeconds(3) };
                browser.Visit($"{config["TrueLayerRedirectUri"]}&state={userIdentifier}");
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
