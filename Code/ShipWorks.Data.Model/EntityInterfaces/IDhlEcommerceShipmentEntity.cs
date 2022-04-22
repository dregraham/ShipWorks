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
    /// Entity interface which represents the entity 'DhlEcommerceShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDhlEcommerceShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The DhlEcommerceAccountID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DhlEcommerceAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DhlEcommerceAccountID { get; }
        /// <summary> The Service property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The DeliveredDutyPaid property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DeliveredDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DeliveredDutyPaid { get; }
        /// <summary> The NonMachinable property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean NonMachinable { get; }
        /// <summary> The SaturdayDelivery property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean SaturdayDelivery { get; }
        /// <summary> The RequestedLabelFormat property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        /// <summary> The Contents property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Contents { get; }
        /// <summary> The NonDelivery property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 NonDelivery { get; }
        /// <summary> The ShipEngineLabelID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ShipEngineLabelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShipEngineLabelID { get; }
        /// <summary> The IntegratorTransactionID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."IntegratorTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Guid> IntegratorTransactionID { get; }
        /// <summary> The StampsTransactionID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."StampsTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Guid> StampsTransactionID { get; }
        /// <summary> The ResidentialDelivery property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ResidentialDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ResidentialDelivery { get; }
        /// <summary> The CustomsRecipientTin property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."CustomsRecipientTin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CustomsRecipientTin { get; }
        /// <summary> The CustomsTaxIdType property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."CustomsTaxIdType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> CustomsTaxIdType { get; }
        /// <summary> The CustomsTinIssuingAuthority property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."CustomsTinIssuingAuthority"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CustomsTinIssuingAuthority { get; }
        /// <summary> The ScanFormBatchID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ScanFormBatchID { get; }
        /// <summary> The PackagingType property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PackagingType { get; }
        /// <summary> The DimsProfileID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The Reference1 property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Reference1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Reference1 { get; }
        /// <summary> The Insurance property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean InsurancePennyOne { get; }
        /// <summary> The AncillaryEndorsement property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."AncillaryEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AncillaryEndorsement { get; }
        
        IShipmentEntity Shipment { get; }
        
        IScanFormBatchEntity ScanFormBatch { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlEcommerceShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlEcommerceShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DhlEcommerceShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class DhlEcommerceShipmentEntity : IDhlEcommerceShipmentEntity
    {
        IShipmentEntity IDhlEcommerceShipmentEntity.Shipment => Shipment;
        
        IScanFormBatchEntity IDhlEcommerceShipmentEntity.ScanFormBatch => ScanFormBatch;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlEcommerceShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDhlEcommerceShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDhlEcommerceShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDhlEcommerceShipmentEntity(this, objectMap);
        }

        
    }
}
