using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Data.Administration.VersionSpecifcUpdates
{
    /// <summary>
    /// ShipWorks update that should be applied for a specific version
    /// </summary>
    [Service]
    public interface IVersionSpecificUpdate
    {
        /// <summary>
        /// To which version does this update apply
        /// </summary>
        Version AppliesTo { get; }

        /// <summary>
        /// Execute the update
        /// </summary>
        void Update();
    }
}