using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ShipWorksHub.Pages
{
    class EditWarehousePage
    {
        private IWebDriver _driver;
        public EditWarehousePage(IWebDriver driver) => _driver = driver;
        protected IWebElement NameTxt => _driver.FindElement(By.Name("name"));
        protected IWebElement CodeTxt => _driver.FindElement(By.Name("code"));
        protected IWebElement StreetTxt => _driver.FindElement(By.Name("street"));
        protected IWebElement CityTxt => _driver.FindElement(By.Name("city"));
        protected IWebElement ZipTxt => _driver.FindElement(By.Name("zip"));
        protected IWebElement StateDropDown => _driver.FindElement(By.XPath("/html/body/div/div/div[2]/div/article[1]/div/div[1]/label[3]/select"));
        protected IWebElement SaveButton => _driver.FindElement(By.Name("saveWarehouse"));
        protected IWebElement CancelButton => _driver.FindElement(By.Name("cancelWarehouse"));
        protected IWebElement StreetCharacterLimitMessage => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div/div[2]/label[1]/span[2]"));
        protected IWebElement CityCharacterLimitMessage => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div/div[2]/label[2]/span[2]"));
        protected IWebElement StateBlankMessage => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div/div[2]/label[3]/span[2]"));
        protected IWebElement ZipRequiredMessage => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div/div[2]/label[4]/span[2]"));
        protected IWebElement ZipInvalidMessage => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div/div[2]/label[4]/span[3]"));

        public void ValidateFieldLengthErrorMessages()
        {
            try
            {                
                Assert.Contains("Street cannot be longer than 500 characters", StreetCharacterLimitMessage.Text);
                Assert.Contains("City cannot be longer than 500 characters", CityCharacterLimitMessage.Text);
            }
            catch (Exception e)
            {
                _driver.Quit();
                throw new Exception("Automation failed at WarehousePage.ValidateFieldLengthErrorMessages method. Exception: " + e);
            }
        }

        public void ValidateBlankFieldMessages()
        {
            try
            {   
                Assert.Contains("Street is required", StreetCharacterLimitMessage.Text);
                Assert.Contains("City is required", CityCharacterLimitMessage.Text);
                Assert.Contains("State is required", StateBlankMessage.Text);
                Assert.Contains("Zip is required", ZipRequiredMessage.Text);
                Assert.Contains("Zip must be a valid zip code", ZipInvalidMessage.Text);
            }
            catch (Exception e)
            {
                _driver.Quit();
                throw new Exception("Automation failed at WarehousePage.ValidateBlankFieldMessages method. Exception: " + e);
            }
        }

        public void BlankOutFields()
        {
            try
            {
                var StateName = new SelectElement(StateDropDown);

                StateName.SelectByValue("");                                
                StreetTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
                CityTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
                ZipTxt.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Delete);
            }
            catch (Exception e)
            {
                _driver.Quit();
                throw new Exception("Automation failed at WarehousePage.BlankOutFields method. Exception: " + e);
            }
        }

        public void EditWarehouseDetails(string[] details)
        {
            try
            {
                var StateName = new SelectElement(StateDropDown);

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
                _driver.Quit();
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
                _driver.Quit();
                throw new Exception("Automation failed at WarehousePage.Save method. Exception: " + e);
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
                _driver.Quit();
                throw new Exception("Automation failed at WarehousePage.Cancel method. Exception: " + e);
            }
        }
    }
}
