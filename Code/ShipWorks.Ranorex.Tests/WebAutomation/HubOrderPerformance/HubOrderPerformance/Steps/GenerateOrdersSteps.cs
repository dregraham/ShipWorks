using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace HubOrderPerformance.Steps
{
    [Binding]
    public class GenerateOrdersSteps : BaseSteps
    {
        private IWebDriver _driver;

        [Given(@"the following user with '(.*)' and '(.*)' wants to navigate to the fake stores login page using '(.*)'")]
        public void GivenTheFollowingUserWithAndWantsToNavigateToTheFakeStoresLoginPageUsing(string Username, string Password, string Browser)
        {
            _driver = SetWebDriver(Browser);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Navigate().GoToUrl("http://localhost:4004/ui/login");
            IWebElement usernameField = _driver.FindElement(By.Name("username"));
            IWebElement passwordField = _driver.FindElement(By.Name("password"));
            IWebElement loginButton = _driver.FindElement(By.XPath("/ html / body / form / div[3] / input"));
            usernameField.SendKeys(Username);
            passwordField.SendKeys(Password);
            loginButton.Click();
        }

        [Then(@"the user navigates to the generate page at '(.*)'")]
        public void ThenTheUserNavigatesToTheGeneratePageAt(string StoreURL)
        {
            _driver.Navigate().GoToUrl(StoreURL);
        }

        [Then(@"the user generates '(.*)' of orders")]
        public void ThenTheUserGeneratesOfOrders(int Number)
        {
            var date = DateTime.Now;
            IWebElement numberOfOrdersField = _driver.FindElement(By.Name("numberOfOrders"));
            IWebElement startTime = _driver.FindElement(By.Name("startTime"));
            IWebElement endTime = _driver.FindElement(By.Name("endTime"));
            IWebElement generateButton = _driver.FindElement(By.XPath("/html/body/form[2]/div/input"));
            startTime.Clear();
            endTime.Clear();
            numberOfOrdersField.Clear();
            startTime.SendKeys($"{ date.Hour}{date.Minute}{date.ToString("tt")}");

            if (date.Hour == 11 && date.ToString("tt") == "AM")
            {
                endTime.SendKeys($"{date.Hour + 1}{date.Minute}PM");
            }
            else if (date.Hour == 11 && date.ToString("tt") == "PM")
            {
                endTime.SendKeys($"{date.Hour + 1}{date.Minute}AM");
            }
            else
            {
                endTime.SendKeys($"{date.Hour + 1}{date.Minute}{date.ToString("tt")}");
            }

            numberOfOrdersField.SendKeys(Number.ToString());
            //generateButton.Click(); //commented out to not accidentally generate orders
            _driver.Close();
        }
    }
}
