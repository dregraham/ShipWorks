using System.Data;
using Interapptive.Shared.Security;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the license information from the database
    /// </summary>
    public class CustomerLicenseReader : ICustomerLicenseReader
    {
        private readonly IDatabaseIdentifier databaseIdentifier;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProviderFactory encryptionProviderFactory,
            ISqlAdapterFactory sqlAdapterFactory,
            IDatabaseIdentifier databaseIdentifier
        )
        {
            this.databaseIdentifier = databaseIdentifier;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            IEncryptionProvider encryptionProvider = encryptionProviderFactory.CreateLicenseEncryptionProvider();
            string customerKey = string.Empty;

            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(ConfigurationFields.CustomerKey, 0, "CustomerKey", "");

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                IDataReader reader = adapter.FetchDataReader(resultFields, null, CommandBehavior.CloseConnection, 0, null, true);
                while (reader.Read())
                {
                    customerKey = reader.GetString(0);
                }
            }

            // try to decrypt the key, if we get an ExcryptionException try setting the cached database identifier
            // and try again. This happens when switching between databases or restoring a database with a different database identifier
            try
            {
                return encryptionProvider.Decrypt(customerKey);
            }
            catch (EncryptionException)
            {
                databaseIdentifier.Reset();
                return encryptionProvider.Decrypt(customerKey);
            }
        }
    }
}
