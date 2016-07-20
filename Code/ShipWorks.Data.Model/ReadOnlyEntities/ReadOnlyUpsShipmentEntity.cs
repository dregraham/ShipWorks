///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'UpsShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsShipmentEntity : IUpsShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsShipmentEntity(IUpsShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            UpsAccountID = source.UpsAccountID;
            Service = source.Service;
            SaturdayDelivery = source.SaturdayDelivery;
            CodEnabled = source.CodEnabled;
            CodAmount = source.CodAmount;
            CodPaymentType = source.CodPaymentType;
            DeliveryConfirmation = source.DeliveryConfirmation;
            ReferenceNumber = source.ReferenceNumber;
            ReferenceNumber2 = source.ReferenceNumber2;
            PayorType = source.PayorType;
            PayorAccount = source.PayorAccount;
            PayorPostalCode = source.PayorPostalCode;
            PayorCountryCode = source.PayorCountryCode;
            EmailNotifySender = source.EmailNotifySender;
            EmailNotifyRecipient = source.EmailNotifyRecipient;
            EmailNotifyOther = source.EmailNotifyOther;
            EmailNotifyOtherAddress = source.EmailNotifyOtherAddress;
            EmailNotifyFrom = source.EmailNotifyFrom;
            EmailNotifySubject = source.EmailNotifySubject;
            EmailNotifyMessage = source.EmailNotifyMessage;
            CustomsDocumentsOnly = source.CustomsDocumentsOnly;
            CustomsDescription = source.CustomsDescription;
            CommercialPaperlessInvoice = source.CommercialPaperlessInvoice;
            CommercialInvoiceTermsOfSale = source.CommercialInvoiceTermsOfSale;
            CommercialInvoicePurpose = source.CommercialInvoicePurpose;
            CommercialInvoiceComments = source.CommercialInvoiceComments;
            CommercialInvoiceFreight = source.CommercialInvoiceFreight;
            CommercialInvoiceInsurance = source.CommercialInvoiceInsurance;
            CommercialInvoiceOther = source.CommercialInvoiceOther;
            WorldShipStatus = source.WorldShipStatus;
            PublishedCharges = source.PublishedCharges;
            NegotiatedRate = source.NegotiatedRate;
            ReturnService = source.ReturnService;
            ReturnUndeliverableEmail = source.ReturnUndeliverableEmail;
            ReturnContents = source.ReturnContents;
            UspsTrackingNumber = source.UspsTrackingNumber;
            Endorsement = source.Endorsement;
            Subclassification = source.Subclassification;
            PaperlessAdditionalDocumentation = source.PaperlessAdditionalDocumentation;
            ShipperRelease = source.ShipperRelease;
            CarbonNeutral = source.CarbonNeutral;
            CostCenter = source.CostCenter;
            IrregularIndicator = source.IrregularIndicator;
            Cn22Number = source.Cn22Number;
            ShipmentChargeType = source.ShipmentChargeType;
            ShipmentChargeAccount = source.ShipmentChargeAccount;
            ShipmentChargePostalCode = source.ShipmentChargePostalCode;
            ShipmentChargeCountryCode = source.ShipmentChargeCountryCode;
            UspsPackageID = source.UspsPackageID;
            RequestedLabelFormat = source.RequestedLabelFormat;
            
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsPackageEntity>();

            CopyCustomUpsShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The UpsAccountID property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."UpsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UpsAccountID { get; }
        /// <summary> The Service property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The SaturdayDelivery property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SaturdayDelivery { get; }
        /// <summary> The CodEnabled property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CodEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CodEnabled { get; }
        /// <summary> The CodAmount property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CodAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CodAmount { get; }
        /// <summary> The CodPaymentType property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CodPaymentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CodPaymentType { get; }
        /// <summary> The DeliveryConfirmation property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DeliveryConfirmation { get; }
        /// <summary> The ReferenceNumber property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReferenceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceNumber { get; }
        /// <summary> The ReferenceNumber2 property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReferenceNumber2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceNumber2 { get; }
        /// <summary> The PayorType property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PayorType { get; }
        /// <summary> The PayorAccount property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorAccount { get; }
        /// <summary> The PayorPostalCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorPostalCode { get; }
        /// <summary> The PayorCountryCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayorCountryCode { get; }
        /// <summary> The EmailNotifySender property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifySender"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifySender { get; }
        /// <summary> The EmailNotifyRecipient property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyRecipient"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifyRecipient { get; }
        /// <summary> The EmailNotifyOther property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifyOther { get; }
        /// <summary> The EmailNotifyOtherAddress property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyOtherAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailNotifyOtherAddress { get; }
        /// <summary> The EmailNotifyFrom property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailNotifyFrom { get; }
        /// <summary> The EmailNotifySubject property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifySubject"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EmailNotifySubject { get; }
        /// <summary> The EmailNotifyMessage property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 120<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailNotifyMessage { get; }
        /// <summary> The CustomsDocumentsOnly property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CustomsDocumentsOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomsDocumentsOnly { get; }
        /// <summary> The CustomsDescription property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CustomsDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsDescription { get; }
        /// <summary> The CommercialPaperlessInvoice property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialPaperlessInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CommercialPaperlessInvoice { get; }
        /// <summary> The CommercialInvoiceTermsOfSale property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceTermsOfSale"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CommercialInvoiceTermsOfSale { get; }
        /// <summary> The CommercialInvoicePurpose property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoicePurpose"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CommercialInvoicePurpose { get; }
        /// <summary> The CommercialInvoiceComments property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CommercialInvoiceComments { get; }
        /// <summary> The CommercialInvoiceFreight property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceFreight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CommercialInvoiceFreight { get; }
        /// <summary> The CommercialInvoiceInsurance property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceInsurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CommercialInvoiceInsurance { get; }
        /// <summary> The CommercialInvoiceOther property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CommercialInvoiceOther { get; }
        /// <summary> The WorldShipStatus property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."WorldShipStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 WorldShipStatus { get; }
        /// <summary> The PublishedCharges property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PublishedCharges"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal PublishedCharges { get; }
        /// <summary> The NegotiatedRate property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."NegotiatedRate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NegotiatedRate { get; }
        /// <summary> The ReturnService property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReturnService"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ReturnService { get; }
        /// <summary> The ReturnUndeliverableEmail property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReturnUndeliverableEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReturnUndeliverableEmail { get; }
        /// <summary> The ReturnContents property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReturnContents"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReturnContents { get; }
        /// <summary> The UspsTrackingNumber property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."UspsTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UspsTrackingNumber { get; }
        /// <summary> The Endorsement property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Endorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Endorsement { get; }
        /// <summary> The Subclassification property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Subclassification"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Subclassification { get; }
        /// <summary> The PaperlessAdditionalDocumentation property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PaperlessAdditionalDocumentation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean PaperlessAdditionalDocumentation { get; }
        /// <summary> The ShipperRelease property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipperRelease"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ShipperRelease { get; }
        /// <summary> The CarbonNeutral property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CarbonNeutral"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CarbonNeutral { get; }
        /// <summary> The CostCenter property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CostCenter"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CostCenter { get; }
        /// <summary> The IrregularIndicator property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."IrregularIndicator"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IrregularIndicator { get; }
        /// <summary> The Cn22Number property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Cn22Number"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Cn22Number { get; }
        /// <summary> The ShipmentChargeType property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentChargeType { get; }
        /// <summary> The ShipmentChargeAccount property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargeAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipmentChargeAccount { get; }
        /// <summary> The ShipmentChargePostalCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargePostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipmentChargePostalCode { get; }
        /// <summary> The ShipmentChargeCountryCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargeCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipmentChargeCountryCode { get; }
        /// <summary> The UspsPackageID property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."UspsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UspsPackageID { get; }
        /// <summary> The RequestedLabelFormat property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        public IEnumerable<IUpsPackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsShipmentData(IUpsShipmentEntity source);
    }
}
