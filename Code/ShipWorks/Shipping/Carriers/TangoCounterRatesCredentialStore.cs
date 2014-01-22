﻿using System;
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
            FedExAccountNumber = "603103343";
            FedExMeterNumber = "118601174";
            FedExUsername = "51LPnQ5iP1uPARkC";
            FedExPassword = "VYnYkYqui2OIux8DO+Po0YQKvySBei17NsODezd5bRY=";
            UpsUserId = "6863e0f62cdd4a1b";
            UpsPassword = "cf9e7473";
            UpsAccessKey = "YbeKtEkBXqxQYcW0MonRIXPCPFKuLQ6l";
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

        /// <summary>
        /// Gets the Ups user id used for obtaining counter rates
        /// </summary>
        public string UpsUserId
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the Ups password used for obtaining counter rates
        /// </summary>
        public string UpsPassword
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the Ups access key used for obtaining counter rates
        /// </summary>
        public string UpsAccessKey { 
            get; 
            private set; 
        }
    }
}
