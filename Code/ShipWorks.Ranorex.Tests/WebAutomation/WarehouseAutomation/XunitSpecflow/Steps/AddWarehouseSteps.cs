using System;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Xunit;
using XunitSpecflow.Pages;

namespace XunitSpecflow.Steps
{
    [Binding]
    public class AddWarehouseSteps : BaseSteps
    {
        private IWebDriver _driver;
        LoginPage loginPage;
        DashboardPage dashboardPage;
        WarehousesPage warehousesPage;

        [Given(@"the user wants to add a warehouse using '(.*)'")]
        public void GivenTheUserWantsToAddAWarehouseUsing(string browser)
        {
            _driver = SetWebDriver(browser);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");

            loginPage = new LoginPage(_driver);
            loginPage.LoginAs("user-0801@example.com", "GOOD");
        }

        [Then(@"the user clicks on the Warehouses tab")]
        public void ThenTheUserClicksOnTheWarehousesTab()
        {
            dashboardPage = new DashboardPage(_driver);
            Assert.Contains("Dashboard", dashboardPage.GetDashboard());
            dashboardPage.ClickWarehouseTab();
        }

        [Then(@"the user clicks the add button")]
        public void ThenTheUserClicksTheAddButton()
        {
            warehousesPage = new WarehousesPage(_driver);
            warehousesPage.CreateWarehouse();
        }

        [Then(@"the user adds the Warehouse details")]
        public void ThenTheUserAddsTheWarehouseDetails()
        {
            string[] details = { "Garrett", "123", "1 Memorial Drive", "St. Louis", "MO", "63102" };
            warehousesPage.AddWarehouseDetails(details);
            // warehousesPage.AddWarehouse();
            warehousesPage.WarehousePageQuit();
        }

        [Then(@"the user clicks the cancel button")]
        public void ThenTheUserClicksTheCancelButton()
        {
            warehousesPage.Cancel();
            warehousesPage.WarehousePageQuit();
        }
    }
}
