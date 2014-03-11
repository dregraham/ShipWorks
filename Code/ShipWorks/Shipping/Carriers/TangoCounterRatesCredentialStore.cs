using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using log4net;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Singleton counter credential store that holds ShipWorks accounts for shipping carriers used to get counter rates.
    /// </summary>
    public class TangoCounterRatesCredentialStore : ICounterRatesCredentialStore
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TangoCounterRatesCredentialStore));

        private static readonly FedExSettingsRepository fedExSettingsRepo = new FedExSettingsRepository();
        
        private static DateTime lastFailure;

        // The lazy loaded singleton instance variable.
        private static readonly Lazy<TangoCounterRatesCredentialStore> lazyInstance =
            new Lazy<TangoCounterRatesCredentialStore>(() => new TangoCounterRatesCredentialStore());

        // The lock used when populating our credential dictionary
        private static readonly object lockObject = new object();

        // A dictionary used for storing the account credentials for providers' production accounts
        private static volatile Dictionary<string, string> productionCredentials;

        // Credentials to use when ShipWorks is using a provider's test environment
        private const string TestCredentialFedExAccountNumber = "603103343";
        private const string TestCredentialFedExMeterNumber = "118601174";
        private const string TestCredentialFedExUsername = "51LPnQ5iP1uPARkC";
        private const string TestCredentialFedExPassword = "VYnYkYqui2OIux8DO+Po0YQKvySBei17NsODezd5bRY=";
        private const string TestCredentialFedExCertificateVerificationData = "<Subject><Value></Value><Value><Value></Subject>";

        private const string TestCredentialUpsUserId = "6863e0f62cdd4a1b";
        private const string TestCredentialUpsPassword = "cf9e7473";
        private const string TestCredentialUpsAccessKey = "YbeKtEkBXqxQYcW0MonRIXPCPFKuLQ6l";
        private const string TestCredentialUpsCertificateVerificationData = "<Subject><Value></Value><Value><Value></Subject>";

        private const string TestCredentialExpress1EndiciaAccountNumber = "ba66e5d7-5224-4273-a7e4-6176e2b06d7b";
        private const string TestCredentialExpress1EndiciaPassPhrase = "Y71yGErhEfgAqBkCprcEXA==";
        private const string TestCredentialExpress1EndiciaCertificateVerificationData = "<Subject><Value></Value><Value><Value></Subject>";

        private const string TestCredentialExpress1StampsUsername = "759cc789-25ab-4701-b791-b0c7d4b47701";
        private const string TestCredentialExpress1StampsPassword = "nqsNMvjHqa3u3qX1qav5BldJ+6deGykO4i/B3T3YR/1PTXRSkBcTfA==";
        private const string TestCredentialExpress1StampsCertificateVerificationData = "<Subject><Value></Value><Value><Value></Subject>";
       
        // Key names of credential values in the dictionary 
        private const string FedExAccountNumberKeyName = "FedExAccountNumber";
        private const string FedExMeterNumberKeyName = "FedExMeterNumber";
        private const string FedExUserNameKeyName = "FedExUsername";
        private const string FedExPasswordKeyName = "FedExPassword";
        private const string FedExCertificateVerificationDataKeyName = "FedExCertificateVerificationData";

        private const string UpsUserIdKeyName = "UpsUserId";
        private const string UpsPasswordKeyName = "UpsPassword";
        private const string UpsAccessKeyKeyName = "UpsAccessKey";
        private const string UpsCertificateVerificationDataKeyName = "UpsCertificateVerificationData";

        private const string Express1EndiciaAccountNumberKeyName = "Express1EndiciaAccountNumber";
        private const string Express1EndiciaPassPhraseKeyName = "Express1EndiciaPassPhrase";
        private const string Express1EndiciaCertificateVerificationDataKeyName = "Express1EndiciaCertificateVerificationData";

        private const string Express1StampsUsernameKeyName = "Express1StampsUsername";
        private const string Express1StampsPasswordKeyName = "Express1StampsPassword";
        private const string Express1StampsCertificateVerificationDataKeyName = "Express1StampsCertificateVerificationData";
        
        
        /// <summary>
        /// Private constructor.
        /// </summary>
        private TangoCounterRatesCredentialStore()
        {
            lastFailure = DateTime.MinValue;
            productionCredentials = new Dictionary<string, string>();
        }
        
        /// <summary>
        /// The instance of this Tangle Credential store singleton.
        /// </summary>
        public static TangoCounterRatesCredentialStore Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        /// <summary>
        /// The ShipWorks FedEx account number
        /// </summary>
        public string FedExAccountNumber
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ? 
                    TestCredentialFedExAccountNumber : GetCredentialValue(FedExAccountNumberKeyName);
            }
        }

        /// <summary>
        /// The ShipWorks FedEx meter number
        /// </summary>
        public string FedExMeterNumber
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ? 
                    TestCredentialFedExMeterNumber : GetCredentialValue(FedExMeterNumberKeyName);
            }
        }

        /// <summary>
        /// The ShipWorks FedEx user name
        /// </summary>
        public string FedExUsername
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ? 
                    TestCredentialFedExUsername : GetCredentialValue(FedExUserNameKeyName);
            }
        }

        /// <summary>
        /// The ShipWorks FedEx password
        /// </summary>
        public string FedExPassword
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ? 
                    TestCredentialFedExPassword : GetCredentialValue(FedExPasswordKeyName);
            }
        }

        /// <summary>
        /// Gets data to verify the SSL certificate from FedEx
        /// </summary>
        public string FedExCertificateVerificationData
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ?
                    TestCredentialFedExCertificateVerificationData : GetCredentialValue(FedExCertificateVerificationDataKeyName);
            }
        }

        /// <summary>
        /// Gets the Ups user id used for obtaining counter rates
        /// </summary>
        public string UpsUserId
        {
            get
            {
                return UpsWebClient.UseTestServer ? 
                    TestCredentialUpsUserId : GetCredentialValue(UpsUserIdKeyName);
            }
        }

        /// <summary>
        /// Gets the Ups password used for obtaining counter rates
        /// </summary>
        public string UpsPassword
        {
            get
            {
                return UpsWebClient.UseTestServer ? 
                    TestCredentialUpsPassword : GetCredentialValue(UpsPasswordKeyName);
            }
        }

        /// <summary>
        /// Gets the Ups access key used for obtaining counter rates
        /// </summary>
        public string UpsAccessKey
        {
            get
            {
                return UpsWebClient.UseTestServer ? 
                    TestCredentialUpsAccessKey : GetCredentialValue(UpsAccessKeyKeyName);
            }
        }

        /// <summary>
        /// Gets data to verify the SSL certificate from Ups
        /// </summary>
        public string UpsCertificateVerificationData
        {
            get
            {
                return UpsWebClient.UseTestServer ?
                    TestCredentialUpsCertificateVerificationData : GetCredentialValue(UpsCertificateVerificationDataKeyName);
            }
        }

        /// <summary>
        /// Gets the Express1 Endicia account number used for obtaining counter rates
        /// </summary>
        public string Express1EndiciaAccountNumber
        {
            get
            {
                return Express1EndiciaUtility.UseTestServer ? 
                    TestCredentialExpress1EndiciaAccountNumber : GetCredentialValue(Express1EndiciaAccountNumberKeyName);
            }
        }

        /// <summary>
        /// Gets the Express1 Endicia pass phrase used for obtaining counter rates
        /// </summary>
        public string Express1EndiciaPassPhrase
        {
            get
            {
                return Express1EndiciaUtility.UseTestServer ? 
                    TestCredentialExpress1EndiciaPassPhrase : GetCredentialValue(Express1EndiciaPassPhraseKeyName);
            }
        }

        /// <summary>
        /// Gets data to verify the SSL certificate from Express1Endicia
        /// </summary>
        public string Express1EndiciaCertificateVerificationData
        {
            get
            {
                return Express1EndiciaUtility.UseTestServer ?
                    TestCredentialExpress1EndiciaCertificateVerificationData : GetCredentialValue(Express1EndiciaCertificateVerificationDataKeyName);
            }
        }

        /// <summary>
        /// Gets the Express1 Stamps user name used for obtaining counter rates
        /// </summary>
        public string Express1StampsUsername
        {
            get
            {
                return Express1StampsConnectionDetails.UseTestServer ? 
                    TestCredentialExpress1StampsUsername : GetCredentialValue(Express1StampsUsernameKeyName);
            }
        }

        /// <summary>
        /// Gets the Express1 Stamps password used for obtaining counter rates
        /// </summary>
        public string Express1StampsPassword
        {
            get
            {
                return Express1StampsConnectionDetails.UseTestServer ? 
                    TestCredentialExpress1StampsPassword : GetCredentialValue(Express1StampsPasswordKeyName);
            }
        }

        /// <summary>
        /// Gets data to verify the SSL certificate from Express1Stamps
        /// </summary>
        public string Express1StampsCertificateVerificationData
        {
            get
            {
                return Express1StampsConnectionDetails.UseTestServer ?
                    TestCredentialExpress1StampsCertificateVerificationData : GetCredentialValue(Express1StampsCertificateVerificationDataKeyName);
            }
        }

        /// <summary>
        /// Uses the key name provided to get the corresponding value from the credential dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for the given key.</returns>
        private string GetCredentialValue(string key)
        {
            if (!productionCredentials.Any())
            {
                // The credentials haven't been populated yet, so we need to get them
                // from Tango
                LoadTangoProductionCredentials();
            }

            // Each counter rates broker will check for empty credentials to avoid hitting
            // web services with no credentials
            return productionCredentials.ContainsKey(key) ? productionCredentials[key] : string.Empty;
        }

        /// <summary>
        /// Makes a request to Tango to obtain the account credentials that should be
        /// used for requesting counter rates in a provider's production environment.
        /// </summary>
        private void LoadTangoProductionCredentials()
        {
            if (!productionCredentials.Any())
            {
                if (DateTime.UtcNow.Subtract(lastFailure).TotalSeconds < 30)
                {
                    // The 30 second delay from the last failed request hasn't elapsed , so just throw the
                    // shipping exception here.
                    throw new MissingCounterRatesCredentialException("Unable to get counter rates.");
                }

                lock (lockObject)
                {
                    if (!productionCredentials.Any())
                    {
                        try
                        {
                            StoreEntity firstStore = StoreManager.GetAllStores().OrderByDescending(s => s.Enabled).FirstOrDefault();
                            if (firstStore == null)
                            {
                                return;
                            }

                            // Try to get the counter rates credentials from Tango
                            productionCredentials = TangoWebClient.GetCounterRatesCredentials(firstStore);
                        }
                        catch (TangoException ex)
                        {
                            log.Error(ex);

                            // Note the time of the failure, so we can throttle any subsequent 
                            // calls to Tango before throwing the exception;
                            lastFailure = DateTime.UtcNow;
                            throw new MissingCounterRatesCredentialException("Unable to get counter rates.", ex);
                        }

                        // Check the result...make sure it's not null, has more than 0 entries, and at least has the FedExAccountNumber
                        if (!productionCredentials.Any() || !productionCredentials.ContainsKey(FedExAccountNumberKeyName))
                        {
                            // We still don't have any credentials
                            lastFailure = DateTime.UtcNow;
                            throw new MissingCounterRatesCredentialException("Unable to get counter rates.");
                        }
                    }
                }
            }
        }
    }
}
