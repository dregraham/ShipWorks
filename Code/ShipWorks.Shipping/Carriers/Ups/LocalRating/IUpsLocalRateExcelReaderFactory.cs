using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Interface for creating local rate excel readers
    /// </summary>
    public interface IUpsLocalRateExcelReaderFactory
    {
        /// <summary>
        /// Create the rate excel readers
        /// </summary>
        IEnumerable<IUpsRateExcelReader> CreateRateExcelReaders();

        /// <summary>
        /// Create the zone excel readers
        /// </summary>
        IEnumerable<IUpsZoneExcelReader> CreateZoneExcelReaders();
    }
}