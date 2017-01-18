using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
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
        /// Should this run whenever the user upgrades ShipWorks
        /// </summary>
        /// <remarks>
        /// This was put into place because we can't guarantee VersionSpecificUpdates actually ran during a previous upgrade.
        /// Once we can guarantee it runs, we should remove this property.
        /// </remarks>
        bool AlwaysRun { get; }

        /// <summary>
        /// Execute the update
        /// </summary>
        void Update();
    }
}