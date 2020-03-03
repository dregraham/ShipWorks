using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.ShipEngine;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Ups.OneBalance
{
    /// <summary>
    /// Factory for creating UPS ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.UpsOnLineTools)]
    public class UpsShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly UpsAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipmentRequestFactory(IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager)
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.shipmentElementFactory = shipmentElementFactory;
            this.accountRepository = new UpsAccountRepository();
        }

        /// <summary>
        /// Ensures the UPS shipment is not null
        /// </summary>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Ups, nameof(shipment.Ups));
        }

        /// <summary>
        /// Gets the ShipEngine carrier ID from the UPS shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            UpsAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new UpsException("Invalid account associated with shipment.");
            }

            return account.ShipEngineCarrierId;
        }

        /// <summary>
        /// Create a PurchaseLabelRequest for UPS that includes UPS Insurance
        /// </summary>
        public override PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            PurchaseLabelRequest result = base.CreatePurchaseLabelRequest(shipment);

            if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                result.Shipment.InsuranceProvider = Shipment.InsuranceProviderEnum.Carrier;
            }

            return result;
        }

        /// <summary>
        /// Create a RateShipmentRequest for UPS that includes UPS Insurance
        /// </summary>
        public override RateShipmentRequest CreateRateShipmentRequest(ShipmentEntity shipment)
        {
            RateShipmentRequest result = base.CreateRateShipmentRequest(shipment);

            if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                result.Shipment.InsuranceProvider = AddressValidatingShipment.InsuranceProviderEnum.Carrier;
            }

            return result;
        }

        /// <summary>
        /// Gets the api value for the UPS service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment) =>
            UpsShipEngineServiceTypeUtility.GetServiceCode((UpsServiceType) shipment.Ups.Service);

        /// <summary>
        /// Insurce the ups packages when the user has picked ups insurance
        /// </summary>
        protected override void SetPackageInsurance(ShipmentPackage shipmentPackage, IPackageAdapter packageAdapter)
        {
            if (packageAdapter.InsuranceChoice.Insured && 
                packageAdapter.InsuranceChoice.InsuranceProvider == Insurance.InsuranceProvider.Carrier)
            {
                shipmentPackage.InsuredValue = new MoneyDTO(MoneyDTO.CurrencyEnum.USD, decimal.ToDouble(packageAdapter.InsuranceChoice.InsuranceValue));
            }
        }

        /// <summary>
        /// Creates the UPS advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            AdvancedOptions.BillToPartyEnum? billToParty = null;

            if ((UpsPayorType) shipment.Ups.PayorType == UpsPayorType.ThirdParty)
            {
                billToParty = AdvancedOptions.BillToPartyEnum.Thirdparty;
            }
            else if ((UpsPayorType) shipment.Ups.PayorType == UpsPayorType.Receiver)
            {
                billToParty = AdvancedOptions.BillToPartyEnum.Recipient;
            }

            return new AdvancedOptions(billToAccount: shipment.Ups.PayorAccount,
                billToCountryCode: shipment.Ups.PayorCountryCode,
                billToParty: billToParty,
                billToPostalCode: shipment.Ups.PayorPostalCode,
                saturdayDelivery: shipment.Ups.SaturdayDelivery,
                deliveredDutyPaid: (UpsTermsOfSale) shipment.Ups.CommercialInvoiceTermsOfSale == UpsTermsOfSale.DeliveryDutyPaid,
                nonMachinable: (UpsPostalSubclassificationType) shipment.Ups.Subclassification != UpsPostalSubclassificationType.Machineable);
        }

        /// <summary>
        /// Get the packaging code for the given adapter
        /// </summary>
        protected override string GetPackagingCode(IPackageAdapter package) =>
            UpsShipEngineServiceTypeUtility.GetPackageCode((UpsPackagingType) package.PackagingType);

        /// <summary>
        /// Creates the UPS customs node
        /// </summary>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            InternationalOptions.ContentsEnum contents;

            switch ((UpsExportReason) shipment.Ups.CommercialInvoicePurpose)
            {
                case UpsExportReason.Gift:
                    contents = InternationalOptions.ContentsEnum.Gift;
                    break;
                case UpsExportReason.Sample:
                    contents = InternationalOptions.ContentsEnum.Sample;
                    break;
                case UpsExportReason.Return:
                    contents = InternationalOptions.ContentsEnum.Returnedgoods;
                    break;
                case UpsExportReason.Sale:
                    contents = shipment.Ups.CustomsDocumentsOnly ? InternationalOptions.ContentsEnum.Documents : InternationalOptions.ContentsEnum.Merchandise;
                    break;
                case UpsExportReason.Repair:
                    throw new ShippingException("Unable to get rates using export reason \"Repair\". Please select a different export reason option.");
                case UpsExportReason.InterCompanyData:
                    throw new ShippingException("Unable to get rates using export reason \"Inter-Company Data\". Please select a different export reason option.");
                default:
                    throw new ShippingException("Unexpected export reason encountered.");
            }

            InternationalOptions customs = new InternationalOptions()
            {
                Contents = contents,
                CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
                NonDelivery = InternationalOptions.NonDeliveryEnum.Returntosender
            };

            return customs;
        }
    }
}