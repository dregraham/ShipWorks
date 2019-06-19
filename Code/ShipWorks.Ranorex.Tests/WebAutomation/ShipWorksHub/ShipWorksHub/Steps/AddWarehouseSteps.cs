using System;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ShipWorksHub.Pages;
using TechTalk.SpecFlow;
using Xunit;

namespace ShipWorksHub.Steps
{
    [Binding]
    public class AddWarehouseSteps : BaseSteps
    {
        int WarehouseBeforeCount; //used to count the number of warehouses on the warehouse page
        string warehouseName = DateTime.Now.ToString("WMdyyHHmmss");
        string newWarehouseName;
        string warehouseDefault;
        private IWebDriver _driver;
        LoginPage loginPage;
        DashboardPage dashboardPage;
        WarehousesPage warehousesPage;
        AddWarehousePage addWarehousePage;
        EditWarehousePage editWarehousePage;

        [Given(@"the following user with '(.*)' and '(.*)' wants to navigate to the warehouse page using '(.*)'")]
        public void GivenTheFollowingUserWithAndWantsToNavigateToTheWarehousePageUsing(string Username, string Password, string Browser)
        {
            _driver = SetWebDriver(Browser);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");

            loginPage = new LoginPage(_driver);
            dashboardPage = loginPage.LoginAs(Username, Password);

            Assert.Contains("Dashboard", GetText(dashboardPage.DashboardTxt));
            warehousesPage = dashboardPage.ClickWarehouseTab();

            Assert.Contains(warehousesPage.warehousesHeadingText, GetText(warehousesPage.warehousesHeading));
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
            addWarehousePage = new AddWarehousePage(_driver);
        }

        [Then(@"the user adds a warehouse to be sorted")]
        public void ThenTheUserAddsAWarehouseToBeSorted()
        {
            warehouseDefault = warehousesPage.NewWarehouseName(WarehouseBeforeCount);
            string[] details = { warehouseDefault, "123", "1 Memorial Drive", "St. Louis", "MO", "63102" };
            addWarehousePage.AddWarehouseDetails(details);
        }

        [Then(@"the user verifies that the warehouses are sorted correctly")]
        public void ThenTheUserVerifiesThatTheWarehousesAreSortedCorrectly()
        {
            warehousesPage.CheckWarehouseSort();
        }

        [Then(@"the user gets the list of all the warehouses")]
        public void ThenTheUserGetsTheListOfAllTheWarehouses()
        {
            newWarehouseName = warehousesPage.GetListOfWarehouseNames();
        }


        [Then(@"the user verifies that they are back on the settings page")]
        public void ThenTheUserVerifiesThatTheyAreBackOnTheSettingsPage()
        {
            string s = GetText(warehousesPage.CreateWarehouseButton);
            Assert.Equal(warehousesPage.settingsURL, _driver.Url);
        }


        [Then(@"the user adds the Warehouse details")]
        public void ThenTheUserAddsTheWarehouseDetails()
        {
            string[] details = { warehouseName, "123", "1 Memorial Drive", "St. Louis", "MO", "63102" };

            addWarehousePage.AddWarehouseDetails(details);
        }

        [Then(@"the user clicks the cancel button")]
        public void ThenTheUserClicksTheCancelButton()
        {
            addWarehousePage.Cancel();
        }

        [Then(@"the user clicks the cancel button on edit warehouse page")]
        public void ThenTheUserClicksTheCancelButtonOnEditWarehousePage()
        {
            editWarehousePage.Cancel();
        }


        [Then(@"the user clicks the edit button")]
        public void ThenTheUserClicksTheEditButton()
        {
            warehousesPage = new WarehousesPage(_driver);
            warehousesPage.Edit();
            editWarehousePage = new EditWarehousePage(_driver);
        }

        [Then(@"the user enters new details")]
        public void ThenTheUserEntersNewDetails()
        {
            string[] details = { "Garrett", "4", "214 Main Street", "Kansas Town", "AR", "60000" };
            editWarehousePage.EditWarehouseDetails(details);
        }

