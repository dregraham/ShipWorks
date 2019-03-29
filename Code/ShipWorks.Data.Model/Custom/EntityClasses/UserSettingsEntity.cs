using System;
using Interapptive.Shared.Utility;
using ShipWorks.Shared.Users;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom code for the UserSettings Entity
    /// </summary>
    public partial class UserSettingsEntity
    {
        /// <summary>
        /// Get or set an object for the DialogSettings XML
        /// </summary>
        public DialogSettings DialogSettingsObject
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.DialogSettings) ?
                    new DialogSettings() :
                    SerializationUtility.DeserializeFromXml<DialogSettings>(this.DialogSettings);
            }
            set
            {
                DialogSettings = SerializationUtility.SerializeToXml(value);
            }
        }

        /// <summary>
        /// Get the last version of release notes seen by the user
        /// </summary>
        public Version LastReleaseNotesSeenVersion
        {
            get => Version.Parse(LastReleaseNotesSeen);
            set => LastReleaseNotesSeen = value.ToString();
        }
    }
}
