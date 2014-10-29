﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
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

namespace ShipWorks.Tests.Integration.MSTest.Shipping.ShipSense
{
    [TestClass]
    public class ShipSenseLoaderTest
    {
        private ShipSenseLoader testObject;

        private readonly Mock<ExecutionMode> executionMode;
        private readonly Mock<IProgressReporter> progressReporter;

        public ShipSenseLoaderTest()
        {
            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            progressReporter = new Mock<IProgressReporter>();

            Guid swInstance = ShipWorksInitializer.GetShipWorksInstance();

            if (ApplicationCore.ShipWorksSession.ComputerID == Guid.Empty)
            {

                ApplicationCore.ShipWorksSession.Initialize(swInstance);
                SqlSession.Initialize();

                Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
                Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

                DataProvider.InitializeForApplication(executionMode.Object);
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
        [TestCategory("ShipSense"), TestCategory("IncludeInJenkinsBuild")]
        public void LoadData_WithSeededDatabase_CompletesInFiveSecondsOrLess_Test()
        {
            // This assumes it is being run against the "seeded" database (see SeedDatabase.sql script
            // in solution directory)
            Stopwatch stopWatch = new Stopwatch();
            using (ShipSenseLoaderGateway gateway = new ShipSenseLoaderGateway(new Knowledgebase()))
            {
                testObject = new ShipSenseLoader(progressReporter.Object, gateway);
                
                stopWatch.Start();
                testObject.LoadData();
                stopWatch.Stop();
            }

            Assert.IsTrue(stopWatch.ElapsedMilliseconds < 5000);

            Console.WriteLine(@"===================================================================================================");
            Console.WriteLine(@"====                           ShipSense Loader                                                ====");
            Console.WriteLine(@"===================================================================================================");
            Console.WriteLine(@"Elapsed time: {0} seconds", stopWatch.ElapsedMilliseconds / 1000.0M);
            Console.WriteLine(@"===================================================================================================");

        }
    }
}
