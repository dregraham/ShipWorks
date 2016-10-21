///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'FedExShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFedExShipmentEntity : IFedExShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFedExShipmentEntity(IFedExShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            FedExAccountID = source.FedExAccountID;
            MasterFormID = source.MasterFormID;
            Service = source.Service;
            Signature = source.Signature;
            PackagingType = source.PackagingType;
            NonStandardContainer = source.NonStandardContainer;
            ReferenceCustomer = source.ReferenceCustomer;
            ReferenceInvoice = source.ReferenceInvoice;
            ReferencePO = source.ReferencePO;
            ReferenceShipmentIntegrity = source.ReferenceShipmentIntegrity;
            PayorTransportType = source.PayorTransportType;
            PayorTransportName = source.PayorTransportName;
            PayorTransportAccount = source.PayorTransportAccount;
            PayorDutiesType = source.PayorDutiesType;
            PayorDutiesAccount = source.PayorDutiesAccount;
            PayorDutiesName = source.PayorDutiesName;
            PayorDutiesCountryCode = source.PayorDutiesCountryCode;
            SaturdayDelivery = source.SaturdayDelivery;
            HomeDeliveryType = source.HomeDeliveryType;
            HomeDeliveryInstructions = source.HomeDeliveryInstructions;
            HomeDeliveryDate = source.HomeDeliveryDate;
            HomeDeliveryPhone = source.HomeDeliveryPhone;
            FreightInsidePickup = source.FreightInsidePickup;
            FreightInsideDelivery = source.FreightInsideDelivery;
            FreightBookingNumber = source.FreightBookingNumber;
            FreightLoadAndCount = source.FreightLoadAndCount;
            EmailNotifyBroker = source.EmailNotifyBroker;
            EmailNotifySender = source.EmailNotifySender;
            EmailNotifyRecipient = source.EmailNotifyRecipient;
            EmailNotifyOther = source.EmailNotifyOther;
            EmailNotifyOtherAddress = source.EmailNotifyOtherAddress;
            EmailNotifyMessage = source.EmailNotifyMessage;
            CodEnabled = source.CodEnabled;
            CodAmount = source.CodAmount;
            CodPaymentType = source.CodPaymentType;
            CodAddFreight = source.CodAddFreight;
            CodOriginID = source.CodOriginID;
            CodFirstName = source.CodFirstName;
            CodLastName = source.CodLastName;
            CodCompany = source.CodCompany;
            CodStreet1 = source.CodStreet1;
            CodStreet2 = source.CodStreet2;
            CodStreet3 = source.CodStreet3;
            CodCity = source.CodCity;
            CodStateProvCode = source.CodStateProvCode;
            CodPostalCode = source.CodPostalCode;
            CodCountryCode = source.CodCountryCode;
            CodPhone = source.CodPhone;
            CodTrackingNumber = source.CodTrackingNumber;
            CodTrackingFormID = source.CodTrackingFormID;
            CodTIN = source.CodTIN;
            CodChargeBasis = source.CodChargeBasis;
            CodAccountNumber = source.CodAccountNumber;
            BrokerEnabled = source.BrokerEnabled;
            BrokerAccount = source.BrokerAccount;
            BrokerFirstName = source.BrokerFirstName;
            BrokerLastName = source.BrokerLastName;
            BrokerCompany = source.BrokerCompany;
            BrokerStreet1 = source.BrokerStreet1;
            BrokerStreet2 = source.BrokerStreet2;
            BrokerStreet3 = source.BrokerStreet3;
            BrokerCity = source.BrokerCity;
            BrokerStateProvCode = source.BrokerStateProvCode;
            BrokerPostalCode = source.BrokerPostalCode;
            BrokerCountryCode = source.BrokerCountryCode;
            BrokerPhone = source.BrokerPhone;
            BrokerPhoneExtension = source.BrokerPhoneExtension;
            BrokerEmail = source.BrokerEmail;
            CustomsAdmissibilityPackaging = source.CustomsAdmissibilityPackaging;
            CustomsRecipientTIN = source.CustomsRecipientTIN;
            CustomsDocumentsOnly = source.CustomsDocumentsOnly;
            CustomsDocumentsDescription = source.CustomsDocumentsDescription;
            CustomsExportFilingOption = source.CustomsExportFilingOption;
            CustomsAESEEI = source.CustomsAESEEI;
            CustomsRecipientIdentificationType = source.CustomsRecipientIdentificationType;
            CustomsRecipientIdentificationValue = source.CustomsRecipientIdentificationValue;
            CustomsOptionsType = source.CustomsOptionsType;
            CustomsOptionsDesription = source.CustomsOptionsDesription;
            CommercialInvoice = source.CommercialInvoice;
            CommercialInvoiceFileElectronically = source.CommercialInvoiceFileElectronically;
            CommercialInvoiceTermsOfSale = source.CommercialInvoiceTermsOfSale;
            CommercialInvoicePurpose = source.CommercialInvoicePurpose;
            CommercialInvoiceComments = source.CommercialInvoiceComments;
            CommercialInvoiceFreight = source.CommercialInvoiceFreight;
            CommercialInvoiceInsurance = source.CommercialInvoiceInsurance;
            CommercialInvoiceOther = source.CommercialInvoiceOther;
            CommercialInvoiceReference = source.CommercialInvoiceReference;
            ImporterOfRecord = source.ImporterOfRecord;
            ImporterAccount = source.ImporterAccount;
            ImporterTIN = source.ImporterTIN;
            ImporterFirstName = source.ImporterFirstName;
            ImporterLastName = source.ImporterLastName;
            ImporterCompany = source.ImporterCompany;
            ImporterStreet1 = source.ImporterStreet1;
            ImporterStreet2 = source.ImporterStreet2;
            ImporterStreet3 = source.ImporterStreet3;
            ImporterCity = source.ImporterCity;
            ImporterStateProvCode = source.ImporterStateProvCode;
            ImporterPostalCode = source.ImporterPostalCode;
            ImporterCountryCode = source.ImporterCountryCode;
            ImporterPhone = source.ImporterPhone;
            SmartPostIndicia = source.SmartPostIndicia;
            SmartPostEndorsement = source.SmartPostEndorsement;
            SmartPostConfirmation = source.SmartPostConfirmation;
            SmartPostCustomerManifest = source.SmartPostCustomerManifest;
            SmartPostHubID = source.SmartPostHubID;
            SmartPostUspsApplicationId = source.SmartPostUspsApplicationId;
            DropoffType = source.DropoffType;
            OriginResidentialDetermination = source.OriginResidentialDetermination;
            FedExHoldAtLocationEnabled = source.FedExHoldAtLocationEnabled;
            HoldLocationId = source.HoldLocationId;
            HoldLocationType = source.HoldLocationType;
            HoldContactId = source.HoldContactId;
            HoldPersonName = source.HoldPersonName;
            HoldTitle = source.HoldTitle;
            HoldCompanyName = source.HoldCompanyName;
            HoldPhoneNumber = source.HoldPhoneNumber;
            HoldPhoneExtension = source.HoldPhoneExtension;
            HoldPagerNumber = source.HoldPagerNumber;
            HoldFaxNumber = source.HoldFaxNumber;
            HoldEmailAddress = source.HoldEmailAddress;
            HoldStreet1 = source.HoldStreet1;
            HoldStreet2 = source.HoldStreet2;
            HoldStreet3 = source.HoldStreet3;
            HoldCity = source.HoldCity;
            HoldStateOrProvinceCode = source.HoldStateOrProvinceCode;
            HoldPostalCode = source.HoldPostalCode;
            HoldUrbanizationCode = source.HoldUrbanizationCode;
            HoldCountryCode = source.HoldCountryCode;
            HoldResidential = source.HoldResidential;
            CustomsNaftaEnabled = source.CustomsNaftaEnabled;
            CustomsNaftaPreferenceType = source.CustomsNaftaPreferenceType;
            CustomsNaftaDeterminationCode = source.CustomsNaftaDeterminationCode;
            CustomsNaftaProducerId = source.CustomsNaftaProducerId;
            CustomsNaftaNetCostMethod = source.CustomsNaftaNetCostMethod;
            ReturnType = source.ReturnType;
            RmaNumber = source.RmaNumber;
            RmaReason = source.RmaReason;
            ReturnSaturdayPickup = source.ReturnSaturdayPickup;
            TrafficInArmsLicenseNumber = source.TrafficInArmsLicenseNumber;
            IntlExportDetailType = source.IntlExportDetailType;
            IntlExportDetailForeignTradeZoneCode = source.IntlExportDetailForeignTradeZoneCode;
            IntlExportDetailEntryNumber = source.IntlExportDetailEntryNumber;
            IntlExportDetailLicenseOrPermitNumber = source.IntlExportDetailLicenseOrPermitNumber;
            IntlExportDetailLicenseOrPermitExpirationDate = source.IntlExportDetailLicenseOrPermitExpirationDate;
            WeightUnitType = source.WeightUnitType;
            LinearUnitType = source.LinearUnitType;
            RequestedLabelFormat = source.RequestedLabelFormat;
            FimsAirWaybill = source.FimsAirWaybill;
            ReturnsClearance = source.ReturnsClearance;
            MaskedData = source.MaskedData;
            ReferenceFIMS = source.ReferenceFIMS;
            ThirdPartyConsignee = source.ThirdPartyConsignee;
            
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IFedExPackageEntity>();

            CopyCustomFedExShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The FedExAccountID property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."FedExAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FedExAccountID { get; }
        /// <summary> The MasterFormID property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."MasterFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 4<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MasterFormID { get; }
        /// <summary> The Service property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The Signature property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."Signature"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Signature { get; }
        /// <summary> The PackagingType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PackagingType { get; }
        /// <summary> The NonStandardContainer property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."NonStandardContainer"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NonStandardContainer { get; }
        /// <summary> The ReferenceCustomer property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReferenceCustomer"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceCustomer { get; }
        /// <summary> The ReferenceInvoice property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReferenceInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceInvoice { get; }
        /// <summary> The ReferencePO property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReferencePO"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferencePO { get; }
        /// <summary> The ReferenceShipmentIntegrity property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReferenceShipmentIntegrity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceShipmentIntegrity { get; }
        /// <summary> The PayorTransportType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PayorTransportType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PayorTransportType { get; }
        /// <summary> The PayorTransportName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PayorTransportName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorTransportName { get; }
        /// <summary> The PayorTransportAccount property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PayorTransportAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorTransportAccount { get; }
        /// <summary> The PayorDutiesType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PayorDutiesType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PayorDutiesType { get; }
        /// <summary> The PayorDutiesAccount property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PayorDutiesAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorDutiesAccount { get; }
        /// <summary> The PayorDutiesName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PayorDutiesName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorDutiesName { get; }
        /// <summary> The PayorDutiesCountryCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."PayorDutiesCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorDutiesCountryCode { get; }
        /// <summary> The SaturdayDelivery property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SaturdayDelivery { get; }
        /// <summary> The HomeDeliveryType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HomeDeliveryType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 HomeDeliveryType { get; }
        /// <summary> The HomeDeliveryInstructions property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HomeDeliveryInstructions"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 74<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String HomeDeliveryInstructions { get; }
        /// <summary> The HomeDeliveryDate property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HomeDeliveryDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime HomeDeliveryDate { get; }
        /// <summary> The HomeDeliveryPhone property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HomeDeliveryPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 24<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String HomeDeliveryPhone { get; }
        /// <summary> The FreightInsidePickup property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."FreightInsidePickup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FreightInsidePickup { get; }
        /// <summary> The FreightInsideDelivery property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."FreightInsideDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FreightInsideDelivery { get; }
        /// <summary> The FreightBookingNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."FreightBookingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FreightBookingNumber { get; }
        /// <summary> The FreightLoadAndCount property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."FreightLoadAndCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FreightLoadAndCount { get; }
        /// <summary> The EmailNotifyBroker property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."EmailNotifyBroker"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifyBroker { get; }
        /// <summary> The EmailNotifySender property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."EmailNotifySender"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifySender { get; }
        /// <summary> The EmailNotifyRecipient property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."EmailNotifyRecipient"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifyRecipient { get; }
        /// <summary> The EmailNotifyOther property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."EmailNotifyOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifyOther { get; }
        /// <summary> The EmailNotifyOtherAddress property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."EmailNotifyOtherAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailNotifyOtherAddress { get; }
        /// <summary> The EmailNotifyMessage property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."EmailNotifyMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 120<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailNotifyMessage { get; }
        /// <summary> The CodEnabled property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CodEnabled { get; }
        /// <summary> The CodAmount property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CodAmount { get; }
        /// <summary> The CodPaymentType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodPaymentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CodPaymentType { get; }
        /// <summary> The CodAddFreight property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodAddFreight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CodAddFreight { get; }
        /// <summary> The CodOriginID property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodOriginID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 CodOriginID { get; }
        /// <summary> The CodFirstName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodFirstName { get; }
        /// <summary> The CodLastName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodLastName { get; }
        /// <summary> The CodCompany property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodCompany { get; }
        /// <summary> The CodStreet1 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodStreet1 { get; }
        /// <summary> The CodStreet2 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodStreet2 { get; }
        /// <summary> The CodStreet3 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodStreet3 { get; }
        /// <summary> The CodCity property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodCity { get; }
        /// <summary> The CodStateProvCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodStateProvCode { get; }
        /// <summary> The CodPostalCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodPostalCode { get; }
        /// <summary> The CodCountryCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodCountryCode { get; }
        /// <summary> The CodPhone property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodPhone { get; }
        /// <summary> The CodTrackingNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodTrackingNumber { get; }
        /// <summary> The CodTrackingFormID property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodTrackingFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 4<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodTrackingFormID { get; }
        /// <summary> The CodTIN property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodTIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 24<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodTIN { get; }
        /// <summary> The CodChargeBasis property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodChargeBasis"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CodChargeBasis { get; }
        /// <summary> The CodAccountNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CodAccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodAccountNumber { get; }
        /// <summary> The BrokerEnabled property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean BrokerEnabled { get; }
        /// <summary> The BrokerAccount property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerAccount { get; }
        /// <summary> The BrokerFirstName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerFirstName { get; }
        /// <summary> The BrokerLastName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerLastName { get; }
        /// <summary> The BrokerCompany property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerCompany { get; }
        /// <summary> The BrokerStreet1 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerStreet1 { get; }
        /// <summary> The BrokerStreet2 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerStreet2 { get; }
        /// <summary> The BrokerStreet3 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerStreet3 { get; }
        /// <summary> The BrokerCity property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerCity { get; }
        /// <summary> The BrokerStateProvCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerStateProvCode { get; }
        /// <summary> The BrokerPostalCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerPostalCode { get; }
        /// <summary> The BrokerCountryCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerCountryCode { get; }
        /// <summary> The BrokerPhone property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerPhone { get; }
        /// <summary> The BrokerPhoneExtension property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerPhoneExtension"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerPhoneExtension { get; }
        /// <summary> The BrokerEmail property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."BrokerEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BrokerEmail { get; }
        /// <summary> The CustomsAdmissibilityPackaging property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsAdmissibilityPackaging"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsAdmissibilityPackaging { get; }
        /// <summary> The CustomsRecipientTIN property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsRecipientTIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 24<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsRecipientTIN { get; }
        /// <summary> The CustomsDocumentsOnly property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsDocumentsOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomsDocumentsOnly { get; }
        /// <summary> The CustomsDocumentsDescription property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsDocumentsDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsDocumentsDescription { get; }
        /// <summary> The CustomsExportFilingOption property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsExportFilingOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsExportFilingOption { get; }
        /// <summary> The CustomsAESEEI property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsAESEEI"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsAESEEI { get; }
        /// <summary> The CustomsRecipientIdentificationType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsRecipientIdentificationType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsRecipientIdentificationType { get; }
        /// <summary> The CustomsRecipientIdentificationValue property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsRecipientIdentificationValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsRecipientIdentificationValue { get; }
        /// <summary> The CustomsOptionsType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsOptionsType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsOptionsType { get; }
        /// <summary> The CustomsOptionsDesription property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsOptionsDesription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsOptionsDesription { get; }
        /// <summary> The CommercialInvoice property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CommercialInvoice { get; }
        /// <summary> The CommercialInvoiceFileElectronically property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoiceFileElectronically"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CommercialInvoiceFileElectronically { get; }
        /// <summary> The CommercialInvoiceTermsOfSale property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoiceTermsOfSale"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CommercialInvoiceTermsOfSale { get; }
        /// <summary> The CommercialInvoicePurpose property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoicePurpose"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CommercialInvoicePurpose { get; }
        /// <summary> The CommercialInvoiceComments property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoiceComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CommercialInvoiceComments { get; }
        /// <summary> The CommercialInvoiceFreight property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoiceFreight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CommercialInvoiceFreight { get; }
        /// <summary> The CommercialInvoiceInsurance property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoiceInsurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CommercialInvoiceInsurance { get; }
        /// <summary> The CommercialInvoiceOther property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoiceOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CommercialInvoiceOther { get; }
        /// <summary> The CommercialInvoiceReference property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CommercialInvoiceReference"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CommercialInvoiceReference { get; }
        /// <summary> The ImporterOfRecord property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterOfRecord"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ImporterOfRecord { get; }
        /// <summary> The ImporterAccount property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterAccount { get; }
        /// <summary> The ImporterTIN property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterTIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 24<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterTIN { get; }
        /// <summary> The ImporterFirstName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterFirstName { get; }
        /// <summary> The ImporterLastName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterLastName { get; }
        /// <summary> The ImporterCompany property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterCompany { get; }
        /// <summary> The ImporterStreet1 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterStreet1 { get; }
        /// <summary> The ImporterStreet2 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterStreet2 { get; }
        /// <summary> The ImporterStreet3 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterStreet3 { get; }
        /// <summary> The ImporterCity property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterCity { get; }
        /// <summary> The ImporterStateProvCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterStateProvCode { get; }
        /// <summary> The ImporterPostalCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterPostalCode { get; }
        /// <summary> The ImporterCountryCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterCountryCode { get; }
        /// <summary> The ImporterPhone property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ImporterPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImporterPhone { get; }
        /// <summary> The SmartPostIndicia property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."SmartPostIndicia"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SmartPostIndicia { get; }
        /// <summary> The SmartPostEndorsement property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."SmartPostEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SmartPostEndorsement { get; }
        /// <summary> The SmartPostConfirmation property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."SmartPostConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SmartPostConfirmation { get; }
        /// <summary> The SmartPostCustomerManifest property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."SmartPostCustomerManifest"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SmartPostCustomerManifest { get; }
        /// <summary> The SmartPostHubID property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."SmartPostHubID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SmartPostHubID { get; }
        /// <summary> The SmartPostUspsApplicationId property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."SmartPostUspsApplicationId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SmartPostUspsApplicationId { get; }
        /// <summary> The DropoffType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."DropoffType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DropoffType { get; }
        /// <summary> The OriginResidentialDetermination property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."OriginResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OriginResidentialDetermination { get; }
        /// <summary> The FedExHoldAtLocationEnabled property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."FedExHoldAtLocationEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FedExHoldAtLocationEnabled { get; }
        /// <summary> The HoldLocationId property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldLocationId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldLocationId { get; }
        /// <summary> The HoldLocationType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldLocationType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> HoldLocationType { get; }
        /// <summary> The HoldContactId property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldContactId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldContactId { get; }
        /// <summary> The HoldPersonName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldPersonName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldPersonName { get; }
        /// <summary> The HoldTitle property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldTitle"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldTitle { get; }
        /// <summary> The HoldCompanyName property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldCompanyName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldCompanyName { get; }
        /// <summary> The HoldPhoneNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldPhoneNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldPhoneNumber { get; }
        /// <summary> The HoldPhoneExtension property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldPhoneExtension"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldPhoneExtension { get; }
        /// <summary> The HoldPagerNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldPagerNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldPagerNumber { get; }
        /// <summary> The HoldFaxNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldFaxNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldFaxNumber { get; }
        /// <summary> The HoldEmailAddress property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldEmailAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldEmailAddress { get; }
        /// <summary> The HoldStreet1 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldStreet1 { get; }
        /// <summary> The HoldStreet2 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldStreet2 { get; }
        /// <summary> The HoldStreet3 property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldStreet3 { get; }
        /// <summary> The HoldCity property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldCity { get; }
        /// <summary> The HoldStateOrProvinceCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldStateOrProvinceCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldStateOrProvinceCode { get; }
        /// <summary> The HoldPostalCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldPostalCode { get; }
        /// <summary> The HoldUrbanizationCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldUrbanizationCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldUrbanizationCode { get; }
        /// <summary> The HoldCountryCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HoldCountryCode { get; }
        /// <summary> The HoldResidential property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."HoldResidential"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> HoldResidential { get; }
        /// <summary> The CustomsNaftaEnabled property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsNaftaEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomsNaftaEnabled { get; }
        /// <summary> The CustomsNaftaPreferenceType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsNaftaPreferenceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsNaftaPreferenceType { get; }
        /// <summary> The CustomsNaftaDeterminationCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsNaftaDeterminationCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsNaftaDeterminationCode { get; }
        /// <summary> The CustomsNaftaProducerId property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsNaftaProducerId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsNaftaProducerId { get; }
        /// <summary> The CustomsNaftaNetCostMethod property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."CustomsNaftaNetCostMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsNaftaNetCostMethod { get; }
        /// <summary> The ReturnType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReturnType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ReturnType { get; }
        /// <summary> The RmaNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."RmaNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String RmaNumber { get; }
        /// <summary> The RmaReason property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."RmaReason"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String RmaReason { get; }
        /// <summary> The ReturnSaturdayPickup property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReturnSaturdayPickup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ReturnSaturdayPickup { get; }
        /// <summary> The TrafficInArmsLicenseNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."TrafficInArmsLicenseNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TrafficInArmsLicenseNumber { get; }
        /// <summary> The IntlExportDetailType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."IntlExportDetailType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IntlExportDetailType { get; }
        /// <summary> The IntlExportDetailForeignTradeZoneCode property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."IntlExportDetailForeignTradeZoneCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String IntlExportDetailForeignTradeZoneCode { get; }
        /// <summary> The IntlExportDetailEntryNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."IntlExportDetailEntryNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String IntlExportDetailEntryNumber { get; }
        /// <summary> The IntlExportDetailLicenseOrPermitNumber property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."IntlExportDetailLicenseOrPermitNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String IntlExportDetailLicenseOrPermitNumber { get; }
        /// <summary> The IntlExportDetailLicenseOrPermitExpirationDate property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."IntlExportDetailLicenseOrPermitExpirationDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> IntlExportDetailLicenseOrPermitExpirationDate { get; }
        /// <summary> The WeightUnitType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."WeightUnitType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 WeightUnitType { get; }
        /// <summary> The LinearUnitType property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."LinearUnitType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LinearUnitType { get; }
        /// <summary> The RequestedLabelFormat property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The FimsAirWaybill property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."FimsAirWaybill"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String FimsAirWaybill { get; }
        /// <summary> The ReturnsClearance property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReturnsClearance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ReturnsClearance { get; }
        /// <summary> The MaskedData property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."MaskedData"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> MaskedData { get; }
        /// <summary> The ReferenceFIMS property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ReferenceFIMS"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceFIMS { get; }
        /// <summary> The ThirdPartyConsignee property of the Entity FedExShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExShipment"."ThirdPartyConsignee"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ThirdPartyConsignee { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        public IEnumerable<IFedExPackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFedExShipmentData(IFedExShipmentEntity source);
    }
}
