using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace ShipWorksHub
{
    [Binding]
    public class LaunchShipWorksHubSteps
    {
        private IWebDriver webDriver;

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            webDriver = new ChromeDriver(Directory.GetCurrentDirectory());
            webDriver.Navigate().GoToUrl("https://s2.www.warehouseapp.link/login");
        }
    }
}
