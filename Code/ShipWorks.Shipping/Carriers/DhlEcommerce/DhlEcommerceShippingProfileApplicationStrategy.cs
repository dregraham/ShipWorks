using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// DhlEcommerce shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.DhlEcommerce)]
    public class DhlEcommerceShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        private readonly ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity> accountRepository) :
            base(shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            base.ApplyProfile(profile, shipment);

            var dhlShipment = shipment.DhlEcommerce;

            IDhlEcommerceProfileEntity source = profile.DhlEcommerce;

            long? accountID = (source.DhlEcommerceAccountID == 0 && accountRepository.Accounts.Any())
                ? accountRepository.Accounts.First().DhlEcommerceAccountID
                : source.DhlEcommerceAccountID;

            // From
            ApplyProfileValue(accountID, dhlShipment, DhlEcommerceShipmentFields.DhlEcommerceAccountID);

            // Shipment
            ApplyProfileValue(source.Service, dhlShipment, DhlEcommerceShipmentFields.Service);
            ApplyProfileValue(source.PackagingType, dhlShipment, DhlEcommerceShipmentFields.PackagingType);

            var packageProfile = profile.Packages.Single();

            if (packageProfile.Weight.HasValue && !packageProfile.Weight.Value.IsEquivalentTo(0))
            {
                ApplyProfileValue(packageProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ApplyProfileValue(packageProfile.DimsProfileID, dhlShipment, DhlEcommerceShipmentFields.DimsProfileID);
            ApplyProfileValue(packageProfile.DimsWeight, dhlShipment, DhlEcommerceShipmentFields.DimsWeight);

            if (packageProfile.DimsLength.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsLength, dhlShipment, DhlEcommerceShipmentFields.DimsLength);
            }

            if (packageProfile.DimsWidth.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsWidth, dhlShipment, DhlEcommerceShipmentFields.DimsWidth);
            }

            if (packageProfile.DimsHeight.GetValueOrDefault() > 0)
            {
                ApplyProfileValue(packageProfile.DimsHeight, dhlShipment, DhlEcommerceShipmentFields.DimsHeight);
            }

            ApplyProfileValue(packageProfile.DimsAddWeight, dhlShipment, DhlEcommerceShipmentFields.DimsAddWeight);

            // Options
            ApplyProfileValue(source.SaturdayDelivery, dhlShipment, DhlEcommerceShipmentFields.SaturdayDelivery);
            ApplyProfileValue(source.DeliveryDutyPaid, dhlShipment, DhlEcommerceShipmentFields.DeliveredDutyPaid);
            ApplyProfileValue(source.NonMachinable, dhlShipment, DhlEcommerceShipmentFields.NonMachinable);
            ApplyProfileValue(source.ResidentialDelivery, dhlShipment, DhlEcommerceShipmentFields.ResidentialDelivery);
            ApplyProfileValue(source.Reference1, dhlShipment, DhlEcommerceShipmentFields.Reference1);

            // Customs
            ApplyProfileValue(source.Contents, dhlShipment, DhlEcommerceShipmentFields.Contents);
            ApplyProfileValue(source.NonDelivery, dhlShipment, DhlEcommerceShipmentFields.NonDelivery);
            ApplyProfileValue(source.CustomsTaxIdType, dhlShipment, DhlEcommerceShipmentFields.CustomsTaxIdType);
            ApplyProfileValue(source.CustomsRecipientTin, dhlShipment, DhlEcommerceShipmentFields.CustomsRecipientTin);
            ApplyProfileValue(source.CustomsTinIssuingAuthority, dhlShipment, DhlEcommerceShipmentFields.CustomsTinIssuingAuthority);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            shipmentType.UpdateTotalWeight(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);
        }
    }
}