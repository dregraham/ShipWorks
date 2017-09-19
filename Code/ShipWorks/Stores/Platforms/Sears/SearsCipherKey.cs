using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// An ICipherKey implementation specific to the Sears store type.
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    [KeyedComponent(typeof(ICipherKey), CipherContext.Sears)]
    public class SearsCipherKey : ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector =>
            new byte[] { 84, 104, 101, 68, 111, 111, 115, 107, 101, 114, 110, 111, 111, 100, 108, 101 };

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key => new Guid("{A2FC95D9-F255-4D23-B86C-756889A51C6A}").ToByteArray();
    }
}
