using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Ups.ShipEngine;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ShipEngine;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.Ups.OneBalance
{
    /// <summary>
    /// Factory for creating UPS ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.UpsOnLineTools)]
    public class UpsShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;
        private readonly ITemplateTokenProcessor templateTokenProcessor;
        private readonly IShipEngineRequestFactory shipmentElementFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipmentRequestFactory(IShipEngineRequestFactory shipmentElementFactory,
            IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            ITemplateTokenProcessor templateTokenProcessor)
            : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.shipmentElementFactory = shipmentElementFactory;
            this.accountRepository = accountRepository;
            this.templateTokenProcessor = templateTokenProcessor;
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
            IUpsAccountEntity account = accountRepository.GetAccountReadOnly(shipment);

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

            switch ((UpsDeliveryConfirmationType) shipment.Ups.DeliveryConfirmation)
            {
                case UpsDeliveryConfirmationType.NoSignature:
                    result.Shipment.Confirmation = Shipment.ConfirmationEnum.Delivery;
                    break;
                case UpsDeliveryConfirmationType.Signature:
                    result.Shipment.Confirmation = Shipment.ConfirmationEnum.Signature;
                    break;
                case UpsDeliveryConfirmationType.AdultSignature:
                    result.Shipment.Confirmation = Shipment.ConfirmationEnum.Adultsignature;
                    break;
                case UpsDeliveryConfirmationType.UspsDeliveryConfirmation:
                    result.Shipment.Confirmation = Shipment.ConfirmationEnum.Delivery;
                    break;
                case UpsDeliveryConfirmationType.None:
                default:
                    result.Shipment.Confirmation = Shipment.ConfirmationEnum.None;
                    break;
            }

            foreach (var package in result.Shipment.Packages)
            {
                package.LabelMessages.Reference1 = templateTokenProcessor.ProcessTokens(shipment.Ups.ReferenceNumber, shipment.ShipmentID);
                package.LabelMessages.Reference2 = templateTokenProcessor.ProcessTokens(shipment.Ups.ReferenceNumber2, shipment.ShipmentID);
            }

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

            switch ((UpsDeliveryConfirmationType) shipment.Ups.DeliveryConfirmation)
            {
                case UpsDeliveryConfirmationType.NoSignature:
                    result.Shipment.Confirmation = AddressValidatingShipment.ConfirmationEnum.Delivery;
                    break;
                case UpsDeliveryConfirmationType.Signature:
                    result.Shipment.Confirmation = AddressValidatingShipment.ConfirmationEnum.Signature;
                    break;
                case UpsDeliveryConfirmationType.AdultSignature:
                    result.Shipment.Confirmation = AddressValidatingShipment.ConfirmationEnum.Adultsignature;
                    break;
                case UpsDeliveryConfirmationType.UspsDeliveryConfirmation:
                    result.Shipment.Confirmation = AddressValidatingShipment.ConfirmationEnum.Delivery;
                    break;
                case UpsDeliveryConfirmationType.None:
                default:
                    result.Shipment.Confirmation = AddressValidatingShipment.ConfirmationEnum.None;
                    break;
            }

            if (shipment.Insurance && 
                shipment.InsuranceProvider == (int) InsuranceProvider.Carrier && 
                result?.Shipment != null)
            {
                result.Shipment.InsuranceProvider = AddressValidatingShipment.InsuranceProviderEnum.Carrier;
            }

            return result;
        }
        
        /// <summary>
        /// Gets the api value for the UPS service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment) =>
            UpsShipEngineServiceTypeUtility.GetServiceCode((UpsServiceType) shipment.Ups.Service, shipment.AdjustedShipCountryCode());

        /// <summary>
        /// Insurce the ups packages when the user has picked ups insurance
        /// </summary>
        protected override void SetPackageInsurance(ShipmentPackage shipmentPackage, IPackageAdapter packageAdapter)
        {
            if (packageAdapter.InsuranceChoice.Insured && 
                packageAdapter.InsuranceChoice.InsuranceProvider == InsuranceProvider.Carrier)
            {
                shipmentPackage.InsuredValue = new MoneyDTO(MoneyDTO.CurrencyEnum.USD, decimal.ToDouble(packageAdapter.InsuranceChoice.InsuranceValue));
            }
        }

        /// <summary>
        /// Creates the UPS advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            var options = new AdvancedOptions()
            {
                NonMachinable = shipment.Ups.Packages.Any(p => p.AdditionalHandlingEnabled),
                SaturdayDelivery = shipment.Ups.SaturdayDelivery
            };

            if (shipment.Ups.Packages.Any(p => p.DryIceEnabled))
            {
                options.DryIceWeight = new Weight(shipment.Ups.Packages.Sum(p => p.DryIceWeight), Weight.UnitEnum.Pound);
                options.DryIce = true;
            }

            return options;
        }
 
        /// <summary>
        /// Get the packaging code for the given adapter
        /// </summary>
        protected override string GetPackagingCode(IPackageAdapter package)
        {
            if (package == null)
            {
                return null;
            }

            string packagingCode = UpsShipEngineServiceTypeUtility.GetPackageCode((UpsPackagingType) package.PackagingType);

            return string.IsNullOrWhiteSpace(packagingCode) ? null : packagingCode;
        }
            

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

        /// <summary>
        /// Creates the UPS tax identifier node
        /// </summary>
        protected override List<TaxIdentifier> CreateTaxIdentifiers(ShipmentEntity shipment)
        {
            return null;
        }
    }
}
