using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Applies fuel surcharge to air service rate
    /// </summary>
    public class FuelAirSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        public FuelAirSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (serviceRate.Service != UpsServiceType.UpsGround)
            {
                decimal fuelSurchargeRate = (decimal) surcharges[UpsSurchargeType.FuelAir];
                decimal fuelSurcharge = Math.Round(serviceRate.Amount * fuelSurchargeRate, 2, MidpointRounding.AwayFromZero);

                serviceRate.AddAmount(fuelSurcharge, $"Air Fuel Surcharge of {fuelSurchargeRate}");
            }
        }
    }
}
