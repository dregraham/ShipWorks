using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace ShipWorksHub.Steps
{
    public class BaseSteps
    {
        IAlert alert;
        IWebDriver _driver;
        public IWebDriver SetWebDriver(string browser)
        {
            switch (browser)
            {
                case "Chrome":
                    ChromeOptions chromeOptions = new ChromeOptions();
                    //chromeOptions.AddArgument("--headless");
                    _driver = new ChromeDriver(Directory.GetCurrentDirectory(), chromeOptions);
                    _driver.Manage().Window.Maximize();
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    break;

                case "Firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    //firefoxOptions.AddArgument("--headless");
                    _driver = new FirefoxDriver(Directory.GetCurrentDirectory(), firefoxOptions);
                    _driver.Manage().Window.Maximize();
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    break;

                case "Edge":
                    _driver = new EdgeDriver(Directory.GetCurrentDirectory());
                    _driver.Manage().Window.Maximize();
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    break;
            }

            return _driver;
        }
        public string GetText(IWebElement element)
        {
            try
            {
                System.Threading.Thread.Sleep(350); //waiting for driver to hand off control of element
                return element.Text;
            }
            catch (Exception e)
            {
                _driver.Quit();
                throw new Exception("Automation failed at BaseSteps.GetText method. Exception: " + e);
            }
        }
        public void DisposeWebDriver()
        {
            System.Threading.Thread.Sleep(1000);
            _driver.Quit();
        }
        public int GetCount(string identifier)
        {
            return _driver.FindElements(By.CssSelector(identifier)).Count;
        }

        public void AcceptAlert()
        {
            alert = _driver.SwitchTo().Alert();
            alert.Accept();
        }
        public void DismissAlert()
        {
            alert = _driver.SwitchTo().Alert();
            alert.Dismiss();
        }
    }
}
