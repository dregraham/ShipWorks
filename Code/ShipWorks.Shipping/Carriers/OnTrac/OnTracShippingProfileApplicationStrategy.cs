using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.OnTrac)]
    public class OnTracShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRetriever<OnTracAccountEntity, IOnTracAccountEntity> accountRepo;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Profile application strategy for OnTrac
        /// </summary>
        public OnTracShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
                                                        ICarrierAccountRetriever<OnTracAccountEntity, IOnTracAccountEntity> accountRepo) :
            base(shipmentTypeManager)
        {
            this.accountRepo = accountRepo;
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.OnTrac);
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            OnTracShipmentEntity onTracShipment = shipment.OnTrac;
            IOnTracProfileEntity onTracProfile = profile.OnTrac;
            IPackageProfileEntity packageProfile = profile.Packages.Single();

            long? accountID = onTracProfile.OnTracAccountID == 0 && accountRepo.AccountsReadOnly.Any() ?
                accountRepo.AccountsReadOnly.First().OnTracAccountID :
                onTracProfile.OnTracAccountID;

            ShippingProfileUtility.ApplyProfileValue(accountID, onTracShipment, OnTracShipmentFields.OnTracAccountID);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Service, onTracShipment, OnTracShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.PackagingType, onTracShipment, OnTracShipmentFields.PackagingType);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.ShippingProfile.Insurance, onTracShipment, OnTracShipmentFields.Insurance);

            ShippingProfileUtility.ApplyProfileValue(onTracProfile.SaturdayDelivery, onTracShipment, OnTracShipmentFields.SaturdayDelivery);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.SignatureRequired, onTracShipment, OnTracShipmentFields.SignatureRequired);

            if (packageProfile.Weight.HasValue && packageProfile.Weight.Value != 0)
            {
                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, onTracShipment, OnTracShipmentFields.DimsProfileID);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, onTracShipment, OnTracShipmentFields.DimsWeight);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, onTracShipment, OnTracShipmentFields.DimsLength);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, onTracShipment, OnTracShipmentFields.DimsHeight);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, onTracShipment, OnTracShipmentFields.DimsWidth);
            ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, onTracShipment, OnTracShipmentFields.DimsAddWeight);

            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Reference1, onTracShipment, OnTracShipmentFields.Reference1);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Reference2, onTracShipment, OnTracShipmentFields.Reference2);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Instructions, onTracShipment, OnTracShipmentFields.Instructions);

            ShippingProfileUtility.ApplyProfileValue(onTracProfile.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

            shipmentType.UpdateTotalWeight(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);
        }
    }
}