using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyFinances.Tests.E2E
{
    class HeadlessChromeWebDriverProfile : SeleniumWebDriver
    {
        public HeadlessChromeWebDriverProfile(Browser browser) : base(GetDriver(), browser)
        {

        }

        protected HeadlessChromeWebDriverProfile(IWebDriver webDriver, Browser browser) : base(GetDriver(), browser)
        {
        }

        private static IWebDriver GetDriver()
        {
            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless");
            return new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);
        }

    }
}
