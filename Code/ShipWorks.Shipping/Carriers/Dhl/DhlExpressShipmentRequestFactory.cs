using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Factory for creating DHL Express ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShipmentRequestFactory(IDhlExpressAccountRepository accountRepository,
            IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager)
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Ensures the DHL Express shipment is not null
        /// </summary>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlExpress, nameof(shipment.DhlExpress));
        }

        /// <summary>
        /// Gets the ShipEngine carrier ID from the DHL Express shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            DhlExpressAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new DhlExpressException("Invalid account associated with shipment.");
            }

            return account.ShipEngineCarrierId;
        }

        /// <summary>
        /// Gets the api value for the DHL Express service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment) =>
            EnumHelper.GetApiValue((DhlExpressServiceType) shipment.DhlExpress.Service);

        /// <summary>
        /// Creates the DHL Express advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            return new AdvancedOptions(
                deliveredDutyPaid: shipment.DhlExpress.DeliveredDutyPaid,
                nonMachinable: shipment.DhlExpress.NonMachinable,
                saturdayDelivery: shipment.DhlExpress.SaturdayDelivery);
        }

        /// <summary>
        /// Creates the DHL Express customs node
        /// </summary>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            InternationalOptions customs = new InternationalOptions()
            {
                Contents = (InternationalOptions.ContentsEnum) shipment.DhlExpress.Contents,
                CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
                NonDelivery = (InternationalOptions.NonDeliveryEnum) shipment.DhlExpress.NonDelivery
            };

            return customs;
        }

        /// <summary>
        /// Creates the TaxIdentifiers node
        /// </summary>
        protected override List<TaxIdentifier> CreateTaxIdentifiers(ShipmentEntity shipment)
        {
            List<TaxIdentifier> TaxIdentifiers = new List<TaxIdentifier>();
            if (!string.IsNullOrWhiteSpace(shipment.DhlExpress.CustomsRecipientTin) && shipment.DhlExpress.CustomsRecipientTin != string.Empty)
            {
                TaxIdentifiers.Add(
                    new TaxIdentifier()
                    {
                        IdentifierType = (TaxIdentifier.IdentifierTypeEnum) shipment.DhlExpress.CustomsTaxIdType,
                        IssuingAuthority = shipment.DhlExpress.CustomsTinIssuingAuthority,
                        TaxableEntityType = "shipper",
                        Value = shipment.DhlExpress.CustomsRecipientTin,
                    });
            };

            return TaxIdentifiers;
        }
    }
}
