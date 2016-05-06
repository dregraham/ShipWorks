using System;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// An ICipherKey implementation specific to the Odbc store type.
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    public class OdbcCipherKey : ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector => new Guid("{6EA39684-7C8F-4985-AA20-CE938413412B}").ToByteArray();

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key => new Guid("{3DDC117C-617F-4F57-A3DB-A3A8865EAD1C}").ToByteArray();
    }
}
