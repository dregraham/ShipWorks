using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace XunitSpecflow.Pages
{
    class WarehousesPage
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/header/div/button")]
        protected IWebElement CreateWarehouseButton { get; set; }

        [FindsBy(How = How.Name, Using = "name")]
        protected IWebElement NameTxt { get; set; }

        [FindsBy(How = How.Name, Using = "code")]
        protected IWebElement CodeTxt { get; set; }

        [FindsBy(How = How.Name, Using = "street")]
        protected IWebElement StreetTxt { get; set; }

        [FindsBy(How = How.Name, Using = "city")]
        protected IWebElement CityTxt { get; set; }

        [FindsBy(How = How.Name, Using = "zip")]
        protected IWebElement ZipTxt { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[1]/label[5]/select")]
        protected IWebElement StateDropDown { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/button[2]/div")]
        protected IWebElement AddWarehouseButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/button[1]/div")]
        protected IWebElement CancelButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/div/nav/div[1]/button/div")]
        protected IWebElement EditButton { get; set; }

        [FindsBy(How = How.Name, Using = "saveWarehouse")]
        protected IWebElement SaveButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/div/nav/div[2]/button/div")]
        protected IWebElement RemoveButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/label[1]/span[2]")]
        protected IWebElement NameCharacterLimitMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/label[2]/span[2]")]
        protected IWebElement CodeCharacterLimitMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/label[3]/span[2]")]
        protected IWebElement StreetCharacterLimitMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/label[4]/span[2]")]
        protected IWebElement CityCharacterLimitMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/label[5]/span[2]")]
        protected IWebElement StateBlankMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/label[6]/span[2]")]
        protected IWebElement ZipRequiredMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/label[6]/span[3]")]
        protected IWebElement ZipInvalidMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/header/h2")]
        public IWebElement DoNotRemoveWareHouseName { get; set; }

        public string WarehouseList = "div.sc-brqgnP"; //css selector for warehouse list

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/div/div/div[1]")]
        public IWebElement DoNotRemoveWarehouseTwoStreetName { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/div/div/div[2]")]
        public IWebElement DoNotRemoveWarehouseTwoCityStateZipName { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/header/div")]
        public IWebElement DoNotRemoveWarehouseTwoCodeName { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div/div/div[3]/div")]
        public IWebElement OnlyDefaultText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[1]/div/div/div[3]/div")]
        public IWebElement FirstDefaultText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/div/div/div[3]/div")]
        public IWebElement SecondDefaultText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/div/div/div[3]/button/div")]
        public IWebElement SecondDefaultButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[1]/div/div/div[3]/button/div")]
        public IWebElement FirstDefaultButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/header/h1")]
        public IWebElement PageNotFoundText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div")]
        public IWebElement WarehouseNotFoundText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[1]/header/h2")]
        public IWebElement FirstWarehouseNameText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[2]/header/h2")]
        public IWebElement SecondWarehouseNameText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div[3]/header/h2")]
        public IWebElement ThirdWarehouseNameText { get; set; }
        
        
        IWebDriver _driver;

        public WarehousesPage(IWebDriver driver)
        {
            try
            {
                _driver = driver;
                PageFactory.InitElements(_driver, this);
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.WarehousesPage method. Exception: " + e);
            }
        }
        public void CreateWarehouse()
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div[2]/div/header/div/button")));
                CreateWarehouseButton.Click();
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.CreateWarehouse method. Exception: " + e);
            }
        }

        public void AddWarehouseDetails(string[] details)
        {
            try
            {
                var StateName = new SelectElement(StateDropDown);
                NameTxt.SendKeys(details[0]);
                CodeTxt.SendKeys(details[1]);
                StreetTxt.SendKeys(details[2]);
                CityTxt.SendKeys(details[3]);
                StateName.SelectByValue(details[4]);
                ZipTxt.SendKeys(details[5]);
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.AddWarehouseDetails method. Exception: " + e);
            }
        }

        public void AddWarehouse()
        {
            try
            {
                AddWarehouseButton.Click();
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.AddWarehouse method. Exception: " + e);
            }
        }

        public void Cancel()
        {
            try
            {
                CancelButton.Click();
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.Cancel method. Exception: " + e);
            }
        }
        public void WarehousePageQuit()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception e)
            {
                throw new Exception("Automation failed at WarehousePage.WarehousePageQuit method. Exception: " + e);
            }
        }

        public void Edit()
        {
            try
            {
                EditButton.Click();
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.Edit method. Exception: " + e);
            }
        }

        public void EditWarehouseDetails(string[] details)
        {
            try
            {
                var StateName = new SelectElement(StateDropDown);

                NameTxt.Clear();
                NameTxt.SendKeys(details[0]);
                CodeTxt.Clear();
                CodeTxt.SendKeys(details[1]);
                StreetTxt.Clear();
                StreetTxt.SendKeys(details[2]);
                CityTxt.Clear();
                CityTxt.SendKeys(details[3]);
                ZipTxt.Clear();
                ZipTxt.SendKeys(details[5]);
                StateName.SelectByValue(details[4]);
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.EditWarehouseDetails method. Exception: " + e);
            }
        }

        public void Save()
        {
            try
            {
                SaveButton.Click();
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.Save method. Exception: " + e);
            }
        }

        public void Remove()
        {
            try
            {
                RemoveButton.Click();
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.Remove method. Exception: " + e);
            }
        }

        public void ValidateFieldLengthErrorMessages()
        {
            try
            {
                Assert.Contains("Name cannot be longer than 500 characters", NameCharacterLimitMessage.Text);
                Assert.Contains("Code cannot be longer than 500 characters", CodeCharacterLimitMessage.Text);
                Assert.Contains("Street cannot be longer than 500 characters", StreetCharacterLimitMessage.Text);
                Assert.Contains("City cannot be longer than 500 characters", CityCharacterLimitMessage.Text);
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.ValidateFieldLengthErrorMessages method. Exception: " + e);
            }
        }

        public void ValidateBlankFieldMessages()
        {
            try
            {
                Assert.Contains("Name is required", NameCharacterLimitMessage.Text);
                Assert.Contains("Code is required", CodeCharacterLimitMessage.Text);
                Assert.Contains("Street is required", StreetCharacterLimitMessage.Text);
                Assert.Contains("City is required", CityCharacterLimitMessage.Text);
                Assert.Contains("State is required", StateBlankMessage.Text);
                Assert.Contains("Zip is required", ZipRequiredMessage.Text);
                Assert.Contains("Zip must be a valid zip code", ZipInvalidMessage.Text);
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.ValidateBlankFieldMessages method. Exception: " + e);
            }
        }

        public void BlankOutFields()
        {
            try
            {
                var StateName = new SelectElement(StateDropDown);

                StateName.SelectByValue("");
                NameTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
                CodeTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
                StreetTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
                CityTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
                ZipTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.BlankOutFields method. Exception: " + e);
            }
        }

        public void CompareWarehouseDetails()
        {
            Assert.Equal("Do Not Remove", DoNotRemoveWareHouseName.Text);
            Assert.Equal("1 Memorial Drive", DoNotRemoveWarehouseTwoStreetName.Text);
            Assert.Equal("St. Louis, MO 63102", DoNotRemoveWarehouseTwoCityStateZipName.Text);
            Assert.Equal("Code 2", DoNotRemoveWarehouseTwoCodeName.Text);
        }

        public void MakeWarehouseDefault(IWebElement button)
        {
                button.Click();
        }
    }
}
