using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// AmazonSWA shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWAShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRepository<AmazonSWAAccountEntity, IAmazonSWAAccountEntity> accountRepository;

        /// <summary>
        /// constructor
        /// </summary>
        public AmazonSWAShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRepository<AmazonSWAAccountEntity, IAmazonSWAAccountEntity> accountRepository)
            : base(shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            AmazonSWAShipmentEntity AmazonSWAShipment = shipment.AmazonSWA;
            IAmazonSWAProfileEntity AmazonSWAProfile = profile.AmazonSWA;

            long? accountID = (AmazonSWAProfile.AmazonSWAAccountID == 0 && accountRepository.AccountsReadOnly.Any()) ?
                accountRepository.AccountsReadOnly.First().AmazonSWAAccountID :
                AmazonSWAProfile.AmazonSWAAccountID;
            ApplyProfileValue(accountID, AmazonSWAShipment, AmazonSWAShipmentFields.AmazonSWAAccountID);

            ApplyProfileValue(AmazonSWAProfile.Service, AmazonSWAShipment, AmazonSWAShipmentFields.Service);
            ApplyProfileValue(AmazonSWAProfile.ShippingProfile.Insurance, AmazonSWAShipment, AmazonSWAShipmentFields.Insurance);

            IPackageProfileEntity packageProfile = profile.Packages.Single();
            if (packageProfile.Weight.HasValue && !packageProfile.Weight.Value.IsEquivalentTo(0))
            {
                ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }
            ApplyProfileValue(packageProfile.DimsProfileID, AmazonSWAShipment, AmazonSWAShipmentFields.DimsProfileID);
            ApplyProfileValue(packageProfile.DimsWeight, AmazonSWAShipment, AmazonSWAShipmentFields.DimsWeight);

            if (packageProfile.DimsLength.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsLength, AmazonSWAShipment, AmazonSWAShipmentFields.DimsLength);
            }

            if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsWidth, AmazonSWAShipment, AmazonSWAShipmentFields.DimsWidth);
            }

            if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsHeight, AmazonSWAShipment, AmazonSWAShipmentFields.DimsHeight);
            }

            ApplyProfileValue(packageProfile.DimsAddWeight, AmazonSWAShipment, AmazonSWAShipmentFields.DimsAddWeight);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            shipmentType.UpdateTotalWeight(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);
        }
    }
}