using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;

namespace ShipWorks.ApplicationCore.Security
{
    /// <summary>
    /// An ICipherKey implementation specific to the Hub Config.
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    [KeyedComponent(typeof(ICipherKey), CipherContext.HubConfig)]
    public class HubConfigCipherKey : ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector => new Guid("{65EC42FF-A4FC-4EA8-8FDD-B8EC39C51C7D}").ToByteArray();

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key => new Guid("{1E7ADEBD-6525-41AD-875D-874B27E84569}").ToByteArray();
    }
}
