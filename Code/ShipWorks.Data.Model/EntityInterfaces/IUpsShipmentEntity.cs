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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'UpsShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The UpsAccountID property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."UpsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UpsAccountID { get; }
        /// <summary> The Service property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The SaturdayDelivery property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean SaturdayDelivery { get; }
        /// <summary> The CodEnabled property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CodEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CodEnabled { get; }
        /// <summary> The CodAmount property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CodAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal CodAmount { get; }
        /// <summary> The CodPaymentType property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CodPaymentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 CodPaymentType { get; }
        /// <summary> The DeliveryConfirmation property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DeliveryConfirmation { get; }
        /// <summary> The ReferenceNumber property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReferenceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReferenceNumber { get; }
        /// <summary> The ReferenceNumber2 property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReferenceNumber2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReferenceNumber2 { get; }
        /// <summary> The PayorType property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PayorType { get; }
        /// <summary> The PayorAccount property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PayorAccount { get; }
        /// <summary> The PayorPostalCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PayorPostalCode { get; }
        /// <summary> The PayorCountryCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PayorCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PayorCountryCode { get; }
        /// <summary> The EmailNotifySender property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifySender"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 EmailNotifySender { get; }
        /// <summary> The EmailNotifyRecipient property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyRecipient"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 EmailNotifyRecipient { get; }
        /// <summary> The EmailNotifyOther property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 EmailNotifyOther { get; }
        /// <summary> The EmailNotifyOtherAddress property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyOtherAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailNotifyOtherAddress { get; }
        /// <summary> The EmailNotifyFrom property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailNotifyFrom { get; }
        /// <summary> The EmailNotifySubject property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifySubject"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 EmailNotifySubject { get; }
        /// <summary> The EmailNotifyMessage property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."EmailNotifyMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 120<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailNotifyMessage { get; }
        /// <summary> The CustomsDocumentsOnly property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CustomsDocumentsOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CustomsDocumentsOnly { get; }
        /// <summary> The CustomsDescription property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CustomsDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CustomsDescription { get; }
        /// <summary> The CommercialPaperlessInvoice property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialPaperlessInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CommercialPaperlessInvoice { get; }
        /// <summary> The CommercialInvoiceTermsOfSale property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceTermsOfSale"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 CommercialInvoiceTermsOfSale { get; }
        /// <summary> The CommercialInvoicePurpose property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoicePurpose"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 CommercialInvoicePurpose { get; }
        /// <summary> The CommercialInvoiceComments property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CommercialInvoiceComments { get; }
        /// <summary> The CommercialInvoiceFreight property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceFreight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal CommercialInvoiceFreight { get; }
        /// <summary> The CommercialInvoiceInsurance property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceInsurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal CommercialInvoiceInsurance { get; }
        /// <summary> The CommercialInvoiceOther property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CommercialInvoiceOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal CommercialInvoiceOther { get; }
        /// <summary> The WorldShipStatus property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."WorldShipStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 WorldShipStatus { get; }
        /// <summary> The PublishedCharges property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PublishedCharges"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal PublishedCharges { get; }
        /// <summary> The NegotiatedRate property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."NegotiatedRate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean NegotiatedRate { get; }
        /// <summary> The ReturnService property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReturnService"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ReturnService { get; }
        /// <summary> The ReturnUndeliverableEmail property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReturnUndeliverableEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReturnUndeliverableEmail { get; }
        /// <summary> The ReturnContents property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ReturnContents"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReturnContents { get; }
        /// <summary> The UspsTrackingNumber property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."UspsTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UspsTrackingNumber { get; }
        /// <summary> The Endorsement property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Endorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Endorsement { get; }
        /// <summary> The Subclassification property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Subclassification"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Subclassification { get; }
        /// <summary> The PaperlessAdditionalDocumentation property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."PaperlessAdditionalDocumentation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean PaperlessAdditionalDocumentation { get; }
        /// <summary> The ShipperRelease property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipperRelease"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ShipperRelease { get; }
        /// <summary> The CarbonNeutral property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CarbonNeutral"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CarbonNeutral { get; }
        /// <summary> The CostCenter property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."CostCenter"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CostCenter { get; }
        /// <summary> The IrregularIndicator property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."IrregularIndicator"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 IrregularIndicator { get; }
        /// <summary> The Cn22Number property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."Cn22Number"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Cn22Number { get; }
        /// <summary> The ShipmentChargeType property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentChargeType { get; }
        /// <summary> The ShipmentChargeAccount property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargeAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipmentChargeAccount { get; }
        /// <summary> The ShipmentChargePostalCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargePostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipmentChargePostalCode { get; }
        /// <summary> The ShipmentChargeCountryCode property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."ShipmentChargeCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipmentChargeCountryCode { get; }
        /// <summary> The UspsPackageID property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."UspsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UspsPackageID { get; }
        /// <summary> The RequestedLabelFormat property of the Entity UpsShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        
        IShipmentEntity Shipment { get; }
        
        
        IEnumerable<IUpsPackageEntity> Packages { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsShipmentEntity : IUpsShipmentEntity
    {
        IShipmentEntity IUpsShipmentEntity.Shipment => Shipment;
        
        
        IEnumerable<IUpsPackageEntity> IUpsShipmentEntity.Packages => Packages;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsShipmentEntity(this, objectMap);
        }

        
    }
}
