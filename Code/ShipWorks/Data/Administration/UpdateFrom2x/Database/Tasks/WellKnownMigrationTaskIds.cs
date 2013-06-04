using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// Strongly typed task identifiers
    /// </summary>
    public static class WellKnownMigrationTaskIds
    {
        // do not change these values, once they have been serialized they need to remain
        public const string UpdateV2Schema = "UpdateV2Schema";
        public const string PrepareDatabase = "PrepareDatabase";
        public const string CheckConstraints = "CheckConstraints";
        public const string ConvertStatusCodes = "ConvertStatusCodes";
        public const string SslCertificateImport = "SslCertificateImport";
        public const string UpdateSchemaVersion = "UpdateSchemaVersion";
        public const string LoadStatusPresets = "LoadStatusPresets";
        public const string UpsNotificationUpgrade = "UpsNotificationUpgrade";
        public const string UpgradePaymentDetails = "UpgradePaymentDetails";
    }
}
