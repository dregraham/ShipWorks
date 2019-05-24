using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using Xunit;
using System;
using System.IO;
using WarehouseWebAutomation.Variables;
using OpenQA.Selenium.Support.UI;

namespace WarehouseWebAutomation
{
    static class AutomationMethods 
    {
        static IWebDriver _webDriver;
        static string screenshotFolder = null;

        public static void SetWebDriver(string WebBrowser)
        {
            switch (WebBrowser)
            {
                case "Firefox":

                    //  SET FIREFOX TO ACCEPT INSECURE CERTIFICATES
                    FirefoxOptions firefoxOptions = new FirefoxOptions
                    {
                    //    AcceptInsecureCertificates = true
                    };

                    _webDriver = new FirefoxDriver(WarehouseVariables.WebDriverPath, firefoxOptions);

                    break;

                case "Chrome":

                   // SET CHROME TO ACCEPT INSECURE CERTIFICATES
                    ChromeOptions chromeOptions = new ChromeOptions
                    {
                     //   AcceptInsecureCertificates = true
                    };

                    _webDriver = new ChromeDriver(WarehouseVariables.WebDriverPath, chromeOptions);

                    break;

                case "Edge":

                    //  SET EDGE TO ACCEPT INSECURE CERTIFICATES
                    EdgeOptions edgeOptions = new EdgeOptions
                    {
                      //  AcceptInsecureCertificates = true
                    };

                    _webDriver = new EdgeDriver(WarehouseVariables.WebDriverPath, edgeOptions);

                    break;
            }
        }

        public static void SetScreenshotFolder()
        {
            screenshotFolder = DateTime.Now.ToString("MMddyyhhmm");
            Directory.CreateDirectory(WarehouseVariables.ScreenshotPath + screenshotFolder);
        }

        public static void Launch(string url, string WebBrowser) //launch the respective web browser at the designated URL
        {
            try
            {
                SetWebDriver(WebBrowser);
                SetScreenshotFolder();
                _webDriver.Navigate().GoToUrl(url);
                _webDriver.Manage().Window.Maximize();
            }
            catch (Exception e)
            {
                throw new Exception("Failed at AutomationMethods.Launch: " + e);
            }
        }

        public static void Click(string xpath) //click on an element on the web page
        {
            try
            {
                _webDriver.FindElement(By.XPath(xpath)).Click();
            }
            catch (Exception e)
            {
                throw new Exception("Failed at AutomationMethods.Click: " + e);
            }
        }

        public static void Type(string value, string xpath) //type into a field/element on a web page
        {
            try
            {
                IWebElement textField = _webDriver.FindElement(By.XPath(xpath));
                textField.SendKeys(value);
            }
            catch (Exception e)
            {
                throw new Exception("Failed at AutomationMethods.Type: " + e);
            }
        }

        public static void ValidateIfExists(string expected, string actual) //compare the 'actual' text on the web page to the text you specify in the 'expected' variable
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch (Exception e)
            {
                throw new Exception("Failed at AutomationMethods.ValidateIfExists: " + e);
            }
        }

        public static string GetText(string xpath) //grab the text at the designated element on the web page
        {            
            string text = null;

            try
            {
                WaitUntilExists(xpath);
                text = _webDriver.FindElement(By.XPath(xpath)).Text;
            }
            catch (Exception e)
            {
                throw new Exception("Failed at AutomationMethods.GetText: " + e);
            }
            return text;
        }

        public static void SaveScreenshot(string imageName)
        {
            Screenshot screenshot = ((ITakesScreenshot) _webDriver).GetScreenshot();
            screenshot.SaveAsFile(WarehouseVariables.ScreenshotPath + screenshotFolder + "\\" + imageName + ".png", ScreenshotImageFormat.Png);
        }

        public static void Dispose() //quits the webdriver and closes out of the respective web browser
        {
            try
            {
                _webDriver.Quit();
            }
            catch (Exception e)
            {
                throw new Exception("Failed at AutomationMethods.Dispose: " + e);
            }
        }
        public static void WaitUntilExists(string xpath)
        {
            int TimeOut = 60;
            WebDriverWait Wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(TimeOut));
            Wait.Until(drv => drv.FindElement(By.XPath(xpath)));
        }
    }
}
