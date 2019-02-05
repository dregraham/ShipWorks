using System;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Represents the ShipWorksSession
    /// </summary>
    public interface IShipWorksSession
    {
        /// <summary>
        /// The ShipWorks ID that is unique to the current installation path
        /// </summary>
        Guid InstanceID { get; }
    }
}