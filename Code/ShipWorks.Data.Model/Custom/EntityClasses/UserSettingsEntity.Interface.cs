using System;
using ShipWorks.Shared.Users;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Custom code for the UserSettingsEntity
    /// </summary>
    public partial interface IUserSettingsEntity
    {
        /// <summary>
        /// Get an object for the DialogSettings XML
        /// </summary>
        DialogSettings DialogSettingsObject { get; }

        /// <summary>
        /// Get the last version of release notes seen by the user
        /// </summary>
        Version LastReleaseNotesSeenVersion { get; }
    }
}
