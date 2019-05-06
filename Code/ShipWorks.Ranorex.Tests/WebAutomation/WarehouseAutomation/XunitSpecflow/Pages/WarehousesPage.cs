using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[2]/div/article/div/div[2]/button[2]/div")]
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

        public void Edit()
        {
            EditButton.Click();
        }

        public void EditWarehouseDetails(string[] details)
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

        public void Save()
        {
            SaveButton.Click();
        }

        public void Remove()
        {
            RemoveButton.Click();
        }

        public void ValidateFieldLengthErrorMessages()
        {
            Assert.Contains("Name cannot be longer than 500 characters", NameCharacterLimitMessage.Text);
            Assert.Contains("Code cannot be longer than 500 characters", CodeCharacterLimitMessage.Text);
            Assert.Contains("Street cannot be longer than 500 characters", StreetCharacterLimitMessage.Text);
            Assert.Contains("City cannot be longer than 500 characters", CityCharacterLimitMessage.Text);
        }
    }
}
