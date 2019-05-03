using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

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


        IWebDriver _driver;

        public WarehousesPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }
        public void CreateWarehouse()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div[2]/div/header/div/button")));
            CreateWarehouseButton.Click();
        }

        public void AddWarehouseDetails(string[] details)
        {
            var StateName = new SelectElement(StateDropDown);
            NameTxt.SendKeys(details[0]);
            CodeTxt.SendKeys(details[1]);
            StreetTxt.SendKeys(details[2]);
            CityTxt.SendKeys(details[3]);
            StateName.SelectByValue(details[4]);
            ZipTxt.SendKeys(details[5]);
        }

        public void AddWarehouse()
        {
            AddWarehouseButton.Click();
        }

        public void Cancel()
        {
            CancelButton.Click();
        }
        public void WarehousePageQuit()
        {
            _driver.Quit();
        }
    }
}
