using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// Typecode for each distinct MigrationTaskBase class. 
    /// </summary>
    public enum MigrationTaskTypeCode
    {
        PrepareDatabaseTask = 0,
        UpgradeV2MigrationTask = 1,
        ScriptMigrationTask = 2,
        CheckConstraintsTask = 4,
        ConvertStatusCodesTask = 6,
        SslCertificateImportTask = 8,
        UpdateSchemaVersionTask = 10,
        LoadStatusPresetsTask = 12,
	    UpsNotificationUpgradeTask = 13,
        UpgradePaymentDetails = 14
    }
}