        [Then(@"the user clicks the save button")]
        public void ThenTheUserClicksTheSaveButton()
        {
            editWarehousePage.Save();
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
            addWarehousePage.AddWarehouseDetails(details);
        }

        [Then(@"the user adds more than five hundred characters on edit warehouse page")]
        public void ThenTheUserAddsMoreThanFiveHundredCharactersOnEditWarehousePage()
        {
            string[] details = {
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "fivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharactersfivehundredcharacters",
                "AR",
                "60000"
            };
            editWarehousePage.EditWarehouseDetails(details);
        }


        [Then(@"the user sees the field validation error messages")]
        public void ThenTheUserSeesTheFieldValidationErrorMessages()
        {
            addWarehousePage.ValidateFieldLengthErrorMessages();
        }

        [Then(@"the user sees the field validation error messages on edit warehouse page")]
        public void ThenTheUserSeesTheFieldValidationErrorMessagesOnEditWarehousePage()
        {
            editWarehousePage.ValidateFieldLengthErrorMessages();
        }


        [Then(@"the user blanks out all fields")]
        public void ThenTheUserBlanksOutAllFields()
        {
            addWarehousePage.BlankOutFields();
        }

        [Then(@"the user blanks out all fields on edit warehouse page")]
        public void ThenTheUserBlanksOutAllFieldsOnEditWarehousePage()
        {
            editWarehousePage.BlankOutFields();
        }


        [Then(@"the user sees empty field error messages")]
        public void ThenTheUserSeesEmptyFieldErrorMessages()
        {
            addWarehousePage.ValidateBlankFieldMessages();
        }

        [Then(@"the user sees empty field error messages on edit warehouse page")]
        public void ThenTheUserSeesEmptyFieldErrorMessagesOnEditWarehousePage()
        {
            editWarehousePage.ValidateBlankFieldMessages();
        }

        [Then(@"the user closes the warehouse page")]
        public void ThenTheUserClosesTheWarehousePage()
        {
            _driver.Quit();
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
            addWarehousePage.AddWarehouseDetails(details);
        }

