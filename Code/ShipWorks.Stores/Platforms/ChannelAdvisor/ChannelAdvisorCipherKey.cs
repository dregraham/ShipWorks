using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// An ICipherKey implementation specific to the ChannelAdvisor store type.
    /// </summary>
    /// <seealso cref="Interapptive.Shared.Security.ICipherKey" />
    [KeyedComponent(typeof(ICipherKey), CipherContext.ChannelAdvisor)]
    public class ChannelAdvisorCipherKey : ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        public byte[] InitializationVector => new Guid("{648F2837-20A7-45DF-90E0-3DA61C99134D}").ToByteArray();

        /// <summary>
        /// Gets the key.
        /// </summary>
        public byte[] Key => new Guid("{C8B497BA-ED9B-4920-8C29-9F067D9E1DBA}").ToByteArray();
    }
}