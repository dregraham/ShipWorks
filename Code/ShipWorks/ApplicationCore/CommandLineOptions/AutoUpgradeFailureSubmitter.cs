using System;
using System.Data.Common;
using System.Reflection;
using Autofac;
using Common.Logging;
using Interapptive.Shared.Data;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Class responsible for sending auto upgrade failures to the Azure queue
    /// </summary>
    public class AutoUpgradeFailureSubmitter
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AutoUpgradeFailureSubmitter));
        private (string CustomerEmail, string FirstAndLastName, string LicenseKeys) customerInfo;
        private string dbId = string.Empty;

        /// <summary>
        /// Initialize with data to send
        /// </summary>
        public void Initialize()
        {
            if (SqlSession.IsConfigured && SqlSession.Current?.CanConnect() == true)
            {
                dbId = new DatabaseIdentifier().Get().ToString("N");
                customerInfo = GetCustomerInfo();
            }
        }

        /// <summary>
        /// Submit the failure to the queue
        /// </summary>
        public void Submit(string versionThatFailed, Exception ex)
        {
            try
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;

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
                        StoreKeys = customerInfo.LicenseKeys,
                        MachineName = Environment.MachineName,
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
        /// Get customer info
        /// </summary>
        private (string CustomerEmail, string FirstAndLastName, string LicenseKeys) GetCustomerInfo()
        {
            (string CustomerEmail, string FirstAndLastName, string LicenseKeys) customerInfo = (string.Empty, string.Empty, string.Empty);
            string customerKey = string.Empty;
            string storeLicense = string.Empty;

            try
            {
                // Don't use ConfigurationEntity because its schema may have changed and LLBLGen may throw when hydrating the object.
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    using (var reader = DbCommandProvider.ExecuteReader(con, "SELECT License FROM [Store]"))
                    {
                        while (reader.Read())
                        {
                            storeLicense = reader["License"].ToString();
                            customerInfo.LicenseKeys += storeLicense + Environment.NewLine;
                        }
                    }
                }

                // Get customer key
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    ICustomerLicenseReader customerLicenseReader = scope.Resolve<ICustomerLicenseReader>();
                    customerKey = customerLicenseReader.Read();
                }

                log.Debug($"Attempting GetCustomerEmail({storeLicense}, {customerKey})");

                var response = TangoWebClient.GetCustomerEmail(storeLicense, customerKey);
                customerInfo.CustomerEmail = response.Email;
                customerInfo.FirstAndLastName = response.FirstAndLastName;

                log.Debug($"Results of GetCustomerEmail: {customerInfo.CustomerEmail}, {customerInfo.FirstAndLastName}");

            }
            catch (Exception ex)
            {
                // Just log and return with what we were able to get.
                log.Error("An error occurred getting customer info.", ex);
            }

            return customerInfo;
        }

        /// <summary>
        /// Get the storage connection string
        /// </summary>
        private string GetConnectionString(Version version)
        {
            return version.Major > 0 ?
                "DefaultEndpointsProtocol=https;AccountName=shipworkscrashes;AccountKey=sd1Ozm5Q81N+7Jy1Y5TXuuS06hfmqNAAOUTG3lb0QjiJxZN+QCHTqQTKB6mHRxbuAsJ1FSHC1hdnwM3BXiexWQ==" :
                "DefaultEndpointsProtocol=https;AccountName=sw201606crash;AccountKey=J3aKx7pIpm2yian0B3YufolSx/f/rAkdTmF/VhRi22X6k7BIR37qUWrLgFlJKAThUjSsFOSLccKiIxQzKFHmNQ==";
        }

        /// <summary>
        /// Log the auto upgrade failure to the storage queue
        /// </summary>
        private void LogToQueue(CloudStorageAccount storageAccount, UpgradeFailureDto upgradeFailureDto)
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

            /// <summary>
            /// The machine name on which the upgrade was attempted
            /// </summary>
            public string MachineName { get; set; }
        }
    }
}
