using System;
using System.Linq;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Wrapper for the static ShipWorksSession
    /// </summary>
    public class ShipWorksSessionWrapper : IShipWorksSession
    {
        /// <summary>
        /// The ShipWorks ID that is unique to the current installation path
        /// </summary>
        public Guid InstanceID =>
            ShipWorksSession.InstanceID;
    }
}
