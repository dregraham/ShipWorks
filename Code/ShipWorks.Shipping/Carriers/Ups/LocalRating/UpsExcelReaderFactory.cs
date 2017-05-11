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
        public IEnumerable<IUpsRateExcelReader> CreateRateExcelReaders()
        {
            return new IUpsRateExcelReader[]
            {
                new ServiceUpsRateExcelReader(),
                new SurchargeUpsRateExcelReader()
            };
        }

        /// <summary>
        /// Create the zone excel readers
        /// </summary>
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