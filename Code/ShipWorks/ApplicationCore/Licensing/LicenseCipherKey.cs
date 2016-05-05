using Interapptive.Shared.Security;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// An ICipherKey implementation specific to the customer licensing.
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    public class LicenseCipherKey : ICipherKey
    {
        private readonly IDatabaseIdentifier databaseIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseCipherKey" /> class.
        /// </summary>
        /// <param name="databaseIdentifier">The database identifier to use for generating the key.</param>
        public LicenseCipherKey(IDatabaseIdentifier databaseIdentifier)
        {
            this.databaseIdentifier = databaseIdentifier;
        }

        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector
        {
            get {return new byte[] { 125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14 }; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <exception cref="DatabaseIdentifierException">Can throw a DatabaseIdentifierException if there is a problem
        /// connecting to the database to obtain the key.</exception>
        public byte[] Key
        {
            get
            {
                try
                {
                    return databaseIdentifier.Get().ToByteArray();
                }
                catch (DatabaseIdentifierException ex)
                {
                    throw new EncryptionException(ex.Message, ex);
                }
            }
        }
    }
}
