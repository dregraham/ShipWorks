using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters
{
    /// <summary>
    /// Filters out services based on what value added services are applied
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters.IServiceFilter" />
    public class ValueAddServiceFilter : IServiceFilter
    {
        /// <summary>
        /// Gets the list of eligible service types for the given shipment, from the given list of service types
        /// </summary>
        public IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment, IEnumerable<UpsServiceType> services)
        {
            return services.Where(service => IsServiceEligible(shipment, service));
        }

        /// <summary>
        /// Determines whether [is service eligible] [the specified shipment].
        /// </summary>
        private bool IsServiceEligible(UpsShipmentEntity shipment, UpsServiceType service)
        {
            // NextDayAirEarly does not work with delivery confirmation 
            if (service == UpsServiceType.UpsNextDayAirAM)
            {
                if (shipment.DeliveryConfirmation == (int) UpsDeliveryConfirmationType.NoSignature ||
                    shipment.DeliveryConfirmation == (int) UpsDeliveryConfirmationType.Signature)
                {
                    return false;
                }
            }

            // VerbalConfirmation is only compatible with UpsNextDayAirEarly
            if (shipment.Packages.Any(p => p.VerbalConfirmationEnabled) && service != UpsServiceType.UpsNextDayAirAM)
            {
                return false;
            }

            if (shipment.SaturdayDelivery && !IsSaturdayService(service))
            {
                return false;
            }

            if (shipment.Shipment.ReturnShipment && !IsReturnService(service))
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Determines whether [is return service] [the specified service].
        /// </summary>
        private bool IsReturnService(UpsServiceType service)
        {
            return service != UpsServiceType.Ups2DayAirAM && service != UpsServiceType.UpsNextDayAirSaver;
        }

        /// <summary>
        /// Determines whether [is saturday service] [the specified service].
        /// </summary>
        private bool IsSaturdayService(UpsServiceType service)
        {
            return service != UpsServiceType.Ups2DayAirAM && service != UpsServiceType.UpsNextDayAirSaver;
        }
    }
}