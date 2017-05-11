using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
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
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsSurchargeFactory(IResidentialDeterminationService residentialDeterminationService, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.residentialDeterminationService = residentialDeterminationService;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the specified surcharges to be applied to a shipment
        /// </summary>
        public IEnumerable<IUpsSurcharge> Get(IDictionary<UpsSurchargeType, double> surchargeLookup, UpsLocalRatingZoneFileEntity zoneFileEntity)
        {
            return new IUpsSurcharge[]
            {
                new DeliveryAreaSurcharge(surchargeLookup, zoneFileEntity, residentialDeterminationService, sqlAdapterFactory),
                new LargePackageUpsSurcharge(surchargeLookup),
                new NdaEarlyOver150LbsSurcharge(surchargeLookup),

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