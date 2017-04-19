using ShipWorks.ApplicationCore.ComponentRegistration;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Reads the ups rates excel work sheets and store the rates in to the UpsLocalRateTable
    /// </summary>
    [Service]
    public interface IUpsRateExcelReader
    {
        /// <summary>
        /// Reads the ups rates excel work sheets and store the rates in to the UpsLocalRateTable
        /// </summary>
        void Read(IWorksheets rateWorkSheets, IUpsLocalRateTable upsLocalRateTable);
    }
}
