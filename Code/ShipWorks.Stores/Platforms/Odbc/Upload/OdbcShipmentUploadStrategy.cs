using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Strategy on how to upload shipment details for Odbc
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcShipmentUploadStrategy
    {
        [Description("Do not upload")]
        DoNotUpload = 0,

        [Description("Upload to import data source")]
        UseImportDataSource = 1,

        [Description("Upload to shipment data source")]
        UseShipmentDataSource = 2
    }
}