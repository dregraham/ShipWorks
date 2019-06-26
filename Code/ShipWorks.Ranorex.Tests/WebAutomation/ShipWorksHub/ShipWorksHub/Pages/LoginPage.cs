using System;
using System.Threading;
using OpenQA.Selenium;
using Xunit;

namespace ShipWorksHub.Pages
{
    class LoginPage
    {
        private IWebDriver _driver;
        public LoginPage(IWebDriver driver) => _driver = driver;
        public IWebElement UsernameTxtBox => _driver.FindElement(By.Name("username"));
        public IWebElement PasswordTxtBox => _driver.FindElement(By.Name("password"));
        public IWebElement LoginBtn => _driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div[2]/button"));
        protected IWebElement LoggingInSpinner => _driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/div[1]/div[1]"));              
        protected IWebElement DashboardTxt => _driver.FindElement(By.XPath("/html/body/div/div/div[2]/div/header/h1"));        
        public IWebElement ErrorMessage => _driver.FindElement(By.XPath("//*[@id='root']/div/div/div[1]"));

        string LoginPageURL = "https://s2.www.warehouseapp.link/login";

        public DashboardPage LoginAs(string username, string password)
        {
            try
            {
                UsernameTxtBox.SendKeys(username);
                PasswordTxtBox.SendKeys(password);
                LoginBtn.Click();
                return new DashboardPage(_driver);
            }
            catch (Exception e)
            {
                LoginPageQuit();
                throw new Exception("Automation failed at LoginPage.LoginAs method. Exception: " + e);
            }
        }

        public string GetErrorMessage()
        {
            try
            {
                string text = null;
                while (text != "Invalid username or password")
                {
                    Thread.Sleep(250);
                    text = ErrorMessage.Text;
                }

                return ErrorMessage.Text;
            }
            catch (Exception e)
            {
                LoginPageQuit();
                throw new Exception("Automation failed at LoginPage.GetErrorMessage method. Exception: " + e);
            }
        }

        public void LoginPageQuit()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception e)
            {
                throw new Exception("Automation failed at LoginPage.LoginPageQuit method. Exception: " + e);
            }
        }

        public void LoginRedirectVerification()
        {
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/"); //Dashboard page redirects to Login page
            Assert.Equal(LoginPageURL, _driver.Url);

            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/warehouses"); //Warehouses page redirects to Login page
            Assert.Equal(LoginPageURL, _driver.Url);

            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/warehouses/add"); //Warehouse add page redirects to Login page
            Assert.Equal(LoginPageURL, _driver.Url);

            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/settings"); //Settings page redirects to Login page
            Assert.Equal(LoginPageURL, _driver.Url);
        }
    }
}
