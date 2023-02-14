using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
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
            { FedExSignatureType.NoSignature, Shipment.ConfirmationEnum.None },
            { FedExSignatureType.ServiceDefault, Shipment.ConfirmationEnum.Delivery },
            { FedExSignatureType.Adult, Shipment.ConfirmationEnum.Adultsignature},
            { FedExSignatureType.Indirect, Shipment.ConfirmationEnum.Signature },
            { FedExSignatureType.Direct, Shipment.ConfirmationEnum.Directsignature }
        };

        public FedExShipmentRequestFactory(IFedExAccountRepository accountRepository,
           IShipEngineRequestFactory shipmentElementFactory,
           IShipmentTypeManager shipmentTypeManager)
           : base(shipmentElementFactory, shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
        }

        /// <summary>
        /// Create a purchase label request
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public override PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            var labelRequest = base.CreatePurchaseLabelRequest(shipment);
            if (labelRequest.Shipment.Packages.Any())
            {
                foreach(var package in labelRequest.Shipment.Packages)
                {
                    package.LabelMessages.Reference1 = shipment.FedEx.ReferenceCustomer;
                    package.LabelMessages.Reference2 = shipment.FedEx.ReferenceInvoice;
                    package.LabelMessages.Reference3 = shipment.FedEx.ReferencePO;
                }
            }

            var confirmationType = (FedExSignatureType) shipment.FedEx.Signature;
            labelRequest.Shipment.Confirmation = signatureMap[confirmationType];

            //TODO: Hold At Location
            //TODO: Specify Doc Tabs for thermal labels

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
            //TODO: Return Saturday Pickup
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

            if(billToType != FedExPayorType.Sender)
            {
                options.BillToParty = billToType == FedExPayorType.ThirdParty ? AdvancedOptions.BillToPartyEnum.Thirdparty : AdvancedOptions.BillToPartyEnum.Recipient;
                options.BillToAccount = shipment.FedEx.PayorTransportAccount;
                options.BillToCountryCode = shipment.FedEx.PayorCountryCode;
                options.BillToPostalCode = shipment.FedEx.PayorPostalCode;
            }

            if(shipment.FedEx.CodEnabled && shipment.FedEx.CodAmount > 0)
            {
                var paymentType = (FedExCodPaymentType) shipment.FedEx.CodPaymentType;
                var currencyType = shipment.FedEx.Currency.HasValue ? (CurrencyType) shipment.FedEx.Currency : default;

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

            return options;
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
            //TODO: The below needs to be done but untill we have all the information will cause shipments to fail
            /*
            List<TaxIdentifier> listTaxIds = new List<TaxIdentifier>();
            if (shipment.FedEx.CustomsRecipientTIN != "")
            {
                TaxIdentifier taxIdentifier = new TaxIdentifier()
                {
                    //TODO: Need to figure out what the "FedEx Tin Type" Means relative to IOSS, VAT, etc.
                    IdentifierType = (TaxIdentifier.IdentifierTypeEnum) shipment.FedEx.CustomsRecipientTINType,
                    TaxableEntityType = "shipper",
                    //TODO: Might need to add this to UI not sure
                    IssuingAuthority = shipment.FedEx.CustomsRecipientIssuingAuthority,
                    Value = shipment.FedEx.CustomsRecipientTIN
                };

                listTaxIds.Add(taxIdentifier);
            }
            return listTaxIds;
            */

            //TODO: B13AFiling Option for international CA shipments

            return new List<TaxIdentifier>();
        }
    }
}
