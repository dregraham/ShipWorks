using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
    public class FedExTelemetryMutatorTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
        private readonly ShipmentEntity shipment;
        private readonly DateTime testDateTime;

        public FedExTelemetryMutatorTest()
        {
            testDateTime = DateTime.Now;

            var package = new FedExPackageEntity
            {
                AlcoholRecipientType = (int) ShipWorks.Shipping.Carriers.FedEx.WebServices.OpenShip.AlcoholRecipientType.CONSUMER,
                BatteryMaterial = FedExBatteryMaterialType.LithiumIon,
                BatteryPacking = FedExBatteryPackingType.ContainsInEquipement,
                BatteryRegulatorySubtype = FedExBatteryRegulatorySubType.IATASectionII,
                ContainsAlcohol = true,
                ContainerType = "ContainerType",
                DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible,
                DangerousGoodsCargoAircraftOnly = true,
                DangerousGoodsEmergencyContactPhone = "DangerousGoodsEm",
                DangerousGoodsEnabled = true,
                DangerousGoodsOfferor = "DangerousGoodsOfferor",
                DangerousGoodsPackagingCount = 1,
                DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries,
                DeclaredValue = 2,
                DimsAddWeight = true,
                DimsHeight = 3,
                DimsLength = 4,
                DimsWeight = 5,
                DimsWidth = 6,
                DryIceWeight = 7,
                FreightPackaging = FedExFreightPhysicalPackagingType.Bag,
                FreightPieces = 8,
                HazardousMaterialClass = "Hazardou",
                HazardousMaterialNumber = "HazardousMateria",
                HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.I,
                HazardousMaterialProperName = "HazardousMaterialProperName",
                HazardousMaterialQuanityUnits = (int) FedExHazardousMaterialsQuantityUnits.Gram,
                HazardousMaterialQuantityValue = 9,
                HazardousMaterialTechnicalName = "HazardousMaterialTechnicalName",
                Insurance = true,
                InsurancePennyOne = true,
                InsuranceValue = 10,
                NumberOfContainers = 11,
                PackingDetailsCargoAircraftOnly = true,
                PackingDetailsPackingInstructions = "PackingDetailsPackingInstructions",
                PriorityAlert = true,
                PriorityAlertDetailContent = "PriorityAlertDetailContent",
                PriorityAlertEnhancementType = (int) FedExPriorityAlertEnhancementType.PriorityAlert,
                SignatoryContactName = "SignatoryContactName",
                SignatoryPlace = "SignatoryPlace",
                SignatoryTitle = "SignatoryTitle",
                SkidPieces = 12,
                TrackingNumber = "TrackingNumber",
                Weight = 13
            };

            shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    BrokerAccount = "BrokerAccoun",
                    BrokerCity = "BrokerCity",
                    BrokerCompany = "BrokerCompany",
                    BrokerCountryCode = "BrokerCountryCode",
                    BrokerEmail = "BrokerEmail",
                    BrokerEnabled = true,
                    BrokerFirstName = "BrokerFirstName",
                    BrokerLastName = "BrokerLastName",
                    BrokerPhone = "BrokerPhone",
                    BrokerPhoneExtension = "BrokerPh",
                    BrokerPostalCode = "BrokerPostalCode",
                    BrokerStateProvCode = "BrokerStateProvCode",
                    BrokerStreet1 = "BrokerStreet1",
                    BrokerStreet2 = "BrokerStreet2",
                    BrokerStreet3 = "BrokerStreet3",
                    CodAccountNumber = "CodAccountNumber",
                    CodAddFreight = true,
                    CodAmount = 1,
                    CodChargeBasis = (int) FedExCodAddTransportationChargeBasisType.NetCharge,
                    CodCity = "CodCity",
                    CodCompany = "CodCompany",
                    CodCountryCode = "CodCountryCode",
                    CodEnabled = true,
                    CodFirstName = "CodFirstName",
                    CodLastName = "CodLastName",
                    CodOriginID = 2,
                    CodPaymentType = (int) FedExCodPaymentType.Any,
                    CodPhone = "CodPhone",
                    CodPostalCode = "CodPostalCode",
                    CodStateProvCode = "CodStateProvCode",
                    CodStreet1 = "CodStreet1",
                    CodStreet2 = "CodStreet2",
                    CodStreet3 = "CodStreet3",
                    CodTIN = "CodTIN",
                    CodTrackingFormID = "CodT",
                    CodTrackingNumber = "CodTrackingNumber",
                    CommercialInvoice = true,
                    CommercialInvoiceComments = "CommercialInvoiceComments",
                    CommercialInvoiceFileElectronically = true,
                    CommercialInvoiceFreight = 3,
                    CommercialInvoiceInsurance = 4,
                    CommercialInvoiceOther = 5,
                    CommercialInvoicePurpose = (int) FedExCommercialInvoicePurpose.Gift,
                    CommercialInvoiceReference = "CommercialInvoiceReference",
                    CommercialInvoiceTermsOfSale = (int) FedExTermsOfSale.DAP,
                    Currency = (int) CurrencyType.USD,
                    CustomsAdmissibilityPackaging = (int) FedExPhysicalPackagingType.Bag,
                    CustomsAESEEI = "CustomsAESEEI",
                    CustomsDocumentsDescription = "CustomsDocumentsDescription",
                    CustomsDocumentsOnly = true,
                    CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.FiledElectonically,
                    CustomsNaftaDeterminationCode = (int) FedExNaftaDeterminationCode.ProducerOfCommodity,
                    CustomsNaftaEnabled = true,
                    CustomsNaftaNetCostMethod = (int) FedExNaftaNetCostMethod.NetCostMethod,
                    CustomsNaftaPreferenceType = (int) FedExNaftaPreferenceCriteria.A,
                    CustomsNaftaProducerId = "CustomsNaftaProducer",
                    CustomsOptionsDesription = "CustomsOptionsDesription",
                    CustomsOptionsType = (int) FedExCustomsOptionType.Other,
                    CustomsRecipientIdentificationType = (int) FedExCustomsRecipientIdentificationType.Company,
                    CustomsRecipientIdentificationValue = "CustomsRecipientIdentificationValue",
                    CustomsRecipientTIN = "CustomsRecipientTIN",
                    DropoffType = (int) FedExDropoffType.DropBox,
                    EmailNotifyBroker = 1,
                    EmailNotifyMessage = "EmailNotifyMessage",
                    EmailNotifyOther = 1,
                    EmailNotifyOtherAddress = "EmailNotifyOtherAddress",
                    EmailNotifyRecipient = 1,
                    EmailNotifySender = 1,
                    FedExAccountID = 6,
                    FedExHoldAtLocationEnabled = true,
                    FimsAirWaybill = "FimsAirWaybill",
                    FreightBookingNumber = "FreightBooki",
                    FreightClass = FedEx.FedExFreightClassType.CLASS_050,
                    FreightCollectTerms = FedEx.FedExFreightCollectTermsType.Standard,
                    FreightGuaranteeDate = testDateTime,
                    FreightGuaranteeType = FedEx.FedExFreightGuaranteeType.Date,
                    FreightInsideDelivery = true,
                    FreightInsidePickup = true,
                    FreightLoadAndCount = 7,
                    FreightRole = FedEx.FedExFreightShipmentRoleType.Consignee,
                    FreightSpecialServices = (int) FedExFreightSpecialServicesType.Food,
                    FreightTotalHandlinUnits = 8,
                    HoldCity = "HoldCity",
                    HoldCompanyName = "HoldCompanyName",
                    HoldContactId = "HoldContactId",
                    HoldCountryCode = "HoldCountryCode",
                    HoldEmailAddress = "HoldEmailAddress",
                    HoldFaxNumber = "HoldFaxNumber",
                    HoldLocationId = "HoldLocationId",
                    HoldLocationType = (int) FedExLocationType.FedExOffice,
                    HoldPagerNumber = "HoldPagerNumber",
                    HoldPersonName = "HoldPersonName",
                    HoldPhoneExtension = "HoldPhoneE",
                    HoldPhoneNumber = "HoldPhoneNumber",
                    HoldPostalCode = "HoldPostalCode",
                    HoldResidential = true,
                    HoldStateOrProvinceCode = "HoldStateOrProvinceCode",
                    HoldStreet1 = "HoldStreet1",
                    HoldStreet2 = "HoldStreet2",
                    HoldStreet3 = "HoldStreet3",
                    HoldTitle = "HoldTitle",
                    HoldUrbanizationCode = "HoldUrbanizationCode",
                    HomeDeliveryDate = testDateTime,
                    HomeDeliveryInstructions = "HomeDeliveryInstructions",
                    HomeDeliveryPhone = "HomeDeliveryPhone",
                    HomeDeliveryType = (int) FedExHomeDeliveryType.Appointment,
                    ImporterAccount = "ImporterAcco",
                    ImporterCity = "ImporterCity",
                    ImporterCompany = "ImporterCompany",
                    ImporterCountryCode = "ImporterCountryCode",
                    ImporterFirstName = "ImporterFirstName",
                    ImporterLastName = "ImporterLastName",
                    ImporterOfRecord = true,
                    ImporterPhone = "ImporterPhone",
                    ImporterPostalCode = "ImporterPo",
                    ImporterStateProvCode = "ImporterStateProvCode",
                    ImporterStreet1 = "ImporterStreet1",
                    ImporterStreet2 = "ImporterStreet2",
                    ImporterStreet3 = "ImporterStreet3",
                    ImporterTIN = "ImporterTIN",
                    InternationalTrafficInArmsService = true,
                    IntlExportDetailEntryNumber = "IntlExportDetailEntr",
                    IntlExportDetailForeignTradeZoneCode = "IntlExportDetailForeignTradeZoneCode",
                    IntlExportDetailLicenseOrPermitExpirationDate = testDateTime,
                    IntlExportDetailLicenseOrPermitNumber = "IntlExportDetailLicenseOrPermitNumber",
                    IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea036,
                    LinearUnitType = (int) FedExLinearUnitOfMeasure.IN,
                    MaskedData = (int) FedExMaskedDataType.SecondaryBarcode,
                    MasterFormID = "Mast",
                    NonStandardContainer = true,
                    OriginResidentialDetermination = (int) ResidentialDeterminationType.Commercial,
                    Packages = { package },
                    PackagingType = (int) FedExPackagingType.Box,
                    PayorDutiesAccount = "PayorDutiesA",
                    PayorDutiesCountryCode = "PayorDutiesCountryCode",
                    PayorDutiesName = "PayorDutiesName",
                    PayorDutiesType = (int) FedExPayorType.Collect,
                    PayorTransportAccount = "PayorTranspo",
                    PayorTransportName = "PayorTransportName",
                    PayorTransportType = (int) FedExPayorType.Sender,
                    ReferenceCustomer = "ReferenceCustomer",
                    ReferenceFIMS = "ReferenceFIMS",
                    ReferenceInvoice = "ReferenceInvoice",
                    ReferencePO = "ReferencePO",
                    ReferenceShipmentIntegrity = "ReferenceShipmentIntegrity",
                    RequestedLabelFormat = (int) ThermalLanguage.ZPL,
                    ReturnSaturdayPickup = true,
                    ReturnsClearance = true,
                    ReturnType = (int) FedExReturnType.EmailReturnLabel,
                    RmaNumber = "RmaNumber",
                    RmaReason = "RmaReason",
                    SaturdayDelivery = true,
                    Service = (int) FedExServiceType.FedExGround,
                    Signature = (int) FedExSignatureType.Adult,
                    SmartPostConfirmation = true,
                    SmartPostCustomerManifest = "SmartPostCustomerManifest",
                    SmartPostEndorsement = (int) FedExSmartPostEndorsement.ChangeService,
                    SmartPostHubID = "SmartPostH",
                    SmartPostIndicia = (int) FedExSmartPostIndicia.MediaMail,
                    SmartPostUspsApplicationId = "SmartPostU",
                    ThirdPartyConsignee = true,
                    TrafficInArmsLicenseNumber = "TrafficInArmsLicenseNumber",
                    WeightUnitType = (int) WeightUnitOfMeasure.Pounds
                }
            };

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
        }

        [Fact]
        public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
        {
            var testObject = new FedExTelemetryMutator();

            testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerAccount", "BrokerAccoun"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerCity", "BrokerCity"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerCompany", "BrokerCompany"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerCountryCode", "BrokerCountryCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerEmail", "BrokerEmail"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerFirstName", "BrokerFirstName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerLastName", "BrokerLastName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerPhone", "BrokerPhone"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerPhoneExtension", "BrokerPh"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerPostalCode", "BrokerPostalCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerStateProvCode", "BrokerStateProvCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerStreet1", "BrokerStreet1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerStreet2", "BrokerStreet2"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.BrokerStreet3", "BrokerStreet3"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodAccountNumber", "CodAccountNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodAddFreight", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodAmount", "1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodChargeBasis", "Net Charge"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodCity", "CodCity"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodCompany", "CodCompany"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodCountryCode", "CodCountryCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodFirstName", "CodFirstName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodLastName", "CodLastName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodOriginID", "2"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodPaymentType", "Any"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodPhone", "CodPhone"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodPostalCode", "CodPostalCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodStateProvCode", "CodStateProvCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodStreet1", "CodStreet1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodStreet2", "CodStreet2"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodStreet3", "CodStreet3"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodTIN", "CodTIN"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodTrackingFormID", "CodT"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CodTrackingNumber", "CodTrackingNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoice", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoiceComments", "CommercialInvoiceComments"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoiceFileElectronically", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoiceFreight", "3"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoiceInsurance", "4"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoiceOther", "5"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoicePurpose", "Gift"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoiceReference", "CommercialInvoiceReference"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CommercialInvoiceTermsOfSale", "Delivered at Place"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Currency", "USD"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsAdmissibilityPackaging", "Bag"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsAESEEI", "CustomsAESEEI"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsDocumentsDescription", "CustomsDocumentsDescription"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsDocumentsOnly", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsExportFilingOption", "Filed Electronically"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsNaftaDeterminationCode", "Producer of the commodity"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsNaftaEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsNaftaNetCostMethod", "Calculated according to the net cost method"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsNaftaPreferenceType", "A (wholly obtained or produced entirely in US/CA/MX)"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsNaftaProducerId", "CustomsNaftaProducer"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsOptionsDescription", "CustomsOptionsDesription"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsOptionsType", "Other"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsRecipientIdentificationType", "Company"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsRecipientIdentificationValue", "CustomsRecipientIdentificationValue"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.CustomsRecipientTIN", "CustomsRecipientTIN"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.DropoffType", "Drop Box"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.EmailNotifyBroker", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.EmailNotifyMessage", "EmailNotifyMessage"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.EmailNotifyOther", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.EmailNotifyOtherAddress", "EmailNotifyOtherAddress"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.EmailNotifyRecipient", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.EmailNotifySender", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FedExAccountID", "6"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FedExHoldAtLocationEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FimsAirWaybill", "FimsAirWaybill"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightBookingNumber", "FreightBooki"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightClass", "Class 050"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightCollectTerms", "Standard"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightGuaranteeDate", testDateTime.ToShortDateString()));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightGuaranteeType", "Date"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightInsideDelivery", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightInsidePickup", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightLoadAndCount", "7"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightRole", "Consignee"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightSpecialServices", "Food"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.FreightTotalHandlingUnits", "8"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldCity", "HoldCity"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldCompanyName", "HoldCompanyName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldContactId", "HoldContactId"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldCountryCode", "HoldCountryCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldEmailAddress", "HoldEmailAddress"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldFaxNumber", "HoldFaxNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldLocationId", "HoldLocationId"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldLocationType", "FEDEX_OFFICE"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldPagerNumber", "HoldPagerNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldPersonName", "HoldPersonName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldPhoneExtension", "HoldPhoneE"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldPhoneNumber", "HoldPhoneNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldPostalCode", "HoldPostalCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldResidential", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldStateOrProvinceCode", "HoldStateOrProvinceCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldStreet1", "HoldStreet1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldStreet2", "HoldStreet2"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldStreet3", "HoldStreet3"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldTitle", "HoldTitle"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HoldUrbanizationCode", "HoldUrbanizationCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HomeDeliveryDate", testDateTime.ToShortDateString()));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HomeDeliveryInstructions", "HomeDeliveryInstructions"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HomeDeliveryPhone", "HomeDeliveryPhone"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.HomeDeliveryType", "Appointment"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterAccount", "ImporterAcco"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterCity", "ImporterCity"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterCompany", "ImporterCompany"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterCountryCode", "ImporterCountryCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterFirstName", "ImporterFirstName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterLastName", "ImporterLastName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterOfRecord", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterPhone", "ImporterPhone"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterPostalCode", "ImporterPo"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterStateProvCode", "ImporterStateProvCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterStreet1", "ImporterStreet1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterStreet2", "ImporterStreet2"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterStreet3", "ImporterStreet3"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ImporterTIN", "ImporterTIN"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.InternationalTrafficInArmsService", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.IntlExportDetailEntryNumber", "IntlExportDetailEntr"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.IntlExpotDetailForeignTradeZoneCode", "IntlExportDetailForeignTradeZoneCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.IntlExportDetailLicenseOrPermitExpirationDate", testDateTime.ToShortDateString()));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.IntlExportDetailLicenseOrPermitNumber", "IntlExportDetailLicenseOrPermitNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.IntlExportDetailType", "DEA 036"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.LinearUnitType", "Inches"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.MaskedData", "Secondary Barcode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.MasterFormID", "Mast"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.NonStandardContainer", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.OriginResidentialDetermination", "Commercial"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PackagingType", "FedEx® Box"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PayorDutiesAccount", "PayorDutiesA"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PayorDutiesCountryCode", "PayorDutiesCountryCode"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PayorDutiesName", "PayorDutiesName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PayorDutiesType", "Collect"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PayorTransportAccount", "PayorTranspo"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PayorTransportName", "PayorTransportName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.PayorTransportType", "Sender"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReferenceCustomer", "ReferenceCustomer"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReferenceFIMS", "ReferenceFIMS"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReferenceInvoice", "ReferenceInvoice"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReferencePO", "ReferencePO"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReferenceShipmentIntegrity", "ReferenceShipmentIntegrity"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.RequestedLabelFormat", "Thermal - ZPL"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReturnSaturdayPickup", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReturnsClearance", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ReturnType", "Email Return Label"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.RmaNumber", "RmaNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.RmaReason", "RmaReason"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.SaturdayDelivery", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Signature", "Adult Signature Required"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.SmartPostConfirmation", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.SmartPostCustomerManifest", "SmartPostCustomerManifest"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.SmartPostEndorsement", "Change Service"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.SmartPostHubID", "SmartPostH"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.SmartPostIndicia", "FedEx SmartPost® Media"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.SmartPostUspsApplicationId", "SmartPostU"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.ThirdPartyConsignee", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.TrafficInArmsLicenseNumber", "TrafficInArmsLicenseNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.WeightUnitType", "Pounds"));

            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.AlcoholRecipientType", "CONSUMER"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.BatteryMaterial", "Lithium Ion"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.BatteryPacking", "Contained in equipment"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.BatteryRegulatorySubtype", "IATA section II"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.ContainerType", "ContainerType"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.ContainsAlcohol", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DangerousGoodsAccessibilityType", "Accessible"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DangerousGoodsCargoAircraftOnly", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DangerousGoodsEmergencyContactPhone", "DangerousGoodsEm"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DangerousGoodsEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DangerousGoodsOfferor", "DangerousGoodsOfferor"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DangerousGoodsPackagingCount", "1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DangerousGoodsType", "Batteries"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DeclaredValue", "2"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DimsAddWeight", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DimsHeight", "3"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DimsLength", "4"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DimsWeight", "5"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DimsWidth", "6"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.DryIceWeight", "7"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.FreightPackaging", "Bag"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.FreightPieces", "8"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.HazardousMaterialClass", "Hazardou"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.HazardousMaterialNumber", "HazardousMateria"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.HazardousMaterialPackingGroup", "I"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.HazardousMaterialProperName", "HazardousMaterialProperName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.HazardousMaterialQuantityUnits", "G"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.HazardousMaterialQuantityValue", "9"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.HazardousMaterialTechnicalName", "HazardousMaterialTechnicalName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.Insurance", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.InsurancePennyOne", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.InsuranceValue", "10"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.NumberOfContainers", "11"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.PackingDetailsCargoAircraftOnly", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.PackingDetailsPackingInstructions", "PackingDetailsPackingInstructions"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.PriorityAlert", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.PriorityAlertDetailContent", "PriorityAlertDetailContent"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.PriorityAlertEnhancementType", "FedEx Priority Alert®"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.SignatoryContactName", "SignatoryContactName"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.SignatoryPlace", "SignatoryPlace"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.SignatoryTitle", "SignatoryTitle"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.SkidPieces", "12"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.TrackingNumber", "TrackingNumber"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.FedEx.Package.1.Weight", "13"));
        }
    }
}
