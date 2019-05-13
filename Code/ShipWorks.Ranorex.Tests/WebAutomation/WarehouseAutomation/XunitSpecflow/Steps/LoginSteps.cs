using OpenQA.Selenium;
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
        DashboardPage dashboardPage;

        [Given(@"the user is on login page on '(.*)'")]
        public void GivenTheUserIsOnLoginPageOn(string browser)
        {
            _driver = SetWebDriver(browser);
            _driver.Navigate().GoToUrl("http://s2.www.warehouseapp.link/login");
            Assert.Contains("https://s2.www.warehouseapp.link/login", _driver.Url);
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
            dashboardPage = new DashboardPage(_driver);
            Assert.Contains("Dashboard", GetText(dashboardPage.DashboardTxt));
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
            Assert.Contains("Invalid username or password", loginPage.GetErrorMessage());
            loginPage.LoginPageQuit();
        }

        [Then(@"the user clicks logout")]
        public void ThenTheUserClicksLogout()
        {
            dashboardPage = new DashboardPage(_driver);
            dashboardPage.Logout();
        }

        [Then(@"the user validates the browser redirects to the login page from the dashboard, warehouse, settings, and warehouse add pages")]
        public void ThenTheUserValidatesTheBrowserRedirectsToTheLoginPageFromTheDashboardWarehouseSettingsAndWarehouseAddPages()
        {
            loginPage.LoginRedirectVerification();
        }

        [Then(@"the user closes the browser")]
        public void ThenTheUserClosesTheBrowser()
        {
            loginPage.LoginPageQuit();
        }
    }
}

