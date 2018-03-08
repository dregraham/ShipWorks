using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    public class AsendiaShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity> accountRepository;

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
            ShippingProfileUtility.ApplyProfileValue(accountID, asendiaShipment, AsendiaShipmentFields.AsendiaAccountID);
            
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.Service, asendiaShipment, AsendiaShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.ShippingProfile.Insurance, asendiaShipment, AsendiaShipmentFields.Insurance);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.NonMachinable, asendiaShipment, AsendiaShipmentFields.NonMachinable);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.NonDelivery, asendiaShipment, AsendiaShipmentFields.NonDelivery);
            ShippingProfileUtility.ApplyProfileValue(asendiaProfile.Contents, asendiaShipment, AsendiaShipmentFields.Contents);

            IPackageProfileEntity packageProfile = profile.Packages.Single();
            if (packageProfile.Weight.HasValue && !packageProfile.Weight.Value.IsEquivalentTo(0))
            {
                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, asendiaShipment, AsendiaShipmentFields.DimsProfileID);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, asendiaShipment, AsendiaShipmentFields.DimsWeight);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, asendiaShipment, AsendiaShipmentFields.DimsLength);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, asendiaShipment, AsendiaShipmentFields.DimsHeight);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, asendiaShipment, AsendiaShipmentFields.DimsWidth);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, asendiaShipment, AsendiaShipmentFields.DimsAddWeight);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            shipmentType.UpdateTotalWeight(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);
        }
    }
}