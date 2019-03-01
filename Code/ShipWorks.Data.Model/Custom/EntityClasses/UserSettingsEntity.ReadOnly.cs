using System;
using Interapptive.Shared.Utility;
using ShipWorks.Shared.Users;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Custom code for the ReadOnlyUserSettingsEntity
    /// </summary>
    public partial class ReadOnlyUserSettingsEntity
    {
        /// <summary>
        /// Get an object for the DialogSettings XML
        /// </summary>
        public DialogSettings DialogSettingsObject =>
            string.IsNullOrWhiteSpace(this.DialogSettings) ?
                new DialogSettings() :
                SerializationUtility.DeserializeFromXml<DialogSettings>(this.DialogSettings);

        /// <summary>
        /// Get the last version of release notes seen by the user
        /// </summary>
        public Version LastReleaseNotesSeenVersion => Version.Parse(LastReleaseNotesSeen);
    }
}
