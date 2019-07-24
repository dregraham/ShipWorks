using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            File.Delete(@"HubPerformanceResults.csv");

            _driver = SetWebDriver(Browser);
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl("https://s2.www.s2hub.link/login");
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
            _driver.Navigate().GoToUrl("https://s2.www.s2hub.link/orders");
        }

        [Then(@"the user gets the number of orders")]
        public void ThenTheUserGetsTheNumberOfOrders()
        {
            IWebElement orderNumber = _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]"));

            List<string> recordList = new List<string>();
            List<double> average = new List<double>();

            recordList.Add("Timestamp,Order Count,Orders/Minute,Average Orders/min,Orders/hour");
            WaitUntilVisible("50 rows", _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[2]"))); //using this to wait for elements on the page
            double getOrderCountParsed = 0, initialOrderCountParsed = 0, finalOrderCountParsed = 0, previousOrderCountParsed = 0;
            double ordersPerHour, ordersPerMinute;
            Match getOrderCount, initialOrderCount, finalOrderCount;
            initialOrderCount = Regex.Match(_driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]")).Text, @"^\S*\s+(\S+)+\s(\S+)");
            initialOrderCountParsed = Convert.ToDouble(initialOrderCount.Groups[2].Value);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (true)
            {
                Thread.Sleep(60000);

                getOrderCount = Regex.Match(_driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]")).Text, @"^\S*\s+(\S+)+\s(\S+)");

                if (getOrderCount.Success)
                {
                    getOrderCountParsed = Convert.ToDouble(getOrderCount.Groups[2].Value);
                    recordList.Add($"{DateTime.Now},{getOrderCountParsed},{getOrderCountParsed - previousOrderCountParsed}");
                    average.Add(getOrderCountParsed - previousOrderCountParsed);
                    previousOrderCountParsed = getOrderCountParsed;
                    stopWatch.Stop();
                    _driver.Navigate().Refresh();
                    stopWatch.Start();
                    if (stopWatch.Elapsed >= TimeSpan.FromMinutes(60) || getOrderCountParsed >= 12500)
                    {
                        break;
                    }
                }
                else
                {
                    throw new Exception("Could not get order count from the Hub Orders page");
                }
            }

            stopWatch.Stop();
            WaitUntilVisible("50 rows", _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[2]"))); //using this to wait for elements on the page
            finalOrderCount = Regex.Match(_driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/table/tfoot/tr/td/div/div[3]/span[3]")).Text, @"^\S*\s+(\S+)+\s(\S+)");
            finalOrderCountParsed = Convert.ToDouble(finalOrderCount.Groups[2].Value);
            ordersPerMinute = Math.Round(average.Sum() / average.Count, 2);
            ordersPerHour = ordersPerMinute * 60;
            recordList[1] = recordList[1] + $",{ordersPerMinute},{ordersPerHour}";

            File.WriteAllLines(@"HubPerformanceResults.csv", recordList);
        }
    }
}