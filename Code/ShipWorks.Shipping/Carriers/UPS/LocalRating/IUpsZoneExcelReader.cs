using Interapptive.Shared.ComponentRegistration;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Reads the ups zone excel work sheets and stores the zone info in to the UpsLocalRateTable
    /// </summary>
    [Service]
    public interface IUpsZoneExcelReader
    {
        /// <summary>
        ///  Reads the ups zone excel work sheets and stores the zone info in to the UpsLocalRateTable
        /// </summary>
        void Read(IWorksheets zoneWorksheets, IUpsLocalRateTable upsLocalRateTable);
    }
}
