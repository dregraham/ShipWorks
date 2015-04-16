using System;
using System.Collections.Generic;
using System.Globalization;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface for adding customs information
    /// to a FedEx ProcessShipmentReply object.
    /// </summary>
    public class FedExCustomsManipulator : FedExShippingRequestManipulatorBase
    {
        private string shipmentCurrencyType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCustomsManipulator" /> class.
        /// </summary>
        public FedExCustomsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCustomsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExCustomsManipulator(FedExSettings fedExSettings) : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            ShipmentEntity shipment = request.ShipmentEntity;

            shipmentCurrencyType = GetShipmentCurrencyType(request.ShipmentEntity);

            if (!new FedExShipmentType().IsDomestic(request.ShipmentEntity))
            {
                // Obtain a handle to the customs detail
                CustomsClearanceDetail customsDetail = GetCustomsDetail(nativeRequest);

                customsDetail.CustomsValue = new Money
                {
                    Currency = shipmentCurrencyType,
                    Amount = shipment.CustomsValue
                };

                customsDetail.DocumentContent = shipment.FedEx.CustomsDocumentsOnly ? InternationalDocumentContentType.DOCUMENTS_ONLY : InternationalDocumentContentType.NON_DOCUMENTS;
                customsDetail.DocumentContentSpecified = true;
                
                ConfigureCommodities(shipment, customsDetail);
                ConfigureNaftaDetails(shipment, customsDetail);
                ConfigurePaymentDetail(shipment, customsDetail, request.CarrierAccountEntity as FedExAccountEntity);
                ConfigureExportDetail(shipment, customsDetail);
                ConfigureRecipientIdentification(shipment, customsDetail);
                ConfigureCustomsOptions(shipment, customsDetail);

                // Make sure the customs data is assigned back to the request (in the event that a new customs object was
                // created in the GetCustomsDetail method)
                nativeRequest.RequestedShipment.CustomsClearanceDetail = customsDetail;
                
                ConfigureTaxPayerIdentification(shipment, nativeRequest);
            }
        }

        /// <summary>
        /// Configures the recipient identification.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private void ConfigureRecipientIdentification(ShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            if (shipment.FedEx.CustomsRecipientIdentificationType != (int) FedExCustomsRecipientIdentificationType.None)
            {
                customsDetail.RecipientCustomsId = new RecipientCustomsId
                {
                    Type = GetApiRecipientIdentificationType(shipment.FedEx),
                    TypeSpecified = true,
                    Value = shipment.FedEx.CustomsRecipientIdentificationValue
                };
            }
        }

        /// <summary>
        /// Gets the type of the API recipient identification.
        /// </summary>
        /// <param name="fedExShipment">The fed ex shipment.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">An unrecognized type of customs identification was provided.</exception>
        private RecipientCustomsIdType GetApiRecipientIdentificationType(FedExShipmentEntity fedExShipment)
        {
            FedExCustomsRecipientIdentificationType type = (FedExCustomsRecipientIdentificationType) fedExShipment.CustomsRecipientIdentificationType;

            switch (type)
            {
                case FedExCustomsRecipientIdentificationType.Company: return RecipientCustomsIdType.COMPANY;
                case FedExCustomsRecipientIdentificationType.Individual: return RecipientCustomsIdType.INDIVIDUAL;
                case FedExCustomsRecipientIdentificationType.Passport: return RecipientCustomsIdType.PASSPORT;
            }

            throw new InvalidOperationException("An unrecognized type of customs identification was provided.");
        }

        /// <summary>
        /// Configures the export detail of the FedEx customs clearance detail object.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private void ConfigureExportDetail(ShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            FedExCustomsExportFilingOption filingOption = (FedExCustomsExportFilingOption) shipment.FedEx.CustomsExportFilingOption;

            ExportDetail exportDetail = new ExportDetail();
            exportDetail.ExportComplianceStatement = string.IsNullOrEmpty(shipment.FedEx.CustomsAESEEI) ? null : shipment.FedEx.CustomsAESEEI;

            customsDetail.ExportDetail = exportDetail;

            if ((!shipment.ReturnShipment && shipment.AdjustedOriginCountryCode() == "CA" && shipment.AdjustedShipCountryCode() != "US") ||
                (shipment.ReturnShipment && shipment.AdjustedShipCountryCode() == "CA" && shipment.AdjustedOriginCountryCode() != "US"))
            {
                exportDetail.B13AFilingOption = GetApiFilingOption(filingOption);
                exportDetail.B13AFilingOptionSpecified = true;
            }
        }

        private void ConfigureCustomsOptions(ShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            FedExCustomsOptionType optionType = (FedExCustomsOptionType) shipment.FedEx.CustomsOptionsType;

            if (optionType != FedExCustomsOptionType.None)
            {
                customsDetail.CustomsOptions = new CustomsOptionDetail
                {
                    Description = shipment.FedEx.CustomsOptionsDesription,
                    Type = GetApiCustomsOptionType(optionType),
                    TypeSpecified = true
                };
            }
        }

        /// <summary>
        /// Gets the value of the FedEx API customs option type.
        /// </summary>
        /// <param name="optionType">Type of the option.</param>
        /// <returns>The FedEx CustomsOptionType value.</returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized customs option type.</exception>
        private CustomsOptionType GetApiCustomsOptionType(FedExCustomsOptionType optionType)
        {
            switch (optionType)
            {
                case FedExCustomsOptionType.CourtesyReturnLabel: return CustomsOptionType.COURTESY_RETURN_LABEL;
                case FedExCustomsOptionType.ExhibitionTradeShow: return CustomsOptionType.EXHIBITION_TRADE_SHOW;
                case FedExCustomsOptionType.FaultyItem: return CustomsOptionType.FAULTY_ITEM;
                case FedExCustomsOptionType.FollowingRepair: return CustomsOptionType.FOLLOWING_REPAIR;
                case FedExCustomsOptionType.ForRepair: return CustomsOptionType.FOR_REPAIR;
                case FedExCustomsOptionType.ItemForLoan: return CustomsOptionType.ITEM_FOR_LOAN;
                case FedExCustomsOptionType.Other: return CustomsOptionType.OTHER;
                case FedExCustomsOptionType.Rejected: return CustomsOptionType.REJECTED;
                case FedExCustomsOptionType.Replacement: return CustomsOptionType.REPLACEMENT;
                case FedExCustomsOptionType.Trial: return CustomsOptionType.TRIAL;
            }

            throw new InvalidOperationException("Unrecognized customs option type.");
        }

        /// <summary>
        /// Gets the FedEx API B13 filing option for the ShipWorks filing option value.
        /// </summary>
        /// <param name="filingOption">The filing option.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized filing option value</exception>
        private B13AFilingOptionType GetApiFilingOption(FedExCustomsExportFilingOption filingOption)
        {
            switch (filingOption)
            {
                case FedExCustomsExportFilingOption.NotRequired: return B13AFilingOptionType.NOT_REQUIRED;
                case FedExCustomsExportFilingOption.ManuallyAttached: return B13AFilingOptionType.MANUALLY_ATTACHED;
                case FedExCustomsExportFilingOption.FiledElectonically: return B13AFilingOptionType.FILED_ELECTRONICALLY;
                case FedExCustomsExportFilingOption.SummaryReporting: return B13AFilingOptionType.SUMMARY_REPORTING;
            }

            throw new InvalidOperationException("Unrecognized filing option value");
        }

        /// <summary>
        /// Configures the tax payer identification.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="nativeRequest">The native request.</param>
        private void ConfigureTaxPayerIdentification(ShipmentEntity shipment, IFedExNativeShipmentRequest nativeRequest)
        {
            if (nativeRequest.RequestedShipment.Recipient.Tins == null || nativeRequest.RequestedShipment.Recipient.Tins.Length == 0)
            {
                nativeRequest.RequestedShipment.Recipient.Tins = new TaxpayerIdentification[1] { new TaxpayerIdentification() };
            }

            // TODO: We may need to set shipping/recipeint based on who's paying.  See ETD_Request.xml where The Tins info
            // is on Shipper.
            nativeRequest.RequestedShipment.Recipient.Tins[0] = new TaxpayerIdentification() {Number = shipment.FedEx.CustomsRecipientTIN, TinType = TinType.PERSONAL_STATE};

        }

        /// <summary>
        /// Configures the payment detail aspect of the CustomsClearanceDetail object from the shipment entity.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        /// <param name="fedExAccount">The FedEx account.</param>
        private void ConfigurePaymentDetail(ShipmentEntity shipment, CustomsClearanceDetail customsDetail, FedExAccountEntity fedExAccount)
        {
            Payment payment = new Payment();
            payment.PaymentType = GetApiPaymentType((FedExPayorType)shipment.FedEx.PayorDutiesType);

            if (payment.PaymentType != PaymentType.COLLECT)
            {
                payment.Payor = new Payor()
                {
                    ResponsibleParty = new Party() {Address = new Address(), Contact = new Contact()}
                };

                if (payment.PaymentType == PaymentType.SENDER)
                {
                    // Use the sender's account information to populate the payment
                    payment.Payor.ResponsibleParty.Contact.PersonName = fedExAccount.FirstName + " " + fedExAccount.LastName;

                    payment.Payor.ResponsibleParty.AccountNumber = fedExAccount.AccountNumber;
                    payment.Payor.ResponsibleParty.Address.CountryCode = fedExAccount.CountryCode;
                }
                else
                {
                    // Use the payment information from configured on shipment
                    payment.Payor.ResponsibleParty.Contact.PersonName = shipment.FedEx.PayorDutiesName;

                    payment.Payor.ResponsibleParty.AccountNumber = shipment.FedEx.PayorDutiesAccount;

                    if (!string.IsNullOrWhiteSpace(shipment.FedEx.PayorDutiesAccount))
                    {
                        payment.Payor.ResponsibleParty.Address.CountryCode = shipment.FedEx.PayorDutiesCountryCode;
                    }
                    else
                    {
                        payment.Payor.ResponsibleParty.Address.CountryCode = string.Empty;
                    }
                }
            }

            customsDetail.DutiesPayment = payment;
        }


        /// <summary>
        /// Get the FedEx API value for the given payor type
        /// </summary>
        private PaymentType GetApiPaymentType(FedExPayorType payorType)
        {
            switch (payorType)
            {
                case FedExPayorType.Sender: return PaymentType.SENDER;
                case FedExPayorType.Recipient: return PaymentType.RECIPIENT;
                case FedExPayorType.ThirdParty: return PaymentType.THIRD_PARTY;
                case FedExPayorType.Collect: return PaymentType.COLLECT;
            }

            throw new InvalidOperationException("Invalid FedEx payor type " + payorType);
        }

        /// <summary>
        /// Configures the commodities aspect of the CustomsClearanceDetail object from the shipment entity.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private void ConfigureCommodities(ShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            // According to 2012 test cases, all commodities have properties set in the same manner rather
            // than separate settings depending on documents only vs. non-documents as was the case in 
            // previous certifications
            List<Commodity> commodities = new List<Commodity>();
            foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
            {
                Commodity commodity = new Commodity
                {
                    Description = customsItem.Description,
                    Quantity = (decimal)customsItem.Quantity, //Math.Ceiling(customsItem.Quantity).ToString(CultureInfo.InvariantCulture),
                    QuantitySpecified = true,
                    QuantityUnits = "EA", 
                    NumberOfPieces = customsItem.NumberOfPieces.ToString(CultureInfo.InvariantCulture),
                    Weight = new Weight { Value = (decimal) customsItem.Weight, Units = GetApiWeightUnits(shipment) },
                    UnitPrice = new Money { Amount = customsItem.UnitPriceAmount, Currency = shipmentCurrencyType },
                    CountryOfManufacture = customsItem.CountryOfOrigin,
                    HarmonizedCode = customsItem.HarmonizedCode,
                    CustomsValue = new Money { Amount = customsItem.UnitValue, Currency = shipmentCurrencyType }
                };

                commodities.Add(commodity);
            }

            customsDetail.Commodities = commodities.ToArray();
        }

        /// <summary>
        /// Gets the FedEx API's customs item weight units value for the given customs item.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>
        /// The FedEx API WeightUnits value.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized weight unit.</exception>
        private WeightUnits GetApiWeightUnits(ShipmentEntity shipment)
        {
            WeightUnitOfMeasure type = (WeightUnitOfMeasure)shipment.FedEx.WeightUnitType;

            switch (type)
            {
                case WeightUnitOfMeasure.Pounds: return WeightUnits.LB;
                case WeightUnitOfMeasure.Kilograms: return WeightUnits.KG;
            }

            throw new InvalidOperationException("Unrecognized weight unit.");
        }

        /// <summary>
        /// Configures the NAFTA details of the customs detail commodities. This assumes the commodities have
        /// already been configured/created.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private void ConfigureNaftaDetails(ShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            if (shipment.FedEx.CustomsNaftaEnabled)
            {
                customsDetail.RegulatoryControls = new RegulatoryControlType[] {RegulatoryControlType.NAFTA};

                foreach (Commodity commodity in customsDetail.Commodities)
                {
                    commodity.NaftaDetail = new NaftaCommodityDetail()
                    {
                        NetCostMethod = GetApiNetCostMethodCode(shipment),
                        NetCostMethodSpecified = true,

                        PreferenceCriterion = GetApiPreferenceCode(shipment),
                        PreferenceCriterionSpecified = true,

                        ProducerDetermination = GetApiProducerCode(shipment),
                        ProducerDeterminationSpecified = true
                    };
                }
            }
        }

        /// <summary>
        /// Gets the API producer code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A NaftaProducerDeterminationCode value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown value for producer determination code</exception>
        private NaftaProducerDeterminationCode GetApiProducerCode(ShipmentEntity shipment)
        {
            FedExNaftaDeterminationCode determinationCode = (FedExNaftaDeterminationCode) shipment.FedEx.CustomsNaftaDeterminationCode;

            switch (determinationCode)
            {
                case FedExNaftaDeterminationCode.ProducerOfCommodity: return NaftaProducerDeterminationCode.YES;
                case FedExNaftaDeterminationCode.NotProducerKnowledgeOfCommodity: return NaftaProducerDeterminationCode.NO_1;
                case FedExNaftaDeterminationCode.NotProducerWrittenStatement: return NaftaProducerDeterminationCode.NO_2;
                case FedExNaftaDeterminationCode.NotProducerSignedCertificate: return NaftaProducerDeterminationCode.NO_3;
            }

            throw new InvalidOperationException("Unknown value for producer determination code");
        }


        /// <summary>
        /// Gets the API preference code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A NaftaPreferenceCriterionCode value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown value for preference criterion</exception>
        private NaftaPreferenceCriterionCode GetApiPreferenceCode(ShipmentEntity shipment)
        {
            FedExNaftaPreferenceCriteria preference = (FedExNaftaPreferenceCriteria)shipment.FedEx.CustomsNaftaPreferenceType;

            switch (preference)
            {
                case FedExNaftaPreferenceCriteria.A: return NaftaPreferenceCriterionCode.A;
                case FedExNaftaPreferenceCriteria.B: return NaftaPreferenceCriterionCode.B;
                case FedExNaftaPreferenceCriteria.C: return NaftaPreferenceCriterionCode.C;
                case FedExNaftaPreferenceCriteria.D: return NaftaPreferenceCriterionCode.D;
                case FedExNaftaPreferenceCriteria.E: return NaftaPreferenceCriterionCode.E;
                case FedExNaftaPreferenceCriteria.F: return NaftaPreferenceCriterionCode.F;
            }

            throw new InvalidOperationException("Unknown value for preference criterion");
        }

        /// <summary>
        /// Gets the API net cost method code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The NaftaNetCostMethodCode value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown value for net cost method</exception>
        private NaftaNetCostMethodCode GetApiNetCostMethodCode(ShipmentEntity shipment)
        {
            FedExNaftaNetCostMethod costMethod = (FedExNaftaNetCostMethod) shipment.FedEx.CustomsNaftaNetCostMethod;
            switch (costMethod)
            {
                case FedExNaftaNetCostMethod.NetCostMethod: return NaftaNetCostMethodCode.NC;
                case FedExNaftaNetCostMethod.NotCalculated: return NaftaNetCostMethodCode.NO;
            }

            throw new InvalidOperationException("Unknown value for net cost method");
        }


        /// <summary>
        /// Gets the customs detail from the native request if it is not null; if the CustomsClearanceDetail
        /// of the native request is null, a CustomsClearanceDetail object is returned.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <returns>A CustomsClearanceDetail object.</returns>
        private CustomsClearanceDetail GetCustomsDetail(IFedExNativeShipmentRequest nativeRequest)
        {
            return nativeRequest.RequestedShipment.CustomsClearanceDetail ?? new CustomsClearanceDetail();
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.Recipient == null)
            {
                // We'll be manipulating the TINs portion of the recipient, so make sure it's been created
                nativeRequest.RequestedShipment.Recipient = new Party();
            }
        }
    }
}
