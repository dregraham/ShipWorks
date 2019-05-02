﻿using OpenQA.Selenium;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;
using XunitSpecflow.Pages;

namespace XunitSpecflow.Steps
{
    [Binding]
    public class LoginSteps : BaseSteps
    {
        private IWebDriver _driver;
        LoginPage loginPage;

        [Given(@"the user is on login page on '(.*)'")]
        public void GivenTheUserIsOnLoginPageOn(string browser)
        {
            _driver = SetWebDriver(browser);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");            
        }

        [Given(@"the user enters username and password")]
        public void GivenTheUserEntersUsernameAndPassword()
        {
            loginPage = new LoginPage(_driver);
            loginPage.LoginAs("user-0801@example.com", "GOOD");
        }

        [Then(@"the user sees the dashboard")]
        public void ThenTheUserSeesTheDashboard()
        {
            DashboardPage dashboardPage = new DashboardPage(_driver);
            Assert.Contains("Dashboard", dashboardPage.GetDashboard());
            dashboardPage.DashboardQuit();
        }

        [Given(@"the user enters invalid username and password")]
        public void GivenTheUserEntersInvalidUsernameAndPassword()
        {
            loginPage = new LoginPage(_driver);
            loginPage.LoginAs("BadUsername", "GOOD");
        }

        [Then(@"the user sees the error message")]
        public void ThenTheUserSeesTheErrorMessage()
        {
            Thread.Sleep(2000);
            Assert.Contains("Invalid username or password", loginPage.GetErrorMessage());
            loginPage.LoginPageQuit(_driver);
        }
    }
}

