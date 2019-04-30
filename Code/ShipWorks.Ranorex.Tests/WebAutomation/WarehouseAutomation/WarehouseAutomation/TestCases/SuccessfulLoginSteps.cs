using TechTalk.SpecFlow;
using WarehouseWebAutomation;
using WarehouseWebAutomation.Variables;

namespace WarehouseAutomation.TestCases
{
    [Binding]
    public class SuccessfulLoginSteps
    {
        [Given(@"the user is on warehouse login page using the web browser '(.*)'")]
        public void GivenTheUserIsOnWarehouseLoginPageUsingTheWebBrowser(string WebBrowser)
        {
            AutomationMethods.Launch(WarehouseVariables.URL, WebBrowser);
            AutomationMethods.SaveScreenshot("LoginScreen");
        }

        [When(@"the user enters username")]
        public void WhenTheUserEntersUsername()
        {
            AutomationMethods.Click(WarehouseVariables.UsernameXpath);
            AutomationMethods.Type(WarehouseVariables.Username, WarehouseVariables.UsernameXpath);
            AutomationMethods.SaveScreenshot("Username");
               
        }

        [When(@"the user enters password")]
        public void WhenTheUserEntersPassword()
        {
            AutomationMethods.Click(WarehouseVariables.PasswordXpath);
            AutomationMethods.Type(WarehouseVariables.Password, WarehouseVariables.PasswordXpath);
            AutomationMethods.SaveScreenshot("Password");
        }

        [When(@"the user clicks login")]
        public void WhenTheUserClicksLogin()
        {
            AutomationMethods.Click(WarehouseVariables.LoginButtonXpath);
        }

        [Then(@"the user sees the home page")]
        public void ThenTheUserSeesTheHomePage()
        {
            AutomationMethods.ValidateIfExists(WarehouseVariables.Dashboard, AutomationMethods.GetText(WarehouseVariables.DashboardXpath));
            AutomationMethods.SaveScreenshot("HomePage");
            AutomationMethods.Dispose();
        }
    }
}


