using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;

namespace HubOrderPerformance.Steps
{
    [Binding]
    public class HubOrderCountSteps : BaseSteps
    {
        private IWebDriver _driver;
        WebDriverWait wait;
        
        [Given(@"the following user with '(.*)' and '(.*)' wants to navigate to the Hub login page using '(.*)'")]
        public void GivenTheFollowingUserWithAndWantsToNavigateToTheHubLoginPageUsing(string Username, string Password, string Browser)
        {
            _driver = SetWebDriver(Browser);
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl("http://localhost:3000/login");
            IWebElement usernameField = _driver.FindElement(By.Name("username"));
            IWebElement passwordField = _driver.FindElement(By.Name("password"));
            IWebElement loginButton = _driver.FindElement(By.Name("login"));
            usernameField.SendKeys(Username);
            passwordField.SendKeys(Password);
            loginButton.Click();
        }

        [Then(@"the user navigates to the orders page")]
        public void ThenTheUserNavigatesToTheOrdersPage()
        {
            IWebElement ordersButton = _driver.FindElement(By.XPath("//*[@id='root']/div/div[1]/div/nav/section[1]/a[2]"));
            _driver.Navigate().GoToUrl("http://localhost:3000/orders");
        }

        [Then(@"the user gets the number of orders")]
        public void ThenTheUserGetsTheNumberOfOrders()
        {
            //Thread.Sleep(5000); //TODO REPLACE THIS WITH SOMETHING BETTER.. this code makes the program wait long enough to find the element
            IWebElement orderNumber = _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]"));
            wait.Until(_driver => orderNumber.Displayed); //NEW TEST CODE, also above is new
            double getOrderCountParsed = 0, initialOrderCountParsed = 0, finalOrderCountParsed = 0;
            double ordersPerHour, ordersPerMinute;
            Match getOrderCount, initialOrderCount, finalOrderCount;
            initialOrderCount = Regex.Match(_driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]")).Text, @"^\S*\s+(\S+)+\s(\S+)");
            initialOrderCountParsed = Convert.ToDouble(initialOrderCount.Groups[2].Value);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.Elapsed <= TimeSpan.FromMinutes(2) || getOrderCountParsed >= 12500)
            {
                Thread.Sleep(15000);

                getOrderCount = Regex.Match(_driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]")).Text, @"^\S*\s+(\S+)+\s(\S+)");

                if (getOrderCount.Success)
                {
                    getOrderCountParsed = Convert.ToDouble(getOrderCount.Groups[2].Value);
                    File.AppendAllText(@"OrderCount.csv", (DateTime.Now + "," + getOrderCountParsed + " Orders," + Environment.NewLine));
                    _driver.Navigate().Refresh();
                }
                else
                {
                    throw new Exception("Could not get order count from the Hub Orders page");
                }
            }

            stopWatch.Stop();
            //Thread.Sleep(2000); //TODO REPLACE THIS WITH SOMETHING BETTER
            finalOrderCount = Regex.Match(_driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]")).Text, @"^\S*\s+(\S+)+\s(\S+)");
            finalOrderCountParsed = Convert.ToDouble(finalOrderCount.Groups[2].Value);
            ordersPerHour = /*finalOrderCountParsed -*/ initialOrderCountParsed;
            ordersPerMinute = finalOrderCountParsed; //ordersPerHour / 60;
            File.AppendAllText(@"OrderCount.csv", "\nOrders Per Hour: " + ordersPerHour.ToString());
            File.AppendAllText(@"OrderCount.csv", "\nOrders Per Minute: " + Math.Round(ordersPerMinute, 2).ToString());
            File.AppendAllText(@"OrderCount.csv", "\nTotal runtime: " + stopWatch.Elapsed.ToString());
        }
    }
}