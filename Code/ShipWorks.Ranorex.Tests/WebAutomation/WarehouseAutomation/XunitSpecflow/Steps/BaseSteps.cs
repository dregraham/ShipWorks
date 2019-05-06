using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace XunitSpecflow.Steps
{
    public class BaseSteps
    {
        public IWebDriver SetWebDriver(string browser)
        {
            IWebDriver _driver = null;

            switch (browser)
            {
                case "Chrome":
                    ChromeOptions chromeOptions = new ChromeOptions();
                   // chromeOptions.AddArgument("--headless");
                    _driver = new ChromeDriver(Directory.GetCurrentDirectory(), chromeOptions);
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    break;

                case "Firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                   // firefoxOptions.AddArgument("--headless");
                    _driver = new FirefoxDriver(Directory.GetCurrentDirectory(), firefoxOptions);
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    break;

                case "Edge":
                    _driver = new EdgeDriver(Directory.GetCurrentDirectory());
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    break;
            }

            return _driver;
        }


    }
}
