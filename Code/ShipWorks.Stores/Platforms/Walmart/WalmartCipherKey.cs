using System;
using Interapptive.Shared.Security;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Security;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Cipher key for Walmart encryption
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    [KeyedComponent(typeof(ICipherKey), CipherContext.Walmart)]
    public class WalmartCipherKey : ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector => new Guid("{3D0A04DA-4CC0-4ACB-804A-2C5AFF27D833}").ToByteArray();

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key => new Guid("{5B03A4BF-A70A-4335-B507-8B0C084E4571}").ToByteArray();
    }
}
