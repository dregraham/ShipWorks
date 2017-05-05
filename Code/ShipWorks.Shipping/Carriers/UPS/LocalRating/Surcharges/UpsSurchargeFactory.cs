using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    ///  Returns a collection of surcharges to be applied on a base UPS Rate.
    /// </summary>
    [Component]
    public class UpsSurchargeFactory : IUpsSurchargeFactory
    {
        private readonly IResidentialDeterminationService residentialDeterminationService;

        public UpsSurchargeFactory(IResidentialDeterminationService residentialDeterminationService)
        {
            this.residentialDeterminationService = residentialDeterminationService;
        }
        /// <summary>
        /// Gets the specified surcharges to be applied to a shipment
        /// </summary>
        public IEnumerable<IUpsSurcharge> Get(IDictionary<UpsSurchargeType, double> surchargeLookup, IUpsLocalRatingZoneFileEntity zoneFileEntity)
        {
            return new IUpsSurcharge[]
            {
                new LargePackageUpsSurcharge(surchargeLookup),
                new ResidentialSurcharge(surchargeLookup),
                new DeliveryAreaSurcharge(surchargeLookup, zoneFileEntity, residentialDeterminationService), 

                new FuelGroundSurcharge(surchargeLookup),
                new SaturdayDeliverySurcharge(surchargeLookup),
                new FuelAirSurcharge(surchargeLookup),

                // Value adds
                new ReturnsSurcharge(surchargeLookup),
                new AdditionalHandlingSurcharge(surchargeLookup),
                new CarbonNeutralSurcharge(surchargeLookup),
                new CODSurcharge(surchargeLookup),
                new DryIceSurcharge(surchargeLookup),
                new ShipperReleaseSurcharge(surchargeLookup),
                new SignatureSurcharge(surchargeLookup),
                new VerbalConfirmationSurcharge(surchargeLookup), 
                
                // Third party billing is based on the shipments total price
                new ThirdPartyBillingSurcharge(surchargeLookup)
            };
        }
    }
}