using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Applies Returns surcharge
    /// </summary>
    public class ReturnsSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnsSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the ThirdPartyBilling surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (shipment.Shipment.ReturnShipment)
            {
                int packageCount = shipment.Packages.Count;
                UpsSurchargeType surchargeType = GetSurchargeTypeFromReturnServiceType((UpsReturnServiceType)shipment.ReturnService);
                decimal surchargeAmount = (decimal) (surcharges[surchargeType] * packageCount);

                serviceRate.AddAmount(surchargeAmount, EnumHelper.GetDescription(surchargeType));
            }
        }

        /// <summary>
        /// Get the surcharge type from the return service type
        /// </summary>
        private static UpsSurchargeType GetSurchargeTypeFromReturnServiceType(UpsReturnServiceType returnServiceType)
        {
            switch (returnServiceType)
            {
                case UpsReturnServiceType.ElectronicReturnLabel:
                    return UpsSurchargeType.UpsReturnsElectronicReturnLabel;
                case UpsReturnServiceType.PrintReturnLabel:
                    return UpsSurchargeType.UpsReturnsPrintReturnLabel;
                case UpsReturnServiceType.PrintAndMail:
                    return UpsSurchargeType.UpsReturnsPrintandMail;
                case UpsReturnServiceType.ReturnPlus1:
                    return UpsSurchargeType.UpsReturnsReturnsPlusOneAttempt;
                case UpsReturnServiceType.ReturnPlus3:
                    return UpsSurchargeType.UpsReturnsReturnsPlusThreeAttempts;
                default:
                    throw new UpsLocalRatingException($"Unknown return type {EnumHelper.GetDescription(returnServiceType)}.");
            }
        }
    }
}
