using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using static ShipWorks.Shipping.ShipEngine.DTOs.MoneyDTO;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Factory for creating FedEx ShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.FedEx)]
    public class FedExShipmentRequestFactory : ShipEngineShipmentRequestFactory
    {
        private readonly IFedExAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IFedExShipmentTokenProcessor tokenProcessor;
        private readonly Dictionary<FedExCodPaymentType, PaymentTypeEnum> paymentTypeMap = new Dictionary<FedExCodPaymentType, PaymentTypeEnum>
        {
            { FedExCodPaymentType.Any, PaymentTypeEnum.Any },
            { FedExCodPaymentType.Secured, PaymentTypeEnum.Cash },
            { FedExCodPaymentType.Unsecured, PaymentTypeEnum.CashEquivalent },
        };

        private readonly Dictionary<CurrencyType, CurrencyEnum> currencyMap = new Dictionary<CurrencyType, CurrencyEnum>
        {
            { CurrencyType.USD, CurrencyEnum.USD },
            { CurrencyType.EUR, CurrencyEnum.EUR },
            { CurrencyType.UKL, CurrencyEnum.GBP },
            {CurrencyType.CAD, CurrencyEnum.CAD }
        };

        private readonly Dictionary<FedExSignatureType, Shipment.ConfirmationEnum> signatureMap = new Dictionary<FedExSignatureType, Shipment.ConfirmationEnum>
        {
            { FedExSignatureType.ServiceDefault, Shipment.ConfirmationEnum.None },
            { FedExSignatureType.NoSignature, Shipment.ConfirmationEnum.Delivery },
            { FedExSignatureType.Adult, Shipment.ConfirmationEnum.Adultsignature},
            { FedExSignatureType.Indirect, Shipment.ConfirmationEnum.Signature },
            { FedExSignatureType.Direct, Shipment.ConfirmationEnum.Directsignature }
        };

        public FedExShipmentRequestFactory(IFedExAccountRepository accountRepository,
           IShipEngineRequestFactory shipmentElementFactory,
           IShipmentTypeManager shipmentTypeManager,
           IFedExShipmentTokenProcessor tokenProcessor)
           : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Create a purchase label request
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public override PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            FedExUtility.ValidatePackageDimensions(shipment);
            var labelRequest = base.CreatePurchaseLabelRequest(shipment);
            FedExServiceType fedExServiceType = (FedExServiceType) shipment.FedEx.Service;
            if (labelRequest.Shipment.Packages.Any())
            {
                foreach (var package in labelRequest.Shipment.Packages)
                {
                    package.LabelMessages.Reference1 = TokenizeField(shipment.FedEx.ReferenceCustomer, "Reference", shipment, 35);
                    package.LabelMessages.Reference2 = TokenizeField(shipment.FedEx.ReferenceInvoice, "Invoice", shipment);
                    package.LabelMessages.Reference3 = TokenizeField(shipment.FedEx.ReferencePO, "P.O.", shipment);
                }
            }

            if (fedExServiceType == FedExServiceType.SmartPost)
            {
                labelRequest.Shipment.ServiceCode = EnumHelper.GetApiValue((FedExSmartPostIndicia) shipment.FedEx.SmartPostIndicia);
            }

            var confirmationType = (FedExSignatureType) shipment.FedEx.Signature;
            labelRequest.Shipment.Confirmation = signatureMap[confirmationType];

            if (shipment.ReturnShipment)
            {
                labelRequest.RmaNumber = shipment.FedEx.RmaNumber;
            }

            if (shipment.FedEx.FedExHoldAtLocationEnabled)
            {
                if (string.IsNullOrEmpty(shipment.FedEx.HoldLocationId))
                {
                    throw new FedExException("FedEx Hold At Location has been enabled for shipment, but no location was selected.");
                }

                labelRequest.ShipToServicePointId = shipment.FedEx.HoldLocationId;
            }

            if (FedExUtility.IsOneRateService(fedExServiceType))
            {
                foreach(var package in labelRequest.Shipment.Packages)
                {
                    if (package.PackageCode != null)
                    {
                        package.PackageCode += "_onerate";
                    }
                }
            }

            return labelRequest;
        }

        /// <summary>
        /// Create a return label request
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public override PurchaseLabelRequest CreateReturnLabelRequest(ShipmentEntity shipment)
        {
            var req = base.CreateReturnLabelRequest(shipment);
            req.RmaNumber = shipment.FedEx.RmaNumber;
            return req;
        }

        /// <summary>
        /// Gets the api value for the FedEx service
        /// </summary>
        protected override string GetServiceApiValue(ShipmentEntity shipment)
        {
            return EnumHelper.GetApiValue((FedExServiceType) shipment.FedEx.Service);
        }

        /// <summary>
        /// Creates the FedEx advanced options node
        /// </summary>
        protected override AdvancedOptions CreateAdvancedOptions(ShipmentEntity shipment)
        {
            var options = new AdvancedOptions()
            {
                ContainsAlcohol = shipment.FedEx.Packages.Any(p => p.ContainsAlcohol),
            };

            if (shipment.FedEx.Packages.Any(p => p.DryIceWeight > 0))
            {
                options.DryIceWeight = new Weight(shipment.FedEx.Packages.Sum(p => p.DryIceWeight), Weight.UnitEnum.Pound);
                options.DryIce = true;
            }

            var billToType = (FedExPayorType) shipment.FedEx.PayorTransportType;

            if (billToType != FedExPayorType.Sender)
            {
                options.BillToParty = billToType == FedExPayorType.ThirdParty ? AdvancedOptions.BillToPartyEnum.Thirdparty : AdvancedOptions.BillToPartyEnum.Recipient;
                options.BillToAccount = shipment.FedEx.PayorTransportAccount;
                options.BillToCountryCode = shipment.FedEx.PayorCountryCode;
                options.BillToPostalCode = shipment.FedEx.PayorPostalCode;
            }

            if (shipment.FedEx.CodEnabled && shipment.FedEx.CodAmount > 0)
            {
                var paymentType = (FedExCodPaymentType) shipment.FedEx.CodPaymentType;
                var currencyType = shipment.FedEx.Currency.HasValue ? (CurrencyType) shipment.FedEx.Currency : CurrencyType.USD;

                options.CollectOnDelivery = new CollectOnDeliveryAdvancedOption
                {
                    PaymentType = paymentTypeMap[paymentType],
                    PaymentAmount = new MoneyDTO
                    {
                        Amount = (double) shipment.FedEx.CodAmount,
                        Currency = currencyMap[currencyType]
                    }
                };
            }

            options.ThirdPartyConsignee = shipment.FedEx.ThirdPartyConsignee;
            options.NonMachinable = shipment.FedEx.NonStandardContainer;

            var smartpostEndorsement = (FedExSmartPostEndorsement) shipment.FedEx.SmartPostEndorsement;
            if (shipment.FedEx.Service == (int) FedExServiceType.SmartPost && smartpostEndorsement != FedExSmartPostEndorsement.None)
            {
                options.AncillaryEndorsement = EnumHelper.GetApiValue(smartpostEndorsement);
            }

            return options;
        }

        /// <summary>
        /// Insurance the FedEx packages when the user has picked FedEx declared value
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
        /// Ensure the shipment is not null
        /// </summary>
        /// <param name="shipment"></param>
        protected override void EnsureCarrierShipmentIsNotNull(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.FedEx, nameof(shipment.FedEx));
        }

        /// <summary>
        /// Get the ShipEngine Carrier ID
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        /// <exception cref="FedExException"></exception>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            IFedExAccountEntity account = accountRepository.GetAccountReadOnly(shipment);

            if (account == null)
            {
                throw new FedExException("Invalid account associated with shipment.");
            }

            return account.ShipEngineCarrierID;
        }

        /// <summary>
        /// Create the customs info for the shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override InternationalOptions CreateCustoms(ShipmentEntity shipment) =>
            new InternationalOptions()
            {
                Contents = shipment.FedEx.CustomsDocumentsOnly ? InternationalOptions.ContentsEnum.Documents : InternationalOptions.ContentsEnum.Merchandise,
                CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
                NonDelivery = InternationalOptions.NonDeliveryEnum.Returntosender
            };


        /// <summary>
        /// Create the tax identifiers for the shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override List<TaxIdentifier> CreateTaxIdentifiers(ShipmentEntity shipment)
        {
            List<TaxIdentifier> listTaxIds = new List<TaxIdentifier>();
            if (shipment.FedEx.CustomsRecipientTIN != "")
            {
                // We weren't sending IssuingAuthority with the direct integration to FedEx.
                // Not sending it here did result in a label, so should be good to not send it to ShipEngine
                TaxIdentifier taxIdentifier = new TaxIdentifier()
                {
                    // SE is mapping all TIN Types to NATIONAL_BUSINESS, so the only thing we can send is that the 
                    // IdentifierType is TIN
                    IdentifierType = TaxIdentifier.IdentifierTypeEnum.Tin,
                    TaxableEntityType = "shipper",
                    Value = shipment.FedEx.CustomsRecipientTIN
                };

                listTaxIds.Add(taxIdentifier);
            }
            return listTaxIds;
        }

        /// <summary>
        /// Convert tokens to their values
        /// </summary>
        private string TokenizeField(string field, string fieldName, IShipmentEntity shipment, int maxLength = 30)
        {
            string processedValue = tokenProcessor.ProcessTokens(field, shipment);

            if (string.IsNullOrEmpty(processedValue))
            {
                return null;
            }

            if (processedValue.Length > maxLength)
            {
                // FedEx sends back a confusing error message when this occurs, so be proactive and show the user a friendlier error message
                throw new FedExException($"FedEx does not allow {fieldName} # to exceed {maxLength} characters in length. The given value, \"{processedValue}\" exceeds the {maxLength} character limit. Please shorten the value and try again.");
            }

            return processedValue;
        }

        protected override string GetPackagingCode(IPackageAdapter package)
        {
            if (package == null)
            {
                return null;
            }
            FedExPackagingType fedExPackagingType = (FedExPackagingType) package.PackagingType;
            return EnumHelper.GetApiValue(fedExPackagingType);
        }
    }
}
