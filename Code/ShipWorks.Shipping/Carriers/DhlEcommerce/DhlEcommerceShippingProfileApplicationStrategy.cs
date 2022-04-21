using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
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
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRepository<DhlEcommerceAccountEntity, IDhlEcommerceAccountEntity> accountRepository,
            ISqlAdapterFactory sqlAdapterFactory) :
            base(shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(IShippingProfileEntity profile, ShipmentEntity shipment)
        {
            DhlEcommerceShipmentEntity dhlShipment = shipment.DhlEcommerce;
            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            
            base.ApplyProfile(profile, shipment);

            ApplyDhlEcommerceProfile(dhlShipment, profile);

            shipmentType.UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Apply the DHL eCommerce profile
        /// </summary>
        private void ApplyDhlEcommerceProfile(DhlEcommerceShipmentEntity dhlShipment, IShippingProfileEntity profile)
        {
            IDhlEcommerceProfileEntity source = profile.DhlEcommerce;

            long? accountID = (source.DhlEcommerceAccountID == 0 && accountRepository.Accounts.Any())
                ? accountRepository.Accounts.First().DhlEcommerceAccountID
                : source.DhlEcommerceAccountID;

            ApplyProfileValue(accountID, dhlShipment, DhlEcommerceShipmentFields.DhlEcommerceAccountID);
            ApplyProfileValue(source.Service, dhlShipment, DhlEcommerceShipmentFields.Service);
            ApplyProfileValue(source.DeliveryDutyPaid, dhlShipment, DhlEcommerceShipmentFields.DeliveredDutyPaid);
            ApplyProfileValue(source.NonMachinable, dhlShipment, DhlEcommerceShipmentFields.NonMachinable);
            ApplyProfileValue(source.SaturdayDelivery, dhlShipment, DhlEcommerceShipmentFields.SaturdayDelivery);
            ApplyProfileValue(source.NonDelivery, dhlShipment, DhlEcommerceShipmentFields.NonDelivery);
            ApplyProfileValue(source.Contents, dhlShipment, DhlEcommerceShipmentFields.Contents);
            ApplyProfileValue(source.CustomsRecipientTin, dhlShipment, DhlEcommerceShipmentFields.CustomsRecipientTin);
            ApplyProfileValue(source.CustomsTaxIdType, dhlShipment, DhlEcommerceShipmentFields.CustomsTaxIdType);
            ApplyProfileValue(source.CustomsTinIssuingAuthority, dhlShipment, DhlEcommerceShipmentFields.CustomsTinIssuingAuthority);
        }
    }
}