using System;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Class responsible for sending auto upgrade failures to the Azure queue
    /// </summary>
    public static class AutoUpgradeFailureSubmitter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoUpgradeFailureSubmitter));

        /// <summary>
        /// Submit the failure to the queue
        /// </summary>
        public static void Submit(string versionThatFailed, string failureReason)
        {
            try
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;

                if (SqlSession.IsConfigured && SqlSession.Current != null)
                {
                    UserManager.InitializeForCurrentUser();
                    UserEntity user = UserManager.GetUsers(false).OrderByDescending(u => u.IsAdmin).FirstOrDefault();

                    string licenseKeys = string.Empty; 

                    using (ISqlAdapter adapter = SqlAdapter.Default)
                    {
                        StoreCollection stores = new StoreCollection();
                        adapter.FetchEntityCollection(stores, null, (IRelationPredicateBucket) null);
                        licenseKeys = String.Join($"{Environment.NewLine}", stores?.Select(s => s.License));
                    }

                    if (CloudStorageAccount.TryParse(GetConnectionString(version), out CloudStorageAccount storageAccount))
                    {
                        UpgradeFailureDto details = new UpgradeFailureDto
                        {
                            CustomerName = user?.Username ?? "Unknown Username",
                            CustomerEmail = user?.Email ?? "Unknown Email",
                            Version = versionThatFailed,
                            DbID = new DatabaseIdentifier().Get().ToString("N"),
                            FailureReason = failureReason,
                            StoreKeys = licenseKeys,
                        };

                        LogToQueue(storageAccount, details);
                    }
                }
            }
            catch (Exception ex)
            {
                // Just log and carry on
                log.Error(ex);
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
            /// The store license keys
            /// </summary>
            public string StoreKeys { get; set; }
        }
    }
}
