using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ShipWorksHub.Pages
{
    class AddWarehousePage
    {
        private IWebDriver _driver;
        public AddWarehousePage(IWebDriver driver) => _driver = driver;

        protected IWebElement NameTxt => _driver.FindElement(By.Name("name"));
        protected IWebElement CodeTxt => _driver.FindElement(By.Name("code"));
        protected IWebElement StreetTxt => _driver.FindElement(By.Name("street"));
        protected IWebElement CityTxt => _driver.FindElement(By.Name("city"));
        protected IWebElement ZipTxt => _driver.FindElement(By.Name("zip"));
        protected IWebElement StateDropDown => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div/div[1]/label[4]/select"));
        protected IWebElement AddWarehouseButton => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div/div[2]/button[2]/div"));


        public void AddWarehouseDetails(string[] details)
        {
            try
            {
                var StateName = new SelectElement(StateDropDown);
                NameTxt.SendKeys(details[0]);
                //CodeTxt.SendKeys(details[1]);
                StreetTxt.SendKeys(details[2]);
                CityTxt.SendKeys(details[3]);
                StateName.SelectByValue(details[4]);
                ZipTxt.SendKeys(details[5]);
            }
            catch (Exception e)
            {
                _driver.Quit();
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
                _driver.Quit();                
                throw new Exception("Automation failed at WarehousePage.AddWarehouse method. Exception: " + e);
            }
        }

    }
}
