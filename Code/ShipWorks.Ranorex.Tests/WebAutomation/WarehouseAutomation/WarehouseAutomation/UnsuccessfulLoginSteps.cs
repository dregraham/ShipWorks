using TechTalk.SpecFlow;
using WarehouseWebAutomation;
using WarehouseWebAutomation.Variables;
using System.Threading;

namespace WarehouseAutomation
{
    [Binding]
    public class UnsuccessfulLoginSteps
    {
        [When(@"the user enters a bad username")]
        public void WhenTheUserEntersABadUsername()
        {
            AutomationMethods.Click(WarehouseVariables.UsernameXpath);
            AutomationMethods.Type(WarehouseVariables.BadUsername, WarehouseVariables.UsernameXpath);
            AutomationMethods.SaveScreenshot("BadUsername");
        }

        [When(@"the user enters a bad password")]
        public void WhenTheUserEntersABadPassword()
        {
            AutomationMethods.Click(WarehouseVariables.PasswordXpath);
            AutomationMethods.Type(WarehouseVariables.BadPassword, WarehouseVariables.PasswordXpath);
            AutomationMethods.SaveScreenshot("BadPassword");
        }

        [Then(@"the user sees an error message that their credentials are invalid")]
        public void ThenTheUserSeesAnErrorMessageThatTheirCredentialsAreInvalid()
        {
            Thread.Sleep(3000); //wait 3 seconds or else the below method grabs the "logging in" text before the user actually has a chance to log in (and fail because of bad credentials)
            AutomationMethods.ValidateIfExists(WarehouseVariables.LoginErrorMessage, AutomationMethods.GetText(WarehouseVariables.LoginErrorMessageXpath));
            AutomationMethods.Dispose();
        }
    }
}
