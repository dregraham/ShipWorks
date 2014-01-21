using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Singleton counter credential store that holds ShipWorks accounts for shipping carriers used to get counter rates.
    /// </summary>
    public class TangoCounterRatesCredentialStore : ICounterRatesCredentialStore
    {
        // The lazy loaded singleton instance variable.
        private static readonly Lazy<TangoCounterRatesCredentialStore> lazyInstance = 
            new Lazy<TangoCounterRatesCredentialStore>(() => new TangoCounterRatesCredentialStore());

        /// <summary>
        /// Private constructor.
        /// </summary>
        private TangoCounterRatesCredentialStore()
        {
            // TODO: Populate with values from Tango when Tango supports these.
            FedExAccountNumber = "224813333";
            FedExMeterNumber = "118590367";
            FedExUsername = "7prplPors913iUfh";
            FedExPassword = "+//I8JlHYCTg43+YVbmfh9RcO2YvwYhlrj99+ccITPw=";
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
            get; 
            private set;
        }

        /// <summary>
        /// The ShipWorks FedEx meter number
        /// </summary>
        public string FedExMeterNumber
        {
            get; 
            private set;
        }

        /// <summary>
        /// The ShipWorks FedEx username
        /// </summary>
        public string FedExUsername
        {
            get; 
            private set;
        }

        /// <summary>
        /// The ShipWorks FedEx password
        /// </summary>
        public string FedExPassword
        {
            get; 
            private set;
        }
    }
}
