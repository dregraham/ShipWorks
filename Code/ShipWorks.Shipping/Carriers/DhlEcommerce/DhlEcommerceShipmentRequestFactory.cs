using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Factory for creating DHL eCommerce ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.DhlEcommerce)]
    public class DhlEcommerceShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly IDhlEcommerceAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipmentRequestFactory(IDhlEcommerceAccountRepository accountRepository,
            IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager)
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Ensures the DHL Ecommerce shipment is not null
        /// </summary>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlEcommerce, nameof(shipment.DhlEcommerce));
        }

        /// <summary>
        /// Gets the ShipEngine carrier ID from the DHL Ecommerce shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            DhlEcommerceAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new DhlEcommerceException("Invalid account associated with shipment.");
            }

            return account.ShipEngineCarrierId;
        }

        /// <summary>
        /// Gets the api value for the DHL Ecommerce service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment) =>
            EnumHelper.GetApiValue((DhlEcommerceServiceType) shipment.DhlEcommerce.Service);

        /// <summary>
        /// Creates the DHL Ecommerce advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            return new AdvancedOptions(
                deliveredDutyPaid: shipment.DhlEcommerce.DeliveredDutyPaid,
                nonMachinable: shipment.DhlEcommerce.NonMachinable,
                saturdayDelivery: shipment.DhlEcommerce.SaturdayDelivery);
        }

        /// <summary>
        /// Creates the DHL Ecommerce customs node
        /// </summary>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            InternationalOptions customs = new InternationalOptions()
            {
                Contents = (InternationalOptions.ContentsEnum) shipment.DhlEcommerce.Contents,
                CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
                NonDelivery = (InternationalOptions.NonDeliveryEnum) shipment.DhlEcommerce.NonDelivery
            };

            return customs;
        }

        /// <summary>
        /// Creates the TaxIdentifiers node
        /// </summary>
        protected override List<TaxIdentifier> CreateTaxIdentifiers(ShipmentEntity shipment)
        {
            List<TaxIdentifier> TaxIdentifiers = new List<TaxIdentifier>();
            if (!string.IsNullOrWhiteSpace(shipment.DhlEcommerce.CustomsRecipientTin) && shipment.DhlEcommerce.CustomsRecipientTin != string.Empty)
            {
                TaxIdentifiers.Add(
                    new TaxIdentifier()
                    {
                        IdentifierType = (TaxIdentifier.IdentifierTypeEnum) shipment.DhlEcommerce.CustomsTaxIdType,
                        IssuingAuthority = shipment.DhlEcommerce.CustomsTinIssuingAuthority,
                        TaxableEntityType = "shipper",
                        Value = shipment.DhlEcommerce.CustomsRecipientTin,
                    });
            };

            return TaxIdentifiers;
        }
    }
}
