/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 4/9/2019
 * Time: 3:07 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace OrderCreation
{
    /// <summary>
    /// Description of NumberOfOrders.
    /// </summary>
    [TestModule("2C76A8F5-EDA7-4C1F-B960-4E05E1D2B7E5", ModuleType.UserCode, 1)]
    public class NumberOfOrders : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public NumberOfOrders()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void LoggedIn() //Function to check if the user is logged into CA on Firefox
            {
        	 string YesOrNo = "";
           	 int NumberOfOrders = 1; //stores the number of orders the user wants to create 
        	
                Console.WriteLine("Are you signed into ChannelAdvisor on Firefox? Answer format: Y or N");
                YesOrNo = Console.ReadLine().ToLower();

                switch (YesOrNo)
                {
                    case "y": //if user enters yes
                        Console.WriteLine("Please log out of ChannelAdvisor on Firefox.");
                        Console.ReadLine(); //Make user press enter before continuing and next message showing
                        LoggedIn(); //start the series of questions again
                        break;
                    case "n": //if user enters no
                        Console.WriteLine("How many orders do you want to create?");
                        try
                        {
                            NumberOfOrders = Convert.ToInt32(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Please use whole numbers only.");
                            Console.ReadLine(); //Pause console window so user can see error message.
                            LoggedIn(); //start the series of questions again
                        }
                        
                        LogInCA.Start(); //log into ChannelAdvisor account
                        
                        for (int i = 0; i < NumberOfOrders; i++) //loop through and creates the orders 
                        {
                            CreateOrderCA.Start();
                            Variables.NumberOfLoops++;
                        }
                        break;
                    default:
                        Console.WriteLine("Y or N"); // tell the user to enter Y or N
                        Console.ReadLine(); //Pause console window so user can see error message.
                        LoggedIn(); //start the series of questions again
                        break;
                }
            }
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            LoggedIn(); //start function
        }
    }
}