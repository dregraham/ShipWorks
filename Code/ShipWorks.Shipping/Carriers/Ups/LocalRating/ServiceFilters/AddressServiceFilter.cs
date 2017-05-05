using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters
{
    /// <summary>
    /// Get eligible servies based on the address
    /// </summary>
    public class AddressServiceFilter : IServiceFilter
    {
        private readonly IResidentialDeterminationService residentialDetermination;
        private readonly UpsShipmentType upsShipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressServiceFilter(IResidentialDeterminationService residentialDetermination, UpsShipmentType upsShipmentType)
        {
            this.residentialDetermination = residentialDetermination;
            this.upsShipmentType = upsShipmentType;
        }

        /// <summary>
        /// Get eligible servies based on the address
        /// </summary>
        public IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment, IEnumerable<UpsServiceType> services)
        {
            return services.Where(s => IsServiceEligible(shipment, s));
        }

        /// <summary>
        /// Check to see if the service if available 
        /// </summary>
        private bool IsServiceEligible(UpsShipmentEntity shipment, UpsServiceType serviceType)
        {
            // Ups 2nd Day Air A.M. is not available to residential addresses
            if (residentialDetermination.IsResidentialAddress(shipment.Shipment) && serviceType == UpsServiceType.Ups2DayAirAM)
            {
                return false;
            }
            
            return upsShipmentType.IsDomestic(shipment.Shipment);
        }
    }
}