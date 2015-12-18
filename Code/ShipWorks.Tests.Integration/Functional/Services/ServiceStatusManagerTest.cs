using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
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
                    case "tim-pc2":
                        instance = Guid.Parse("{a6c0a1b0-4757-4655-b74b-dbb9195b82ff}");
                        break;
                    case "john3610-pc":
                        instance = Guid.Parse("{a721d9e4-fb3b-4a64-a612-8579b1251c95}");
                        break;
                    case "kevin-pc":
                        instance = Guid.Parse("{6db3aa02-32bb-430e-95d2-0c59b3b7417a}");
                        break;
                    case "MSTest-vm":
                        instance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                        break;
                    case "benz-pc":
                        instance = Guid.Parse("{a21e0f50-8eb6-469c-8d23-7632c5cdc652}");
                        break;
                    case "berger-pc":
                        instance = Guid.Parse("{AABB7285-a889-46af-87b8-69c10cdbAABB}");
                        break;
                    case "mirza-pc2":
                        instance = Guid.Parse("{1231F4A9-640C-4E08-A52A-AE3B2C2FB864}");
                        break;
                    default:
                        throw new ApplicationException("Enter your machine and ShipWorks instance guid in ShipSenseLoaderTest()");
                }
            }

            return instance;
        }

        [Fact]
        [Trait("Category", "ApplicationCore")]
        [Trait("Category", "ContinuousIntegration")]
        public void CheckIn_DoesNotThrowORMEntityOutOfSyncException_WhenDatabaseValuesHaveChanged()
        {
            ServiceStatusEntity firstServiceStatus = null;

            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                firstServiceStatus = new LinqMetaData(adapter).ServiceStatus.FirstOrDefault();
            }

            if (firstServiceStatus == null)
            {
                Assert.False(true, "Could not retrieve a service status");
            }

            // Imitate something else modifying the service status without refetching it
            firstServiceStatus.LastStartDateTime = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, 15));
            SqlAdapter.Default.SaveEntity(firstServiceStatus);

            ServiceStatusManager.CheckIn(firstServiceStatus);
        }
    }
}
