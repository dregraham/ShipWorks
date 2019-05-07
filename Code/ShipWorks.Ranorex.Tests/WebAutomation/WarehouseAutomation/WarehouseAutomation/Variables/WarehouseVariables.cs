using System.IO;
namespace WarehouseWebAutomation.Variables
{
     static class WarehouseVariables
    {
        public static readonly string WebDriverPath = Directory.GetCurrentDirectory();
        public static readonly string ScreenshotPath = Path.GetFullPath(Path.Combine(WebDriverPath, @"../../Screenshots/"));

        public static readonly string URL = "https://qa.www.warehouseapp.link/login";
        public static readonly string Username = "user-0410@example.com";
        public static readonly string Password = "GOOD";
        public static readonly string BadUsername = "badusername";
        public static readonly string BadPassword = "bad";
        public static readonly string Dashboard = "Dashboard";
        public static readonly string LoginErrorMessage = "Invalid username or password";
                
        //  XPATH
        public static readonly string UsernameXpath = "//*[@id='root']/div/div/div/div[2]/div/label[1]/input";
        public static readonly string PasswordXpath = "//*[@id='root']/div/div/div/div[2]/div/label[2]/input";
        public static readonly string LoginButtonXpath = "//*[@id='root']/div/div/div/div[2]/button";
        public static readonly string DashboardXpath = "//*[@id='root']/div/div[2]/div/header/h1";
        public static readonly string LoginErrorMessageXpath = "//*[@id='root']/div/div/div[1]";
    }
}
