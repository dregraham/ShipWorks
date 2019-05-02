using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace XunitSpecflow.Pages
{
    class DashboardPage
    {
        [FindsBy(How = How.XPath, Using = "/html/body/div/div/div[2]/div/header/h1")]
        protected IWebElement DashboardTxt { get; set; }

        private IWebDriver _driver;

        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;            
            PageFactory.InitElements(_driver, this);
        }

        public string GetDashboard()
        {
            return DashboardTxt.Text;
        }

        public void DashboardQuit()
        {
            _driver.Quit();
        }
    }
}
