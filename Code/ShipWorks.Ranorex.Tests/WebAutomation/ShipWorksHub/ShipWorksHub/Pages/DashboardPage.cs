using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace ShipWorksHub.Pages
{
    class DashboardPage
    {   
        private IWebDriver _driver;

        public DashboardPage(IWebDriver driver) => _driver = driver;
        public IWebElement DashboardTxt => _driver.FindElement(By.XPath("/html/body/div/div/div[2]/div/header/h1"));
        public IWebElement WarehouseTab => _driver.FindElement(By.XPath("//*[@id='root']/div/div[1]/div/nav/section[1]/a[2]"));
        protected IWebElement LogoutButton => _driver.FindElement(By.XPath("//*[@id='root']/div/div[1]/div/nav/section[2]/button"));
        
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
