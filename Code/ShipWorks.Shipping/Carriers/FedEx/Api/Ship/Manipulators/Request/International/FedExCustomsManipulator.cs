using System;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface for adding customs information
    /// to a FedEx ProcessShipmentReply object.
    /// </summary>
    public class FedExCustomsManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;
        private readonly ICustomsRequired customsRequired;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExCustomsManipulator(IFedExSettingsRepository settings, ICustomsRequired customsRequired)
        {
            this.settings = settings;
            this.customsRequired = customsRequired;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            customsRequired.IsCustomsRequired(shipment);

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            var account = settings.GetAccountReadOnly(shipment);
            var shipmentCurrencyType = GetShipmentCurrencyType(shipment);

            // Obtain a handle to the customs detail
            var customsDetail = request.RequestedShipment.CustomsClearanceDetail;

            customsDetail.CustomsValue = new Money
            {
                Currency = shipmentCurrencyType,
                Amount = shipment.CustomsValue
            };

            customsDetail.DocumentContent = shipment.FedEx.CustomsDocumentsOnly ? InternationalDocumentContentType.DOCUMENTS_ONLY : InternationalDocumentContentType.NON_DOCUMENTS;
            customsDetail.DocumentContentSpecified = true;

            return GenericResult.FromSuccess(customsDetail)
                .Do(detail => ConfigureCommodities(shipment, shipmentCurrencyType, detail))
                .Do(detail => ConfigureNaftaDetails(shipment, detail))
                .Do(detail => ConfigurePaymentDetail(shipment, detail, account))
                .Do(detail => ConfigureExportDetail(shipment, detail))
                .Do(detail => ConfigureRecipientIdentification(shipment, detail))
                .Do(detail => ConfigureCustomsOptions(shipment, detail))
                .Do(detail => request.RequestedShipment.CustomsClearanceDetail = detail)
                .Map(_ => ConfigureTaxPayerIdentification(shipment, request));
        }

        /// <summary>
        /// Configures the recipient identification.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private static Result ConfigureRecipientIdentification(IShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            if (shipment.FedEx.CustomsRecipientIdentificationType != (int) FedExCustomsRecipientIdentificationType.None)
            {
                return GetApiRecipientIdentificationType(shipment.FedEx)
                    .Map(type => CreateRecipientCustomsId(shipment, type))
                    .Do(id => customsDetail.RecipientCustomsId = id);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Create RecipientCustomsId element
        /// </summary>
        private static RecipientCustomsId CreateRecipientCustomsId(IShipmentEntity shipment, RecipientCustomsIdType recipientCustomsIdType) =>
            new RecipientCustomsId
            {
                Type = recipientCustomsIdType,
                TypeSpecified = true,
                Value = shipment.FedEx.CustomsRecipientIdentificationValue
            };

        /// <summary>
        /// Gets the type of the API recipient identification.
        /// </summary>
        /// <param name="fedExShipment">The fed ex shipment.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">An unrecognized type of customs identification was provided.</exception>
        private static GenericResult<RecipientCustomsIdType> GetApiRecipientIdentificationType(IFedExShipmentEntity fedExShipment)
        {
            FedExCustomsRecipientIdentificationType type = (FedExCustomsRecipientIdentificationType) fedExShipment.CustomsRecipientIdentificationType;

            switch (type)
            {
                case FedExCustomsRecipientIdentificationType.Company: return RecipientCustomsIdType.COMPANY;
                case FedExCustomsRecipientIdentificationType.Individual: return RecipientCustomsIdType.INDIVIDUAL;
                case FedExCustomsRecipientIdentificationType.Passport: return RecipientCustomsIdType.PASSPORT;
            }

            return new InvalidOperationException("An unrecognized type of customs identification was provided.");
        }

        /// <summary>
        /// Configures the export detail of the FedEx customs clearance detail object.
        /// </summary>
        private static Result ConfigureExportDetail(IShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            FedExCustomsExportFilingOption filingOption = (FedExCustomsExportFilingOption) shipment.FedEx.CustomsExportFilingOption;

            ExportDetail exportDetail = new ExportDetail();
            exportDetail.ExportComplianceStatement = string.IsNullOrEmpty(shipment.FedEx.CustomsAESEEI) ? null : shipment.FedEx.CustomsAESEEI;

            customsDetail.ExportDetail = exportDetail;

            if ((!shipment.ReturnShipment && shipment.AdjustedOriginCountryCode() == "CA" && shipment.ShipCountryCode != "US") ||
                (shipment.ReturnShipment && shipment.AdjustedShipCountryCode() == "CA" && shipment.OriginCountryCode != "US"))
            {
                return GetApiFilingOption(filingOption)
                    .Do(x =>
                    {
                        exportDetail.B13AFilingOption = x;
                        exportDetail.B13AFilingOptionSpecified = true;
                    });
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Configure the customs options
        /// </summary>
        private static Result ConfigureCustomsOptions(IShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            FedExCustomsOptionType optionType = (FedExCustomsOptionType) shipment.FedEx.CustomsOptionsType;

            if (optionType == FedExCustomsOptionType.None)
            {
                return Result.FromSuccess();
            }

            return GetApiCustomsOptionType(optionType)
                .Map(x => CreateCustomsOptionDetail(shipment.FedEx, x))
                .Do(x => customsDetail.CustomsOptions = x);
        }

        /// <summary>
        /// Create CustomsOptionsDetail
        /// </summary>
        private static CustomsOptionDetail CreateCustomsOptionDetail(IFedExShipmentEntity fedEx, CustomsOptionType optionType) =>
            new CustomsOptionDetail
            {
                Description = fedEx.CustomsOptionsDesription,
                Type = optionType,
                TypeSpecified = true
            };

        /// <summary>
        /// Gets the value of the FedEx API customs option type.
        /// </summary>
        /// <param name="optionType">Type of the option.</param>
        /// <returns>The FedEx CustomsOptionType value.</returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized customs option type.</exception>
        private static GenericResult<CustomsOptionType> GetApiCustomsOptionType(FedExCustomsOptionType optionType)
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

            return new InvalidOperationException("Unrecognized customs option type.");
        }

        /// <summary>
        /// Gets the FedEx API B13 filing option for the ShipWorks filing option value.
        /// </summary>
        /// <param name="filingOption">The filing option.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized filing option value</exception>
        private static GenericResult<B13AFilingOptionType> GetApiFilingOption(FedExCustomsExportFilingOption filingOption)
        {
            switch (filingOption)
            {
                case FedExCustomsExportFilingOption.NotRequired: return B13AFilingOptionType.NOT_REQUIRED;
                case FedExCustomsExportFilingOption.ManuallyAttached: return B13AFilingOptionType.MANUALLY_ATTACHED;
                case FedExCustomsExportFilingOption.FiledElectonically: return B13AFilingOptionType.FILED_ELECTRONICALLY;
                case FedExCustomsExportFilingOption.SummaryReporting: return B13AFilingOptionType.SUMMARY_REPORTING;
            }

            return new InvalidOperationException("Unrecognized filing option value");
        }

        /// <summary>
        /// Configures the tax payer identification.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="request">The native request.</param>
        private ProcessShipmentRequest ConfigureTaxPayerIdentification(IShipmentEntity shipment, ProcessShipmentRequest request)
        {
            if (request.RequestedShipment.Recipient.Tins == null || request.RequestedShipment.Recipient.Tins.Length == 0)
            {
                request.RequestedShipment.Recipient.Tins = new TaxpayerIdentification[1] { new TaxpayerIdentification() };
            }

            // TODO: We may need to set shipping/recipient based on who's paying.  See ETD_Request.xml where The Tins info
            // is on Shipper.
            request.RequestedShipment.Recipient.Tins[0] = new TaxpayerIdentification() { Number = shipment.FedEx.CustomsRecipientTIN, TinType = TinType.PERSONAL_STATE };

            return request;
        }

        /// <summary>
        /// Configures the payment detail aspect of the CustomsClearanceDetail object from the shipment entity.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        /// <param name="fedExAccount">The FedEx account.</param>
        private static Result ConfigurePaymentDetail(IShipmentEntity shipment, CustomsClearanceDetail customsDetail, IFedExAccountEntity fedExAccount) =>
            GetApiPaymentType((FedExPayorType) shipment.FedEx.PayorDutiesType)
                .Do(x => CreatePaymentDetail(shipment, customsDetail, fedExAccount, x));

        /// <summary>
        /// Create the payment detail element
        /// </summary>
        private static void CreatePaymentDetail(IShipmentEntity shipment, CustomsClearanceDetail customsDetail, IFedExAccountEntity fedExAccount, PaymentType paymentType)
        {
            Payment payment = new Payment();
            payment.PaymentType = paymentType;

            if (payment.PaymentType != PaymentType.COLLECT)
            {
                payment.Payor = new Payor()
                {
                    ResponsibleParty = new Party() { Address = new Address(), Contact = new Contact() }
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
        private static GenericResult<PaymentType> GetApiPaymentType(FedExPayorType payorType)
        {
            switch (payorType)
            {
                case FedExPayorType.Sender: return PaymentType.SENDER;
                case FedExPayorType.Recipient: return PaymentType.RECIPIENT;
                case FedExPayorType.ThirdParty: return PaymentType.THIRD_PARTY;
                case FedExPayorType.Collect: return PaymentType.COLLECT;
            }

            return new InvalidOperationException("Invalid FedEx payor type " + payorType);
        }

        /// <summary>
        /// Configures the commodities aspect of the CustomsClearanceDetail object from the shipment entity.
        /// </summary>
        /// <remarks>
        /// According to 2012 test cases, all commodities have properties set in the same manner rather
        /// than separate settings depending on documents only vs. non-documents as was the case in 
        /// previous certifications
        /// </remarks>
        private static Result ConfigureCommodities(IShipmentEntity shipment, string shipmentCurrencyType, CustomsClearanceDetail customsDetail) =>
            shipment.CustomsItems
                .Select(x => CreateCommodityElement(shipment, shipmentCurrencyType, x))
                .Flatten()
                .Do(x => customsDetail.Commodities = x.ToArray());

        /// <summary>
        /// Create a commodity element
        /// </summary>
        private static GenericResult<Commodity> CreateCommodityElement(IShipmentEntity shipment, string shipmentCurrencyType, IShipmentCustomsItemEntity customsItem) =>
            GetApiWeightUnits(shipment)
                .Map(x => new Commodity
                {
                    Description = customsItem.Description,
                    Quantity = (decimal) customsItem.Quantity, //Math.Ceiling(customsItem.Quantity).ToString(CultureInfo.InvariantCulture),
                    QuantitySpecified = true,
                    QuantityUnits = "EA",
                    NumberOfPieces = customsItem.NumberOfPieces.ToString(CultureInfo.InvariantCulture),
                    Weight = new Weight { Value = (decimal) customsItem.Weight, Units = x },
                    UnitPrice = new Money { Amount = customsItem.UnitPriceAmount, Currency = shipmentCurrencyType },
                    CountryOfManufacture = customsItem.CountryOfOrigin,
                    HarmonizedCode = customsItem.HarmonizedCode,
                    CustomsValue = new Money { Amount = customsItem.UnitValue, Currency = shipmentCurrencyType }
                });

        /// <summary>
        /// Gets the FedEx API's customs item weight units value for the given customs item.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>
        /// The FedEx API WeightUnits value.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized weight unit.</exception>
        private static GenericResult<WeightUnits> GetApiWeightUnits(IShipmentEntity shipment)
        {
            WeightUnitOfMeasure type = (WeightUnitOfMeasure) shipment.FedEx.WeightUnitType;

            switch (type)
            {
                case WeightUnitOfMeasure.Pounds: return WeightUnits.LB;
                case WeightUnitOfMeasure.Kilograms: return WeightUnits.KG;
            }

            return new InvalidOperationException("Unrecognized weight unit.");
        }

        /// <summary>
        /// Configures the NAFTA details of the customs detail commodities. This assumes the commodities have
        /// already been configured/created.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private static Result ConfigureNaftaDetails(IShipmentEntity shipment, CustomsClearanceDetail customsDetail)
        {
            if (shipment.FedEx.CustomsNaftaEnabled)
            {
                customsDetail.RegulatoryControls = new RegulatoryControlType[] { RegulatoryControlType.NAFTA };

                return customsDetail.Commodities
                    .Select(commodity => CreateNaftaCommodityDetail(shipment).Map(x => commodity.NaftaDetail = x))
                    .Flatten();
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Create the NAFTA commodity detail element
        /// </summary>
        private static GenericResult<NaftaCommodityDetail> CreateNaftaCommodityDetail(IShipmentEntity shipment) =>
            GenericResult.FromSuccess(new NaftaCommodityDetail())
                .Do(x => SetNetCostMethod(shipment, x))
                .Do(x => SetPreferenceCriterion(shipment, x))
                .Do(x => SetProducerDetermination(shipment, x));

        /// <summary>
        /// Set the net cost method
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="detail"></param>
        private static Result SetNetCostMethod(IShipmentEntity shipment, NaftaCommodityDetail detail) =>
            GetApiNetCostMethodCode(shipment)
                .Do(x =>
                {
                    detail.NetCostMethod = x;
                    detail.NetCostMethodSpecified = true;
                });

        /// <summary>
        /// Set the preference criterion
        /// </summary>
        private static Result SetPreferenceCriterion(IShipmentEntity shipment, NaftaCommodityDetail detail) =>
            GetApiPreferenceCode(shipment)
                .Do(x =>
                {
                    detail.PreferenceCriterion = x;
                    detail.PreferenceCriterionSpecified = true;
                });

        /// <summary>
        /// Set the producer determination
        /// </summary>
        private static Result SetProducerDetermination(IShipmentEntity shipment, NaftaCommodityDetail detail) =>
            GetApiProducerCode(shipment)
                .Do(x =>
                {
                    detail.ProducerDetermination = x;
                    detail.ProducerDeterminationSpecified = true;
                });

        /// <summary>
        /// Gets the API producer code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A NaftaProducerDeterminationCode value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown value for producer determination code</exception>
        private static GenericResult<NaftaProducerDeterminationCode> GetApiProducerCode(IShipmentEntity shipment)
        {
            FedExNaftaDeterminationCode determinationCode = (FedExNaftaDeterminationCode) shipment.FedEx.CustomsNaftaDeterminationCode;

            switch (determinationCode)
            {
                case FedExNaftaDeterminationCode.ProducerOfCommodity: return NaftaProducerDeterminationCode.YES;
                case FedExNaftaDeterminationCode.NotProducerKnowledgeOfCommodity: return NaftaProducerDeterminationCode.NO_1;
                case FedExNaftaDeterminationCode.NotProducerWrittenStatement: return NaftaProducerDeterminationCode.NO_2;
                case FedExNaftaDeterminationCode.NotProducerSignedCertificate: return NaftaProducerDeterminationCode.NO_3;
            }

            return new InvalidOperationException("Unknown value for producer determination code");
        }

        /// <summary>
        /// Gets the API preference code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A NaftaPreferenceCriterionCode value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown value for preference criterion</exception>
        private static GenericResult<NaftaPreferenceCriterionCode> GetApiPreferenceCode(IShipmentEntity shipment)
        {
            FedExNaftaPreferenceCriteria preference = (FedExNaftaPreferenceCriteria) shipment.FedEx.CustomsNaftaPreferenceType;

            switch (preference)
            {
                case FedExNaftaPreferenceCriteria.A: return NaftaPreferenceCriterionCode.A;
                case FedExNaftaPreferenceCriteria.B: return NaftaPreferenceCriterionCode.B;
                case FedExNaftaPreferenceCriteria.C: return NaftaPreferenceCriterionCode.C;
                case FedExNaftaPreferenceCriteria.D: return NaftaPreferenceCriterionCode.D;
                case FedExNaftaPreferenceCriteria.E: return NaftaPreferenceCriterionCode.E;
                case FedExNaftaPreferenceCriteria.F: return NaftaPreferenceCriterionCode.F;
            }

            return new InvalidOperationException("Unknown value for preference criterion");
        }

        /// <summary>
        /// Gets the API net cost method code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The NaftaNetCostMethodCode value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown value for net cost method</exception>
        private static GenericResult<NaftaNetCostMethodCode> GetApiNetCostMethodCode(IShipmentEntity shipment)
        {
            FedExNaftaNetCostMethod costMethod = (FedExNaftaNetCostMethod) shipment.FedEx.CustomsNaftaNetCostMethod;
            switch (costMethod)
            {
                case FedExNaftaNetCostMethod.NetCostMethod: return NaftaNetCostMethodCode.NC;
                case FedExNaftaNetCostMethod.NotCalculated: return NaftaNetCostMethodCode.NO;
            }

            return new InvalidOperationException("Unknown value for net cost method");
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(IShipmentEntity shipment, ProcessShipmentRequest request)
        {
            request.Ensure(r => r.RequestedShipment)
                .Ensure(rs => rs.Recipient);
            request.RequestedShipment.Ensure(x => x.CustomsClearanceDetail);
        }

        /// <summary>
        /// Helper method to get shipment currency
        /// </summary>
        protected string GetShipmentCurrencyType(IShipmentEntity shipment)
        {
            return FedExSettings.GetCurrencyTypeApiValue(shipment, () => settings.GetAccountReadOnly(shipment));
        }
    }
}
