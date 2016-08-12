using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Strategy on how to upload shipment details for Odbc
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcShipmentUploadStrategy
    {
        [ApiValue("Do not upload")]
        [Description("Do not upload shipment details")]
        DoNotUpload = 0,

        [ApiValue("Same data source")]
        [Description("Upload shipment details to the same data source")]
        UseImportDataSource = 1,

        [ApiValue("Different data source")]
        [Description("Upload shipment details to a different data source")]
        UseShipmentDataSource = 2
    }
}