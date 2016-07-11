using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Strategy on how to upload shipment details for Odbc
    /// </summary>
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