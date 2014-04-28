﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Stores;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Users.Audit;

namespace ShipWorks.Tests.Integration.MSTest.ShipSense
{
    [TestClass]
    public class ShipSenseLoaderTest
    {
        private ShipSenseLoader testObject;

        private readonly Mock<ExecutionMode> executionMode;

        public ShipSenseLoaderTest()
        {
            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            Guid swInstance;
            switch (Environment.MachineName.ToLower())
            {
                case "tim-pc":
                    swInstance = Guid.Parse("{2D64FF9F-527F-47EF-BA24-ECBF526431EE}");
                    break;
                case "john-pc":
                    swInstance = Guid.Parse("{358e8025-ba77-43c7-8a4e-66af9860bd2c}");
                    break;
                case "kevin-pc":
                    swInstance = Guid.Parse("{0BDCFB64-15FC-4BA3-84BC-83E8A6D0455A}");
                    break;
                case "MSTest-vm":
                    swInstance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                    break;
                case "benz-pc":
                    swInstance = Guid.Parse("{a21e0f50-8eb6-469c-8d23-7632c5cdc652}");
                    break;
                default:
                    throw new ApplicationException("Enter your machine and ShipWorks instance guid in ShipSenseLoaderTest()");
            }

            if (ApplicationCore.ShipWorksSession.ComputerID == Guid.Empty)
            {

                ApplicationCore.ShipWorksSession.Initialize(swInstance);
                SqlSession.Initialize();

                Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
                Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

                DataProvider.InitializeForApplication();
                AuditProcessor.InitializeForApplication();
                
                ShippingSettings.InitializeForCurrentDatabase();
                ShippingProfileManager.InitializeForCurrentSession();
                ShippingDefaultsRuleManager.InitializeForCurrentSession();
                ShippingProviderRuleManager.InitializeForCurrentSession();

                StoreManager.InitializeForCurrentSession();

                UserManager.InitializeForCurrentUser();
                
                UserSession.InitializeForCurrentDatabase(executionMode.Object);

                if (!UserSession.Logon("shipworks", "shipworks", true))
                {
                    throw new Exception("A 'shipworks' account with password 'shipworks' needs to be created.");
                }

                ShippingManager.InitializeForCurrentDatabase();
                LogSession.Initialize();

                TemplateManager.InitializeForCurrentSession();
            }
        }

        [TestMethod]
        [TestCategory("ShipSense")]
        public void LoadData_CompletesInTwentySeconds_Test()
        {
            Stopwatch stopWatch = new Stopwatch();
            using (ShipSenseLoaderGateway gateway = new ShipSenseLoaderGateway(new Knowledgebase()))
            {
                testObject = new ShipSenseLoader(gateway);
                
                stopWatch.Start();
                testObject.LoadData();
                stopWatch.Stop();
            }

            Assert.IsTrue(stopWatch.ElapsedMilliseconds < 60000);

            Console.WriteLine(@"===================================================================================================");
            Console.WriteLine(@"====                           ShipSense Loader                                                ====");
            Console.WriteLine(@"===================================================================================================");
            Console.WriteLine(@"Elapsed time: {0} seconds", stopWatch.ElapsedMilliseconds / 1000.0M);
            Console.WriteLine(@"===================================================================================================");

        }
    }
}
