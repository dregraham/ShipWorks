using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Singleton counter credential store that holds ShipWorks accounts for shipping carriers used to get counter rates.
    /// </summary>
    public class TangoCounterRatesCredentialStore : ICounterRatesCredentialStore
    {
        private static FedExSettingsRepository fedExSettingsRepo = new FedExSettingsRepository();

        // The lazy loaded singleton instance variable.
        private static readonly Lazy<TangoCounterRatesCredentialStore> lazyInstance = 
            new Lazy<TangoCounterRatesCredentialStore>(() => new TangoCounterRatesCredentialStore());

        // Test credentials
        private const string testCredentialFedExAccountNumber = "603103343";
        private const string testCredentialFedExMeterNumber = "118601174";
        private const string testCredentialFedExUsername = "51LPnQ5iP1uPARkC";
        private const string testCredentialFedExPassword = "VYnYkYqui2OIux8DO+Po0YQKvySBei17NsODezd5bRY=";
        private const string testCredentialUpsUserId = "6863e0f62cdd4a1b";
        private const string testCredentialUpsPassword = "cf9e7473";
        private const string testCredentialUpsAccessKey = "YbeKtEkBXqxQYcW0MonRIXPCPFKuLQ6l";
        private const string testCredentialExpress1EndiciaAccountNumber = "ba66e5d7-5224-4273-a7e4-6176e2b06d7b";
        private const string testCredentialExpress1EndiciaPassPhrase = "Y71yGErhEfgAqBkCprcEXA==";
        private const string testCredentialExpress1StampsUsername = "759cc789-25ab-4701-b791-b0c7d4b47701";
        private const string testCredentialExpress1StampsPassword = "nqsNMvjHqa3u3qX1qav5BldJ+6deGykO4i/B3T3YR/1PTXRSkBcTfA==";

        // Production credentials
        private string fedExAccountNumber = "";
        private string fedExMeterNumber = "";
        private string fedExUsername = "";
        private string fedExPassword = "";
        private string upsUserId = "";
        private string upsPassword = "";
        private string upsAccessKey = "";
        private string express1EndiciaAccountNumber = "";
        private string express1EndiciaPassPhrase = "";
        private string express1StampsUsername = "";
        private string express1StampsPassword = "";

        /// <summary>
        /// Private constructor.
        /// </summary>
        private TangoCounterRatesCredentialStore()
        {
        }

        /// <summary>
        /// Calls Tango to get counter rates credentials
        /// </summary>
        private void LoadTangoProductionCredentials()
        {
            Dictionary<string, string> creds;

            // Try to get the counter rates credentials from Tango
            try
            {
                creds = TangoWebClient.GetCounterRatesCredentials();
            }
            catch (TangoException)
            {
                throw new ShippingException("Unable to get counter rates.");
            }

            // Check the result...make sure it's not null, has more than 0 entries, and at least has the FedExAccountNumber
            if (creds == null || creds.Count == 0 || !creds.ContainsKey("FedExAccountNumber"))
            {
                throw new ShippingException("Unable to get counter rates.");
            }

            fedExAccountNumber = creds["FedExAccountNumber"];
            fedExMeterNumber = creds["FedExMeterNumber"];
            fedExUsername = creds["FedExUsername"];
            fedExPassword = creds["FedExPassword"];
            upsUserId = creds["UpsUserId"];
            upsPassword = creds["UpsPassword"];
            upsAccessKey = creds["UpsAccessKey"];
            express1EndiciaAccountNumber = creds["Express1EndiciaAccountNumber"];
            express1EndiciaPassPhrase = creds["Express1EndiciaPassPhrase"];
            express1StampsUsername = creds["Express1StampsUsername"];
            express1StampsPassword = creds["Express1StampsPassword"];
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
                return fedExSettingsRepo.UseTestServer ? testCredentialFedExAccountNumber : fedExAccountNumber;
            }
        }

        /// <summary>
        /// The ShipWorks FedEx meter number
        /// </summary>
        public string FedExMeterNumber
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ? testCredentialFedExMeterNumber : fedExMeterNumber;
            }
        }

        /// <summary>
        /// The ShipWorks FedEx username
        /// </summary>
        public string FedExUsername
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ? testCredentialFedExUsername : fedExUsername;
            }
        }

        /// <summary>
        /// The ShipWorks FedEx password
        /// </summary>
        public string FedExPassword
        {
            get
            {
                return fedExSettingsRepo.UseTestServer ? testCredentialFedExPassword : fedExPassword;
            }
        }

        /// <summary>
        /// Gets the Ups user id used for obtaining counter rates
        /// </summary>
        public string UpsUserId
        {
            get
            {
                return UpsWebClient.UseTestServer ? testCredentialUpsUserId : upsUserId;
            }
        }

        /// <summary>
        /// Gets the Ups password used for obtaining counter rates
        /// </summary>
        public string UpsPassword
        {
            get
            {
                return UpsWebClient.UseTestServer ? testCredentialUpsPassword : upsPassword;
            }
        }

        /// <summary>
        /// Gets the Ups access key used for obtaining counter rates
        /// </summary>
        public string UpsAccessKey
        {
            get
            {
                return UpsWebClient.UseTestServer ? testCredentialUpsAccessKey : upsAccessKey;
            }
        }

        /// <summary>
        /// Gets the Express1 Endicia account number used for obtaining counter rates
        /// </summary>
        public string Express1EndiciaAccountNumber
        {
            get
            {
                return Express1EndiciaUtility.UseTestServer ? testCredentialExpress1EndiciaAccountNumber : express1EndiciaAccountNumber;
            }
        }

        /// <summary>
        /// Gets the Express1 Endicia pass phrase used for obtaining counter rates
        /// </summary>
        public string Express1EndiciaPassPhrase
        {
            get
            {
                return Express1EndiciaUtility.UseTestServer ? testCredentialExpress1EndiciaPassPhrase : express1EndiciaPassPhrase;
            }
        }
        /// <summary>
        /// Gets the Express1 Stamps user name used for obtaining counter rates
        /// </summary>
        public string Express1StampsUsername
        {
            get
            {
                return Express1StampsConnectionDetails.UseTestServer ? testCredentialExpress1StampsUsername : express1StampsUsername;
            }
        }

        /// <summary>
        /// Gets the Express1 Stamps password used for obtaining counter rates
        /// </summary>
        public string Express1StampsPassword
        {
            get
            {
                return Express1StampsConnectionDetails.UseTestServer ? testCredentialExpress1StampsPassword : express1StampsPassword;
            }
        }
    }
}
