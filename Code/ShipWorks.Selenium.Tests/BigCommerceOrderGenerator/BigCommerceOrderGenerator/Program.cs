using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;

namespace BigCommerceOrderGenerator9000
{
    class FirstTestCase
    {
        static void Main(string[] args)
        {
            
            PromptUser();

        }

        static void PromptUser()
        {
            string quantity;
            int iQuantity;
            Console.WriteLine("How many orders do you want created?");

            quantity = Console.ReadLine();

            if (int.TryParse(quantity, out iQuantity))
            {



                if (iQuantity == 0)
                {
                    Console.WriteLine("You selected zero. Application will now close.");
                }
                else if (iQuantity > 1000)
                {
                    Console.WriteLine("You selected " + quantity + " orders. That will take " + (iQuantity * .6).ToString() + " minutes, and send Katie a whole lot of order confirmation emails. Are you sure about that? (y/n)");
                    string Response = Console.ReadLine();
                    if (Response == "y")
                    {
                        Console.WriteLine("Alright... you asked for it...");
                        BigCommerceOrdersMethod(iQuantity);
                    }
                    else
                    {
                        Console.WriteLine("Good choice.");
                        PromptUser();
                    }
                }
                else if (iQuantity <= 1000)
                {
                    BigCommerceOrdersMethod(iQuantity);
                }
            }
            else
            {
                Console.WriteLine("You were supposed to enter an integer. Instead, you entered: " + quantity);
                Console.WriteLine("Try again, but this time, enter a valid integer.");
                PromptUser();
            }
        }

        static void BigCommerceOrdersMethod(int iQuantity)
        {



            Console.WriteLine("Commencing the creation of " + iQuantity + " orders. Close browser or command prompt at any time to abort.");

            //if connection is lost or other exception thrown, exceptionQuantity is how many orders are left

            int exceptionQuantity = iQuantity;

            try
            {
                //FirefoxProfile p = new FirefoxProfile();
                //p.SetPreference("javascript.enabled", false);
                //FirefoxOptions o = new FirefoxOptions();
                //o.AddAdditionalCapability("javascript.enabled", false);
                //o.Profile.AddExtension(@"noscript-10.1.8.23.xpi");
                IWebDriver driver = new FirefoxDriver();


                //driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 30);
                driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 60);
                driver.Url = "https://login.bigcommerce.com/login/";
                //Log into BigCommerce Portal

                IWebElement UserID = driver.FindElement(By.Id("user_email"));
                UserID.SendKeys("operations@shipworks.com");

                IWebElement UserPassword = driver.FindElement(By.Id("user_password"));
                UserPassword.SendKeys("bigcommerce7458-2");

                IWebElement LoginButton = driver.FindElement(By.Name("commit"));
                LoginButton.Submit();

                //click Orders
                IWebElement OrdersNavigator = driver.FindElement(By.CssSelector("[href*='/manage/orders']"));
                OrdersNavigator.Click();

                for (int i = 0; i < iQuantity; i++, exceptionQuantity--)
                {

                    //click add
                    IWebElement AddOrdersNavigator = driver.FindElement(By.CssSelector("[href*='/manage/orders/add-order']"));
                    //IWebElement AddOrdersNavigator = driver.FindElement(By.XPath("/html/body/div/div[2]/nav/section/ul/li[1]/div/div/div/ul[1]/li[2]/a"));
                    AddOrdersNavigator.Click();
                    System.Threading.Thread.Sleep(5000);

                    //search for Katie

                    driver.SwitchTo().Frame("content-iframe");
                    IWebElement CustomerSearchBox = driver.FindElement(By.Name("orderForSearch"));
                    CustomerSearchBox.Click();
                    CustomerSearchBox.SendKeys("Katie");
                    System.Threading.Thread.Sleep(2000);

                    //Click Katie
                    IWebElement KatieGicona = driver.FindElement(By.ClassName("customerDetails"));
                    KatieGicona.Click();
                    System.Threading.Thread.Sleep(5000);

                    //Click 'use this address'
                    IWebElement UseThisAddress = driver.FindElement(By.XPath("//li[1]/div[2]/button"));
                    UseThisAddress.Click();

                    //Click Next
                    System.Threading.Thread.Sleep(5000);
                    IWebElement NextButton = driver.FindElement(By.Name("AddAnother"));
                    NextButton.Click();
                    System.Threading.Thread.Sleep(2000);
                    //Search for item for Katie to purchase
                    IWebElement ItemSearchField = driver.FindElement(By.Id("quote-item-search"));
                    ItemSearchField.Click();
                    System.Threading.Thread.Sleep(2000);
                    ItemSearchField.SendKeys("Embarrassment");
                    System.Threading.Thread.Sleep(4000);
                    ItemSearchField.SendKeys(Keys.Tab);
                    System.Threading.Thread.Sleep(2000);

                    //Click next twice
                    NextButton.Click();
                    System.Threading.Thread.Sleep(2000);
                    NextButton.Click();
                    System.Threading.Thread.Sleep(2000);

                    //Finalize Order
                    SelectElement PaymentMethod = new SelectElement(driver.FindElement(By.Id("paymentMethod")));
                    PaymentMethod.SelectByValue("custom");
                    System.Threading.Thread.Sleep(2000);
                    IWebElement SaveProcessBtn = driver.FindElement(By.XPath("//li[3]/button[2]"));
                    SaveProcessBtn.Click();

                    System.Threading.Thread.Sleep(5000);
                    driver.SwitchTo().ParentFrame();
                    System.Threading.Thread.Sleep(5000);

                    Console.WriteLine("Order " + (i + 1) + " created.");

            }
                Console.WriteLine("All orders complete.");
                //Close FireFox
                driver.Close();
            }
            catch
            {
                Console.WriteLine("Something bad happened, lets try that again. Starting where we left off: " + exceptionQuantity + " orders left.");
                BigCommerceOrdersMethod(exceptionQuantity);
            }

        }
    }
}
