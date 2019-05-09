using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Xunit;
using XunitSpecflow.Pages;

namespace XunitSpecflow.Steps
{
    [Binding]
    public class AddWarehouseSteps : BaseSteps
    {
        int WarehouseBeforeCount; //used to count the number of warehouses on the warehouse page
        private IWebDriver _driver;
        LoginPage loginPage;
        DashboardPage dashboardPage;
        WarehousesPage warehousesPage;

        [Given(@"the following user with '(.*)' and '(.*)' wants to navigate to the warehouse page using '(.*)'")]
        public void GivenTheFollowingUserWithAndWantsToNavigateToTheWarehousePageUsing(string Username, string Password, string Browser)
        {
            
                _driver = SetWebDriver(Browser);
                _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");

                loginPage = new LoginPage(_driver);
                loginPage.LoginAs(Username, Password);

                dashboardPage = new DashboardPage(_driver);
                Assert.Contains("Dashboard", GetText(dashboardPage.DashboardTxt));
                dashboardPage.ClickWarehouseTab();
        }


        [Then(@"the user clicks on the Warehouses tab")]
        public void ThenTheUserClicksOnTheWarehousesTab()
        {
            dashboardPage = new DashboardPage(_driver);
            Assert.Contains("Dashboard", GetText(dashboardPage.DashboardTxt));
            dashboardPage.ClickWarehouseTab();
        }

        [Then(@"the user clicks the add button")]
        public void ThenTheUserClicksTheAddButton()
        {
            warehousesPage = new WarehousesPage(_driver);
            WarehouseBeforeCount = GetCount(warehousesPage.WarehouseList);
            warehousesPage.CreateWarehouse();
        }

        [Then(@"the user adds the Warehouse details")]
        public void ThenTheUserAddsTheWarehouseDetails()
        {
            string[] details = { "Garrett", "123", "1 Memorial Drive", "St. Louis", "MO", "63102" };

            warehousesPage.AddWarehouseDetails(details);
        }

        [Then(@"the user clicks the cancel button")]
        public void ThenTheUserClicksTheCancelButton()
        {
            warehousesPage.Cancel();
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

        [Then(@"the user clicks the save button")]
        public void ThenTheUserClicksTheSaveButton()
        {
            warehousesPage.Save();
        }


        [Then(@"the user clicks the remove button")]
        public void ThenTheUserClicksTheRemoveButton()
        {
            warehousesPage = new WarehousesPage(_driver);
            warehousesPage.Remove();
        }

        [Then(@"the user accepts the remove warehouse confirmation")]
        public void ThenTheUserAcceptsTheRemoveWarehouseConfirmation()
        {
            AcceptAlert();
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

        [Then(@"the user blanks out all fields")]
        public void ThenTheUserBlanksOutAllFields()
        {
            warehousesPage.BlankOutFields();
        }

        [Then(@"the user sees empty field error messages")]
        public void ThenTheUserSeesEmptyFieldErrorMessages()
        {
            warehousesPage.ValidateBlankFieldMessages();
        }

        [Then(@"the user closes the warehouse page")]
        public void ThenTheUserClosesTheWarehousePage()
        {
            DisposeWebDriver();
        }

        [Then(@"the user cancels the remove warehouse action")]
        public void ThenTheUserCancelsTheRemoveWarehouseAction()
        {
            DismissAlert();
        }

        [Then(@"the user checks to see if the warehouse is still on the list")]
        public void ThenTheUserChecksToSeeIfTheWarehouseIsStillOnTheList()
        {
            Assert.Equal("Do Not Remove", GetText(warehousesPage.DoNotRemoveWareHouseName));
        }

        [Then(@"the user verifies that no warehouse was added")]
        public void ThenTheUserVerifiesThatNoWarehouseWasAdded()
        {
            Assert.Equal(WarehouseBeforeCount, GetCount(warehousesPage.WarehouseList));
        }
        [Then(@"the user verifies that no fields were updated")]
        public void ThenTheUserVerifiesThatNoFieldsWereUpdated()
        {
            warehousesPage.CompareWarehouseDetails();
        }

        [Then(@"the user adds the following Warehouse details '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void ThenTheUserAddsTheFollowingWarehouseDetails(string Name, string Code, string Street, string City, string State, string Zip)
        {
            string[] details = { Name, Code, Street, City, State, Zip };
            warehousesPage.AddWarehouseDetails(details);
        }

        [Then(@"the user clicks the add warehouse button")]
        public void ThenTheUserClicksTheAddWarehouseButton()
        {
            warehousesPage.AddWarehouse();
        }

        [Then(@"the user edits the following Warehouse details '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void ThenTheUserEditsTheFollowingWarehouseDetails(string Name, string Code, string Street, string City, string State, string Zip)
        {
            string[] details = { Name, Code, Street, City, State, Zip };
            warehousesPage.EditWarehouseDetails(details);
        }

        [Then(@"the user checks if the first warehouse is the default")]
        public void ThenTheUserChecksIfTheFirstWarehouseIsTheDefault()
        {
            warehousesPage = new WarehousesPage(_driver);
            Assert.Equal("DEFAULT", warehousesPage.OnlyDefaultText.Text.Trim());
        }

        [Then(@"the user checks if the newly added warehouse is not the default")]
        public void ThenTheUserChecksIfTheNewlyAddedWarehouseIsNotTheDefault()
        {
            Assert.Equal("DEFAULT", warehousesPage.FirstDefaultText.Text.Trim());
        }

        [Then(@"the user changes and validates the newly added warehouse is the default")]
        public void ThenTheUserChangesAndValidatesTheNewlyAddedWarehouseIsTheDefault()
        {
            warehousesPage.MakeWarehouseDefault(warehousesPage.SecondDefaultButton);

            new WebDriverWait(_driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div[2]/div/article/div[1]/div/div/div[3]/button/div")));
            Assert.Equal("DEFAULT", warehousesPage.SecondDefaultText.Text.Trim());
        }

        [Then(@"the makes the first warehouse the default and removes the newly added warehouse")]
        public void ThenTheMakesTheFirstWarehouseTheDefaultAndRemovesTheNewlyAddedWarehouse()
        {
            warehousesPage.MakeWarehouseDefault(warehousesPage.FirstDefaultButton);
            new WebDriverWait(_driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div[2]/div/article/div[2]/div/div/div[3]/button/div")));

            warehousesPage.Remove();
            System.Threading.Thread.Sleep(250);

            AcceptAlert();
            System.Threading.Thread.Sleep(1000);
        }
    }
}

