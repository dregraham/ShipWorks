using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Common.Logging;
using Interapptive.Shared.Data;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Class responsible for sending auto upgrade failures to the Azure queue
    /// </summary>
    public class AutoUpgradeFailureSubmitter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoUpgradeFailureSubmitter));
        private static readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;
        private (string CustomerEmail, string FirstAndLastName) customerInfo = (string.Empty, string.Empty);
        private string licenseKeys = string.Empty;
        private string dbId = string.Empty;

        public void Initialize()
        {
            if (SqlSession.IsConfigured && SqlSession.Current?.CanConnect() == true)
            {
                ConfigurationData.InitializeForCurrentDatabase();

                string storeLicense = string.Empty;
                string customerKey = string.Empty;
                dbId = new DatabaseIdentifier().Get().ToString("N");

                using (ISqlAdapter adapter = SqlAdapter.Default)
                {
                    StoreCollection stores = new StoreCollection();
                    adapter.FetchEntityCollection(stores, null, (IRelationPredicateBucket) null);
                    storeLicense = stores.FirstOrDefault()?.License;
                    licenseKeys = String.Join($"{Environment.NewLine}", stores?.Select(s => s.License));

                    // Get customer key
                    ConfigurationEntity config = new ConfigurationEntity(false);
                    adapter.FetchEntity(config);

                    using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                    {
                        ICustomerLicenseReader customerLicenseReader = scope.Resolve<ICustomerLicenseReader>();
                        customerKey = customerLicenseReader.Read();
                    }

                    log.Debug($"Attempting GetCustomerEmail({storeLicense}, {customerKey})");
                    customerInfo = TangoWebClient.GetCustomerEmail(storeLicense, customerKey);
                    log.Debug($"Results of GetCustomerEmail: {customerInfo.CustomerEmail}, {customerInfo.FirstAndLastName}");
                }
            }
        }

        /// <summary>
        /// Submit the failure to the queue
        /// </summary>
        public void Submit(string versionThatFailed, Exception ex)
        {
            try
            {
                if (CloudStorageAccount.TryParse(GetConnectionString(version), out CloudStorageAccount storageAccount))
                {
                    UpgradeFailureDto details = new UpgradeFailureDto
                    {
                        CustomerName = customerInfo.FirstAndLastName,
                        CustomerEmail = customerInfo.CustomerEmail,
                        Version = versionThatFailed,
                        DbID = dbId,
                        FailureReason = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerFailureReason = ex.InnerException?.Message,
                        InnerStackTrace = ex.InnerException?.StackTrace,
                        StoreKeys = licenseKeys,
                    };

                    LogToQueue(storageAccount, details);
                }
            }
            catch (Exception ex1)
            {
                // Just log and carry on
                log.Error(ex1);
            }
        }

        /// <summary>
        /// Get the storage connection string
        /// </summary>
        private static string GetConnectionString(Version version)
        {
            return version.Major > 0 ?
                "DefaultEndpointsProtocol=https;AccountName=shipworkscrashes;AccountKey=sd1Ozm5Q81N+7Jy1Y5TXuuS06hfmqNAAOUTG3lb0QjiJxZN+QCHTqQTKB6mHRxbuAsJ1FSHC1hdnwM3BXiexWQ==" :
                "DefaultEndpointsProtocol=https;AccountName=sw201606crash;AccountKey=J3aKx7pIpm2yian0B3YufolSx/f/rAkdTmF/VhRi22X6k7BIR37qUWrLgFlJKAThUjSsFOSLccKiIxQzKFHmNQ==";
        }

        /// <summary>
        /// Log the auto upgrade failure to the storage queue
        /// </summary>
        private static void LogToQueue(CloudStorageAccount storageAccount, UpgradeFailureDto upgradeFailureDto)
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("upgradefailures");
            queue.CreateIfNotExists();
            queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(upgradeFailureDto)));
        }

        /// <summary>
        /// DTO for sending upgrade failures to the queue
        /// </summary>
        [Obfuscation(ApplyToMembers = true, Exclude = false, StripAfterObfuscation = false)]
        private struct UpgradeFailureDto
        {
            /// <summary>
            /// The customer email
            /// </summary>
            public string CustomerEmail { get; set; }

            /// <summary>
            /// The customer name
            /// </summary>
            public string CustomerName { get; set; }

            /// <summary>
            /// The version that failed to upgrade
            /// </summary>
            public string Version { get; set; }

            /// <summary>
            /// The ShipWorks DB ID
            /// </summary>
            public string DbID { get; set; }

            /// <summary>
            /// The failure reason
            /// </summary>
            public string FailureReason { get; set; }

            /// <summary>
            /// The exception stack trace
            /// </summary>
            public string StackTrace { get; set; }

            /// <summary>
            /// The inner exception failure reason
            /// </summary>
            public string InnerFailureReason { get; set; }

            /// <summary>
            /// The inner exception stack trace
            /// </summary>
            public string InnerStackTrace { get; set; }

            /// <summary>
            /// The store license keys
            /// </summary>
            public string StoreKeys { get; set; }
        }
    }
}
