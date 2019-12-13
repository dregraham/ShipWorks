using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// An ICipherKey implementation specific to the Rakuten store type.
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    [KeyedComponent(typeof(ICipherKey), CipherContext.Rakuten)]
    public class RakutenCipherKey : ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector => new Guid("{7B95379C-B5BF-424F-8FEB-2C65992F71DD}").ToByteArray();

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key => new Guid("{D0A062A2-D057-4351-87F4-3A5BBF8774DE}").ToByteArray();
    }
}
