using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// An ICipherKey implementation specific to the Overstock store type.
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    [KeyedComponent(typeof(ICipherKey), CipherContext.Overstock)]
    public class OverstockCipherKey : ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector =>
            new byte[] { 6, 252, 121, 60, 240, 21, 105, 6, 75, 10, 41, 212, 24, 30, 228, 235 };

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key => new Guid("{BFFD861A-8FA1-43F3-8A1A-1D7C523861D3}").ToByteArray();
    }
}
