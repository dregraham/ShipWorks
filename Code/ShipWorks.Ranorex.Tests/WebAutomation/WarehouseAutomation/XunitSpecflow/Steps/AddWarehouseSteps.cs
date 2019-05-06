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

        [Given(@"the user wants to navigate to the warehouse page using '(.*)'")]
        public void GivenTheUserWantsToNavigateToTheWarehousePageUsing(string browser)
        {
            _driver = SetWebDriver(browser);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");

            loginPage = new LoginPage(_driver);
            loginPage.LoginAs("user-0801@example.com", "GOOD");

            dashboardPage = new DashboardPage(_driver);
            Assert.Contains("Dashboard", dashboardPage.GetDashboard());
            dashboardPage.ClickWarehouseTab();
        }


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
            warehousesPage.AddWarehouse();
            warehousesPage.WarehousePageQuit();
        }

        [Then(@"the user clicks the cancel button")]
        public void ThenTheUserClicksTheCancelButton()
        {
            warehousesPage.Cancel();
            warehousesPage.WarehousePageQuit();
        }

        [Given(@"the user wants to edit a warehouse using '(.*)'")]
        public void GivenTheUserWantsToEditAWarehouseUsing(string browser)
        {
            _driver = SetWebDriver(browser);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");

            loginPage = new LoginPage(_driver);
            loginPage.LoginAs("user-0801@example.com", "GOOD");
        }

        [Then(@"the user clicks the edit button")]
        public void ThenTheUserClicksTheEditButton()
        {
            warehousesPage = new WarehousesPage(_driver);
            warehousesPage.Edit();
        }

        [Then(@"the user enters new details")]
        public void ThenTheUserEntersNewDetails()
        {
            string[] details = { "Garrett", "4", "214 Main Street", "Kansas Town", "AR", "60000" };
            warehousesPage.EditWarehouseDetails(details);
        }

        [Then(@"the user saves the page")]
        public void ThenTheUserSavesThePage()
        {
            warehousesPage.Save();
            warehousesPage.WarehousePageQuit();
        }

        [Given(@"the user wants to remove a warehouse using '(.*)'")]
        public void GivenTheUserWantsToRemoveAWarehouseUsing(string browser)
        {
            _driver = SetWebDriver(browser);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");

            loginPage = new LoginPage(_driver);
            loginPage.LoginAs("user-0801@example.com", "GOOD");
        }

        [Then(@"the user clicks the remove button")]
        public void ThenTheUserClicksTheRemoveButton()
        {
            warehousesPage = new WarehousesPage(_driver);
            warehousesPage.Remove();
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Accept();
        }

        [Then(@"the user adds more than five hundred characters")]
        public void ThenTheUserAddsMoreThanFiveHundredCharacters()
        {
            string[] details = {
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "AR",
                "60000"
            };
            warehousesPage.AddWarehouseDetails(details);
        }

        [Then(@"the user sees the field validation error messages")]
        public void ThenTheUserSeesTheFieldValidationErrorMessages()
        {
            warehousesPage.AddWarehouse();
            warehousesPage.ValidateFieldLengthErrorMessages();
            warehousesPage.WarehousePageQuit();
        }
    }
}
