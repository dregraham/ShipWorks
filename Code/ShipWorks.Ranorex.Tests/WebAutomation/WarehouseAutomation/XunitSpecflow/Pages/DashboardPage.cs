using System;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace XunitSpecflow.Pages
{
    class DashboardPage
    {
        [FindsBy(How = How.XPath, Using = "/html/body/div/div/div[2]/div/header/h1")]
        public IWebElement DashboardTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[1]/div/nav/section[1]/a[2]")]
        protected IWebElement WarehouseTab { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[1]/div/nav/section[2]/button")]
        protected IWebElement LogoutButton { get; set; }
        private IWebDriver _driver;


        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public WarehousesPage ClickWarehouseTab()
        {
            try
            {
                WarehouseTab.Click();
                return new WarehousesPage(_driver);
            }
            catch (Exception e)
            {
                DashboardQuit();
                throw new Exception("Automation failed at DashboardPage.ClickWarehouseTab method. Exception: " + e);
            }
        }

        public void DashboardQuit()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception e)
            {
                throw new Exception("Automation failed at DashboardPage.DashboardQuit method. Exception: " + e);
            }
        }

        public LoginPage Logout()
        {
            try
            {
                LogoutButton.Click();
                new WebDriverWait(_driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div[2]/button")));
                return new LoginPage(_driver);
            }
            catch (Exception e)
            {
                DashboardQuit();
                throw new Exception("Automation failed at DashboardPage.Logout method. Exception: " + e);
            }
        }
    }
}
