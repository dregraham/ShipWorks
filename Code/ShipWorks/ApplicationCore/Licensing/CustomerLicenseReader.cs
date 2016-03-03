using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Data.SqlClient;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the license information from the database
    /// </summary>
    public class CustomerLicenseReader : ICustomerLicenseReader
    {
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProvider encryptionProvider)
        {
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            ConfigurationEntity config = ConfigurationData.Fetch();

            try
            {
                return encryptionProvider.Decrypt(config.CustomerKey);
            }
            catch (EncryptionException ex) when(ex.InnerException.InnerException.InnerException is SqlException)
            {
                SqlException sqlEx = (SqlException)ex.InnerException.InnerException.InnerException;

                // Could not find stored procedure GetDataGuid, we must be in the process
                // of upgrading/restoring a legacy database, return empty string for the key
                if (sqlEx.Number == 2812)
                {
                    return string.Empty;
                }
                throw;
            }
        }
    }
}
