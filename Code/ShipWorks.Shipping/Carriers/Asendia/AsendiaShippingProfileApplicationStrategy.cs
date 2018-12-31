using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Asendia shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.Asendia)]
    public class AsendiaShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity> accountRepository;

        /// <summary>
        /// constructor
        /// </summary>
        public AsendiaShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity> accountRepository)
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

            AsendiaShipmentEntity asendiaShipment = shipment.Asendia;
            IAsendiaProfileEntity asendiaProfile = profile.Asendia;

            long? accountID = (asendiaProfile.AsendiaAccountID == 0 && accountRepository.AccountsReadOnly.Any()) ?
                accountRepository.AccountsReadOnly.First().AsendiaAccountID :
                asendiaProfile.AsendiaAccountID;
            ApplyProfileValue(accountID, asendiaShipment, AsendiaShipmentFields.AsendiaAccountID);

            ApplyProfileValue(asendiaProfile.Service, asendiaShipment, AsendiaShipmentFields.Service);
            ApplyProfileValue(asendiaProfile.ShippingProfile.Insurance, asendiaShipment, AsendiaShipmentFields.Insurance);
            ApplyProfileValue(asendiaProfile.NonMachinable, asendiaShipment, AsendiaShipmentFields.NonMachinable);
            ApplyProfileValue(asendiaProfile.NonDelivery, asendiaShipment, AsendiaShipmentFields.NonDelivery);
            ApplyProfileValue(asendiaProfile.Contents, asendiaShipment, AsendiaShipmentFields.Contents);

            IPackageProfileEntity packageProfile = profile.Packages.Single();
            if (packageProfile.Weight.HasValue && !packageProfile.Weight.Value.IsEquivalentTo(0))
            {
                ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }
            ApplyProfileValue(packageProfile.DimsProfileID, asendiaShipment, AsendiaShipmentFields.DimsProfileID);
            ApplyProfileValue(packageProfile.DimsWeight, asendiaShipment, AsendiaShipmentFields.DimsWeight);

            if (packageProfile.DimsLength.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsLength, asendiaShipment, AsendiaShipmentFields.DimsLength);
            }

            if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsWidth, asendiaShipment, AsendiaShipmentFields.DimsWidth);
            }

            if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsHeight, asendiaShipment, AsendiaShipmentFields.DimsHeight);
            }

            ApplyProfileValue(packageProfile.DimsAddWeight, asendiaShipment, AsendiaShipmentFields.DimsAddWeight);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            shipmentType.UpdateTotalWeight(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);
        }
    }
}