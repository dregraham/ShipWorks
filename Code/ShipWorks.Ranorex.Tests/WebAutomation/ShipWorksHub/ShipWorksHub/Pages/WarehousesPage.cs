using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ShipWorksHub.Pages
{
    class WarehousesPage
    {
        public string settingsURL = "https://s2.www.warehouseapp.link/settings";

        public string warehousesHeadingText = "Warehouses";
        public IWebElement warehousesHeading => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/header[1]/h1"));
        public IWebElement firstWarehouse => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div[1]/div/header/div[1]/h2"));
        protected IWebElement ChangeAddress => _driver.FindElement(By.XPath("/html/body/div/div/div[2]/div/article[1]/div[1]/nav/div[2]/button/div"));
        public IWebElement CreateWarehouseButton => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/header/div/button"));
        protected IWebElement RemoveButton => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[2]/div/nav/div[2]/button/div"));
        public IWebElement DoNotRemoveWareHouseName => _driver.FindElement(By.XPath("/html/body/div/div/div[2]/div/article[1]/div/div/header/div[1]/h2"));
        public IWebElement DoNotRemoveWarehouseTwoStreetName => _driver.FindElement(By.XPath("/html/body/div/div/div[2]/div/article[1]/div/div/div/div[1]"));
        public IWebElement DoNotRemoveWarehouseTwoCityStateZipName => _driver.FindElement(By.XPath("/html/body/div/div/div[2]/div/article[1]/div/div/div/div[2]"));
        public IWebElement DoNotRemoveWarehouseTwoCodeName => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[2]/header/div"));
        public IWebElement CheckMarkDefaultText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article[1]/div[1]/nav/div[1]/div"));
        public IWebElement FirstDefaultText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[1]/div/div/div[3]/div"));
        public IWebElement SecondDefaultText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[2]/div/div/div[3]/div"));
        public IWebElement SecondDefaultButton => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[2]/div/div/div[3]/button/div"));
        public IWebElement FirstDefaultButton => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[1]/div/div/div[3]/button/div"));
        public IWebElement PageNotFoundText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/header/h1"));
        public IWebElement WarehouseNotFoundText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div"));
        public IWebElement FirstWarehouseNameText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[1]/header/h2"));
        public IWebElement SecondWarehouseNameText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[2]/header/h2"));
        public IWebElement ThirdWarehouseNameText => _driver.FindElement(By.XPath("//*[@id='root']/div/div[2]/div/article/div[3]/header/h2"));

        public string WarehouseList = "div.sc-hzDkRC"; // use css selector of div and not article instead of xpath from context menu for warehouse list

        private IWebDriver _driver;
        
        List<string> list = new List<string>();

        string lastWarehouseName;

        public WarehousesPage(IWebDriver driver) => _driver = driver;

        public bool CheckIfNotDefault()
        {
            GetListOfWarehouseNames();
            int compareMax = Int32.Parse(list.Max().TrimStart('A'));
            bool displayed = false;

            displayed = _driver.FindElement(By.XPath($"/html/body/div/div/div[2]/div/article[1]/div[{compareMax}]/nav/div[1]/button/div")).Displayed;

            return displayed;
        }

        public string GetListOfWarehouseNames()
        {
            int listMax = _driver.FindElements(By.CssSelector(WarehouseList)).Count;
            
            for (int listIndex = 1; listIndex <= listMax; listIndex++)
            {
                list.Add(_driver.FindElement(By.XPath($"//*[@id='root']/div/div[2]/div/article[1]/div[{listIndex}]/div/header/div[1]/h2")).Text);
            }
            
            return list.Max();
        }

        public void CheckWarehouseSort()
        {
            GetListOfWarehouseNames();  //  GET LATEST LIST OF WAREHOUSES

            int compareMax = Int32.Parse(list.Max().TrimStart('A'));
            string warehouseXpath, warehouseName;

            for (int compareIndex = 0; compareIndex < compareMax; compareIndex++)
            {
                warehouseXpath = $"//*[@id='root']/div/div[2]/div/article[1]/div[{compareIndex + 1}]/div/header/div[1]/h2";
                warehouseName = _driver.FindElement(By.XPath(warehouseXpath)).Text;
                Assert.Equal($"A{compareIndex + 1}", warehouseName);
            }
        }

        public string NewWarehouseName(int lastWarehouse)
        {
            //int warehouseint = Int32.Parse(lastName.TrimStart('A')) + 1;
            
            lastWarehouseName = $"A{++lastWarehouse}";
            return lastWarehouseName;
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
                ChangeAddress.Click();
            }
            catch (Exception e)
            {
                WarehousePageQuit();
                throw new Exception("Automation failed at WarehousePage.Edit method. Exception: " + e);
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

        public void CompareWarehouseDetails()
        {
            Assert.Equal("Do Not Remove", DoNotRemoveWareHouseName.Text);
            Assert.Equal("1 Memorial Drive", DoNotRemoveWarehouseTwoStreetName.Text);
            Assert.Equal("St. Louis, MO 63102", DoNotRemoveWarehouseTwoCityStateZipName.Text);
        }

        public string MakeWarehouseDefault(string warehouse, string warehouseName)
        {
            GetListOfWarehouseNames();

            int i = list.FindIndex(0, x => x == warehouseName);

            switch (warehouse)
            {
                case "First":
                    warehouse = "/html/body/div/div/div[2]/div/article[1]/div[1]/nav/div[1]/button/div";
                    _driver.FindElement(By.XPath(warehouse)).Click();                    
                    break;
                case "New":
                    warehouse = $"/html/body/div/div/div[2]/div/article[1]/div[{++i}]/nav/div[1]/button/div";
                    _driver.FindElement(By.XPath(warehouse)).Click();
                    break;
            }
            return warehouse;
        }
    }
}
