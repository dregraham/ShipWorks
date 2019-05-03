using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;

namespace XunitSpecflow.Pages
{
    class LoginPage
    {
        [FindsBy(How = How.Name, Using = "username")]
        protected IWebElement UsernameTxtBox { get; set; }

        [FindsBy(How = How.Name, Using = "password")]
        protected IWebElement PasswordTxtBox { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div/div/div[2]/button")]
        protected IWebElement LoginBtn { get; set; }

        [FindsBy(How = How.Id, Using = "/html/body/div/div/div/div[2]/div[1]/div[1]")]
        protected IWebElement LoggingInSpinner { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div/div/div[2]/div/header/h1")]
        protected IWebElement DashboardTxt { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div/div[1]")]
        public IWebElement ErrorMessage { get; set; }

        private readonly IWebDriver _driver;


        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public DashboardPage LoginAs(string username, string password)
        {
            UsernameTxtBox.SendKeys(username);
            PasswordTxtBox.SendKeys(password);
            LoginBtn.Click();
            return new DashboardPage(_driver);
        }

        public string GetErrorMessage()
        {
            try
            {
                Thread.Sleep(3000);

                return ErrorMessage.Text;
            }
            catch (Exception e)
            {
                _driver.Quit();
            }
            return ErrorMessage.Text;
        }

        public void LoginPageQuit()
        {
            _driver.Quit();
        }
    }
}
