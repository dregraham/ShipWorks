using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Represents the ShipWorksSession
    /// </summary>
    [Service]
    public interface IShipWorksSession
    {
        /// <summary>
        /// The ShipWorks ID that is unique to the current installation path
        /// </summary>
        Guid InstanceID { get; }
    }
}