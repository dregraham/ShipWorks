using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac shipping profile application strategy
    /// </summary>
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

            ApplyProfileValue(accountID, onTracShipment, OnTracShipmentFields.OnTracAccountID);
            ApplyProfileValue(onTracProfile.Service, onTracShipment, OnTracShipmentFields.Service);
            ApplyProfileValue(onTracProfile.PackagingType, onTracShipment, OnTracShipmentFields.PackagingType);
            ApplyProfileValue(onTracProfile.ShippingProfile.Insurance, onTracShipment, OnTracShipmentFields.Insurance);

            ApplyProfileValue(onTracProfile.SaturdayDelivery, onTracShipment, OnTracShipmentFields.SaturdayDelivery);
            ApplyProfileValue(onTracProfile.SignatureRequired, onTracShipment, OnTracShipmentFields.SignatureRequired);

            if (packageProfile.Weight.HasValue && packageProfile.Weight.Value != 0)
            {
                ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ApplyProfileValue(packageProfile.DimsProfileID, onTracShipment, OnTracShipmentFields.DimsProfileID);
            ApplyProfileValue(packageProfile.DimsWeight, onTracShipment, OnTracShipmentFields.DimsWeight);
            ApplyProfileValue(packageProfile.DimsLength, onTracShipment, OnTracShipmentFields.DimsLength);
            ApplyProfileValue(packageProfile.DimsHeight, onTracShipment, OnTracShipmentFields.DimsHeight);
            ApplyProfileValue(packageProfile.DimsWidth, onTracShipment, OnTracShipmentFields.DimsWidth);
            ApplyProfileValue(packageProfile.DimsAddWeight, onTracShipment, OnTracShipmentFields.DimsAddWeight);

            ApplyProfileValue(onTracProfile.Reference1, onTracShipment, OnTracShipmentFields.Reference1);
            ApplyProfileValue(onTracProfile.Reference2, onTracShipment, OnTracShipmentFields.Reference2);
            ApplyProfileValue(onTracProfile.Instructions, onTracShipment, OnTracShipmentFields.Instructions);

            ApplyProfileValue(onTracProfile.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

            shipmentType.UpdateTotalWeight(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);
        }
    }
}