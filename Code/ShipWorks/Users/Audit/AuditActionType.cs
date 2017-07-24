using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Enumerates the types of events that can be audited
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum AuditActionType
    {
        /// <summary>
        /// This is when multiple events make up a single Audit, and the ultimate ActionType of those combined events has not yet been determined.
        /// </summary>
        [Description("Preparing...")]
        [ImageResource("hourglass16")]
        Undetermined = 0,

        /// <summary>
        /// Log on to ShipWorks
        /// </summary>
        [Description("Log On")]
        [ImageResource("lock_ok16")]
        Logon = 1,

        /// <summary>
        /// Log off of ShipWorks
        /// </summary>
        [Description("Log Off")]
        [ImageResource("lock16")]
        Logoff = 2,

        /// <summary>
        /// Upgrade the ShipWorks database.
        /// </summary>
        [Description("Upgrade Database")]
        [ImageResource("data_out16")]
        UpgradeDatabase = 3,

        /// <summary>
        /// Backup the ShipWorks database
        /// </summary>
        [Description("Backup Database")]
        [ImageResource("data_disk16")]
        BackupDatabase = 4,

        /// <summary>
        /// Something was added
        /// </summary>
        [Description("Add")]
        [ImageResource("add16")]
        Insert = 5,

        /// <summary>
        /// Something was edited
        /// </summary>
        [Description("Edit")]
        [ImageResource("edit16")]
        Update = 6,

        /// <summary>
        /// Something was deleted
        /// </summary>
        [Description("Delete")]
        [ImageResource("delete16")]
        Delete = 7,

        /// <summary>
        /// The ShipSense knowledge base has been reset
        /// </summary>
        [Description("Reset ShipSense")]
        [ImageResource("delete16")]
        ResetShipSense = 8,

        /// <summary>
        /// A reload of the knowledge base was started.
        /// </summary>
        [Description("Reload ShipSense started")]
        [ImageResource("arrows_green_static")]
        ReloadShipSenseStarted = 9,

        /// <summary>
        /// Orders were combined.
        /// </summary>
        [Description("Combine Order")]
        [ImageResource("arrows_green_static")]
        CombineOrder = 10
    }
}
