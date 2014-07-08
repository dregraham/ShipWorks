using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Stores;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Users.Audit;

namespace ShipWorks.Tests.Integration.MSTest.Functional.Services
{
    [TestClass]
    public class ServiceStatusManagerTest
    {
        private readonly Mock<ExecutionMode> executionMode;
        private Mock<IProgressReporter> progressReporter;

        public ServiceStatusManagerTest()
        {
            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            progressReporter = new Mock<IProgressReporter>();

            Guid swInstance = GetShipWorksInstance();

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

        private Guid GetShipWorksInstance()
        {
            Guid instance;

            string instanceFromConfig = System.Configuration.ConfigurationManager.AppSettings["ShipWorksInstanceGuid"];
            if (!string.IsNullOrWhiteSpace(instanceFromConfig))
            {
                instance = Guid.Parse(instanceFromConfig);
            }
            else
            {
                // Fall back to the hard-coded values in the case where the instance value is not found in the
                // configuration file
                switch (Environment.MachineName.ToLower())
                {
                    case "tim-pc":
                        instance = Guid.Parse("{2D64FF9F-527F-47EF-BA24-ECBF526431EE}");
                        break;
                    case "john-pc":
                        instance = Guid.Parse("{358e8025-ba77-43c7-8a4e-66af9860bd2c}");
                        break;
                    case "kevin-pc":
                        instance = Guid.Parse("{0BDCFB64-15FC-4BA3-84BC-83E8A6D0455A}");
                        break;
                    case "MSTest-vm":
                        instance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                        break;
                    case "benz-pc":
                        instance = Guid.Parse("{a21e0f50-8eb6-469c-8d23-7632c5cdc652}");
                        break;
                    default:
                        throw new ApplicationException("Enter your machine and ShipWorks instance guid in ShipSenseLoaderTest()");
                }
            }

            return instance;
        }

        [TestMethod]
        [TestCategory("ApplicationCore")]
        public void CheckIn_DoesNotThrowORMEntityOutOfSyncException_WhenDatabaseValuesHaveChanged()
        {
            // This assumes it is being run against the "seeded" database (see SeedDatabase.sql script
            // in solution directory)
            ServiceStatusEntity firstServiceStatus = new LinqMetaData(new SqlAdapter(SqlSession.Current.OpenConnection())).ServiceStatus.FirstOrDefault();
            ServiceStatusEntity secondServiceStatus = new LinqMetaData(new SqlAdapter(SqlSession.Current.OpenConnection())).ServiceStatus.FirstOrDefault();

            ServiceStatusManager.CheckIn(firstServiceStatus);
            ServiceStatusManager.CheckIn(secondServiceStatus);
        }
    }
}