        [Then(@"the user adds the following Warehouse details '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void ThenTheUserAddsTheFollowingWarehouseDetails(string Street, string City, string State, string Zip)
        {
            string[] details = { warehouseName, null, Street, City, State, Zip };
            addWarehousePage.AddWarehouseDetails(details);
        }

        [Then(@"the user clicks the add warehouse button")]
        public void ThenTheUserClicksTheAddWarehouseButton()
        {
            addWarehousePage.AddWarehouse();
        }

        [Then(@"the user edits the following Warehouse details '(.*)' '(.*)' '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void ThenTheUserEditsTheFollowingWarehouseDetails(string Name, string Code, string Street, string City, string State, string Zip)
        {
            string[] details = { Name, Code, Street, City, State, Zip };
            editWarehousePage.EditWarehouseDetails(details);
        }

        [Then(@"the user edits the following Warehouse details '(.*)' '(.*)' '(.*)' '(.*)'")]
        public void ThenTheUserEditsTheFollowingWarehouseDetails(string Street, string City, string State, string Zip)
        {
            string[] details = { null, null, Street, City, State, Zip };
            editWarehousePage.EditWarehouseDetails(details);
        }


        [Then(@"the user checks if the first warehouse is the default")]
        public void ThenTheUserChecksIfTheFirstWarehouseIsTheDefault()
        {            
            Assert.Equal("DEFAULT", warehousesPage.CheckMarkDefaultText.Text.Trim());
        }

        [Then(@"the user checks if the newly added warehouse is not the default")]
        public void ThenTheUserChecksIfTheNewlyAddedWarehouseIsNotTheDefault()
        {
            Assert.True(warehousesPage.CheckIfNotDefault());
        }

        [Then(@"the user changes and validates the newly added warehouse is the default")]
        public void ThenTheUserChangesAndValidatesTheNewlyAddedWarehouseIsTheDefault()
        {
            string warehouse = warehousesPage.MakeWarehouseDefault("New", warehouseDefault);
            warehouse = warehouse.Replace("button/", "");

            string text = _driver.FindElement(By.XPath(warehouse)).Text;

            while (text != "DEFAULT")
            {
                System.Threading.Thread.Sleep(250);
                text = _driver.FindElement(By.XPath(warehouse)).Text.Trim();
            }

            Assert.Equal("DEFAULT", text.Trim());
        }

        [Then(@"the makes the first warehouse the default and removes the newly added warehouse")]
        public void ThenTheMakesTheFirstWarehouseTheDefaultAndRemovesTheNewlyAddedWarehouse()
        {
            string warehouse = warehousesPage.MakeWarehouseDefault("First", warehouseDefault);
            warehouse = warehouse.Replace("button/", "");

            string text = _driver.FindElement(By.XPath(warehouse)).Text;

            while (text != "DEFAULT")
            {
                System.Threading.Thread.Sleep(250);
                text = _driver.FindElement(By.XPath(warehouse)).Text.Trim();
            }
            
            Assert.Equal("DEFAULT", text.Trim());
        }

        [Then(@"the user navigates to a nonexistent page")]
        public void ThenTheUserNavigatesToANonexistentPage()
        {
            warehousesPage = new WarehousesPage(_driver);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/wareouses/");
        }

        [Then(@"the user verifies they are on the not found page")]
        public void ThenTheUserVerifiesTheyAreOnTheNotFoundPage()
        {
            Assert.Equal("Not found", GetText(warehousesPage.PageNotFoundText));
        }

        [Then(@"the user navigates to a nonexistent warehouse")]
        public void ThenTheUserNavigatesToANonexistentWarehouse()
        {
            warehousesPage = new WarehousesPage(_driver);
            _driver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/warehouses/edit/bea45790-6dcf-11e9-957c-f19e7");
        }

        [Then(@"the user checks to see if they are on the warehouse not found page")]
        public void ThenTheUserChecksToSeeIfTheyAreOnTheWarehouseNotFoundPage()
        {
            string text = null;
            while (text != "Could not find the requested warehouse.")
            {
                System.Threading.Thread.Sleep(250);
                text = GetText(warehousesPage.WarehouseNotFoundText);
            }

            new WebDriverWait(_driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div[2]/div/article/div")));
            Assert.Equal("Could not find the requested warehouse.", warehousesPage.WarehouseNotFoundText.Text.Trim());
        }

        [Then(@"the user adds the Warehouse details for name sorting")]
        public void ThenTheUserAddsTheWarehouseDetailsForNameSorting()
        {
            string[] details2 = { "B", "2", "1 Memorial Drive", "St. Louis", "MO", "63102" };
            addWarehousePage.AddWarehouseDetails(details2);
            addWarehousePage.AddWarehouse();

            warehousesPage.CreateWarehouse();
            string[] details3 = { "C", "3", "1 Memorial Drive", "St. Louis", "MO", "63102" };
            addWarehousePage.AddWarehouseDetails(details3);
            addWarehousePage.AddWarehouse();
        }

        [Then(@"the user checks the sorting of the warehouse")]
        public void ThenTheUserChecksTheSortingOfTheWarehouse()
        {
            Assert.Equal("A", warehousesPage.FirstWarehouseNameText.Text);
            Assert.Equal("B", warehousesPage.SecondWarehouseNameText.Text);
            Assert.Equal("C", warehousesPage.ThirdWarehouseNameText.Text);
        }

        [Then(@"the user deletes all but the first warehouse")]
        public void ThenTheUserDeletesAllButTheFirstWarehouse()
        {
            Type type = _driver.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            string s = props[1].ReflectedType.Name;

            while (_driver.FindElements(By.CssSelector("div.sc-brqgnP")).Count != 1)
            {
                warehousesPage.Remove();
                if (s.Equals("EdgeDriver") || s.Equals("ChromeDriver")) //add an additional delay if any browser but Firefox
                {
                    System.Threading.Thread.Sleep(1000);
                }
                AcceptAlert();
            }
        }
    }
}

