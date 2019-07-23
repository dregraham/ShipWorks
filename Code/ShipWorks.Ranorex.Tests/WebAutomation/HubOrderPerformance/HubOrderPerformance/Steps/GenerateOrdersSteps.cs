using OpenQA.Selenium;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace HubOrderPerformance.Steps
{
    [Binding]
    public class GenerateOrdersSteps : BaseSteps
    {
        private IWebDriver _driver;
        string storeURL;

        [Given(@"the following user with '(.*)' and '(.*)' wants to navigate to the fake stores login page using '(.*)'")]
        public void GivenTheFollowingUserWithAndWantsToNavigateToTheFakeStoresLoginPageUsing(string Username, string Password, string Browser)
        {
            _driver = SetWebDriver(Browser);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Navigate().GoToUrl("https://master.fake-stores.warehouseapp.link/ui/login");
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
            storeURL = StoreURL;
        }

        [Then(@"the user generates '(.*)' number of orders for '(.*)' number of batches")]
        public void ThenTheUserGeneratesNumberOfOrdersForNumberOfBatches(int BatchSize, int BatchIteration)
        {
            int monthNumber = 7, dayOfMonth = 1;
            for (int i = 0; i < BatchIteration; i++)
            {
                var date = DateTime.Now;
                IWebElement numberOfOrdersField = _driver.FindElement(By.Name("numberOfOrders"));
                IWebElement startDate = _driver.FindElement(By.XPath("/html/body/form[2]/label[1]/input[1]"));
                IWebElement endDate = _driver.FindElement(By.XPath("/html/body/form[2]/label[2]/input[1]"));
                IWebElement startTime = _driver.FindElement(By.Name("startTime"));
                IWebElement endTime = _driver.FindElement(By.Name("endTime"));
                IWebElement generateButton = _driver.FindElement(By.XPath("/html/body/form[2]/div/input"));
                numberOfOrdersField.Clear();
                startDate.Clear();
                endDate.Clear();

                if (dayOfMonth < 10)
                {
                    startDate.SendKeys($"0{monthNumber}0{dayOfMonth}2019");
                    endDate.SendKeys($"0{monthNumber}0{dayOfMonth}2019");
                }
                else if (dayOfMonth >= 10 && dayOfMonth <= 28)
                {
                    startDate.SendKeys($"0{monthNumber}{dayOfMonth}2019");
                    endDate.SendKeys($"0{monthNumber}{dayOfMonth}2019");
                }
                else if (dayOfMonth > 28)
                {
                    monthNumber++;
                    dayOfMonth = 1;
                    startDate.SendKeys($"0{monthNumber}0{dayOfMonth}2019");
                    endDate.SendKeys($"0{monthNumber}0{dayOfMonth}2019");
                }
                dayOfMonth++;

                //startTime.Clear();
                //endTime.Clear();
                //startTime.SendKeys($"{ date.Hour}{date.Minute}{date.ToString("tt")}");

                //if (date.Hour == 11 && date.ToString("tt") == "AM")
                //{
                //    endTime.SendKeys($"{date.Hour + 1}{date.Minute}PM");
                //}
                //else if (date.Hour == 11 && date.ToString("tt") == "PM")
                //{
                //    endTime.SendKeys($"{date.Hour + 1}{date.Minute}AM");
                //}
                //else
                //{
                //    endTime.SendKeys($"{date.Hour + 1}{date.Minute + 10}{date.ToString("tt")}");
                //}

                numberOfOrdersField.SendKeys(BatchSize.ToString());
                generateButton.Click(); //commented out to not accidentally generate orders
                WaitUntilVisible("Unique Identifier", _driver.FindElement(By.XPath("/html/body/table/thead/tr/th[3]"))); //using this to wait for elements on the page
                Thread.Sleep(10000);
                _driver.Navigate().GoToUrl(storeURL);
            }
            _driver.Close();
        }
    }
}
