using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Applies fuel surcharge to ground service rate
    /// </summary>
    public class FuelGroundSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuelGroundSurcharge"/> class.
        /// </summary>
        public FuelGroundSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (serviceRate.Service == UpsServiceType.UpsGround)
            {
                decimal fuelSurchargeRate = (decimal) surcharges[UpsSurchargeType.FuelGround];
                decimal fuelSurcharge = Math.Round(serviceRate.Amount * fuelSurchargeRate, 2, MidpointRounding.AwayFromZero);
                decimal percentageToDisplay = Math.Round(100 * fuelSurchargeRate, 2, MidpointRounding.AwayFromZero);

                serviceRate.AddAmount(fuelSurcharge, $"Ground Fuel Surcharge of {percentageToDisplay}%");
            }
        }
    }
}
