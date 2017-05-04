using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters
{
    /// <summary>
    /// Filter for determining eligible Ups services, based on packaging type
    /// </summary>
    /// <seealso cref="IServiceFilter" />
    public class PackageServiceFilter : IServiceFilter
    {
        /// <summary>
        /// Gets the list of eligible service types for the given shipment, from the given list of service types, based on packaging type
        /// </summary>
        public IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment,
            IEnumerable<UpsServiceType> services)
        {
            List<UpsServiceType> eligibleServices = services as List<UpsServiceType> ?? services.ToList();

            foreach (UpsPackageEntity package in shipment.Packages)
            {
                if (package.PackagingType != (int) UpsPackagingType.Custom)
                {
                    eligibleServices.Remove(UpsServiceType.UpsGround);
                    eligibleServices.Remove(UpsServiceType.Ups3DaySelect);
                }

                if (package.PackagingType != (int) UpsPackagingType.Custom &&
                    package.PackagingType != (int) UpsPackagingType.Letter &&
                    package.PackagingType != (int) UpsPackagingType.Tube &&
                    package.PackagingType != (int) UpsPackagingType.BoxExpress &&
                    package.PackagingType != (int) UpsPackagingType.BoxExpressSmall &&
                    package.PackagingType != (int) UpsPackagingType.BoxExpressMedium &&
                    package.PackagingType != (int) UpsPackagingType.BoxExpressLarge &&
                    package.PackagingType != (int) UpsPackagingType.ExpressEnvelope &&
                    package.PackagingType != (int) UpsPackagingType.Pak)
                {
                    eligibleServices.Remove(UpsServiceType.Ups2DayAirAM);
                    eligibleServices.Remove(UpsServiceType.Ups2DayAir);
                    eligibleServices.Remove(UpsServiceType.UpsNextDayAirAM);
                    eligibleServices.Remove(UpsServiceType.UpsNextDayAir);
                    eligibleServices.Remove(UpsServiceType.UpsNextDayAirSaver);
                }
            }

            return eligibleServices;
        }
    }
}