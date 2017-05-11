using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Factory for creating Ups Local Rate Excel Readers
    /// </summary>
    [Component]
    public class UpsLocalRateExcelReaderFactory : IUpsLocalRateExcelReaderFactory
    {
        /// <summary>
        /// Create the rate excel readers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IUpsRateExcelReader> CreateRateExcelReaders()
        {
            return new IUpsRateExcelReader[]
            {
                new ServiceUpsRateExcelReader(),
                new ServiceUpsRateExcelReader()
            };
        }

        /// <summary>
        /// Create the zone excel readers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IUpsZoneExcelReader> CreateZoneExcelReaders()
        {
            return new IUpsZoneExcelReader[]
            {
                new UpsZoneExcelReader(new AlaskaHawaiiZoneExcelReader()),
                new DeliveryAreaSurchargeUpsZoneExcelReader()
            };
        }
    }
}