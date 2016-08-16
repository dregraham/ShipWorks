﻿using Interapptive.Shared.Security;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Security.Cryptography;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the license information from the database
    /// </summary>
    public class CustomerLicenseReader : ICustomerLicenseReader
    {
        private readonly IConfigurationData configurationData;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProviderFactory encryptionProviderFactory,
            Func<Type, ILog> logFactory,
            IConfigurationData configurationData)
        {
            this.configurationData = configurationData;
            this.encryptionProviderFactory = encryptionProviderFactory;
            log = logFactory(typeof(CustomerLicenseReader));
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            IEncryptionProvider encryptionProvider = encryptionProviderFactory.CreateLicenseEncryptionProvider();
            ConfigurationEntity config = configurationData.Fetch();

            try
            {
                string customerKey = encryptionProvider.Decrypt(config.CustomerKey);
                return customerKey;
            }
            catch (ORMEntityOutOfSyncException ex)
            {
                // This exception was rarely thrown by config.CustomerKey, but it did happen. Hopefully this will 
                // keep it from hapenning again... I could also putting some sort of sleep in here (a couple of seconds)
                // but we'll try this first.
                log.Error(ex);
                return RefreshConfigAndRetryRead(encryptionProvider);
            }
            catch (EncryptionException ex) when (ex.InnerException is CryptographicException)
            {
                // This was hapenning when the key and the databaseid didn't match. This would happen when switching 
                // or restoring a database and shouldn't happen.
                // I don't know if it is hapenning anymore...
                log.Error(ex);
                return RefreshConfigAndRetryRead(encryptionProvider);
            }
        }

        /// <summary>
        /// Retries the read.
        /// </summary>
        private string RefreshConfigAndRetryRead(IEncryptionProvider encryptionProvider)
        {
            configurationData.CheckForChangesNeeded();
            ConfigurationEntity config = configurationData.Fetch();

            return encryptionProvider.Decrypt(config.CustomerKey);
        }
    }
}
