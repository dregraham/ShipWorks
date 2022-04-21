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
    /// Read-only representation of the entity 'DhlEcommerceShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDhlEcommerceShipmentEntity : IDhlEcommerceShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDhlEcommerceShipmentEntity(IDhlEcommerceShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            DhlEcommerceAccountID = source.DhlEcommerceAccountID;
            Service = source.Service;
            DeliveredDutyPaid = source.DeliveredDutyPaid;
            NonMachinable = source.NonMachinable;
            SaturdayDelivery = source.SaturdayDelivery;
            RequestedLabelFormat = source.RequestedLabelFormat;
            Contents = source.Contents;
            NonDelivery = source.NonDelivery;
            ShipEngineLabelID = source.ShipEngineLabelID;
            IntegratorTransactionID = source.IntegratorTransactionID;
            StampsTransactionID = source.StampsTransactionID;
            ResidentialDelivery = source.ResidentialDelivery;
            CustomsRecipientTin = source.CustomsRecipientTin;
            CustomsTaxIdType = source.CustomsTaxIdType;
            CustomsTinIssuingAuthority = source.CustomsTinIssuingAuthority;
            ScanFormBatchID = source.ScanFormBatchID;
            PackagingType = source.PackagingType;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            Reference1 = source.Reference1;
            Insurance = source.Insurance;
            InsuranceValue = source.InsuranceValue;
            InsurancePennyOne = source.InsurancePennyOne;
            
            Shipment = (IShipmentEntity) source.Shipment?.AsReadOnly(objectMap);
            
            ScanFormBatch = (IScanFormBatchEntity) source.ScanFormBatch?.AsReadOnly(objectMap);
            

            CopyCustomDhlEcommerceShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The DhlEcommerceAccountID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DhlEcommerceAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DhlEcommerceAccountID { get; }
        /// <summary> The Service property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The DeliveredDutyPaid property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DeliveredDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DeliveredDutyPaid { get; }
        /// <summary> The NonMachinable property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NonMachinable { get; }
        /// <summary> The SaturdayDelivery property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SaturdayDelivery { get; }
        /// <summary> The RequestedLabelFormat property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The Contents property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Contents { get; }
        /// <summary> The NonDelivery property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 NonDelivery { get; }
        /// <summary> The ShipEngineLabelID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ShipEngineLabelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShipEngineLabelID { get; }
        /// <summary> The IntegratorTransactionID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."IntegratorTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Guid> IntegratorTransactionID { get; }
        /// <summary> The StampsTransactionID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."StampsTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Guid> StampsTransactionID { get; }
        /// <summary> The ResidentialDelivery property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ResidentialDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ResidentialDelivery { get; }
        /// <summary> The CustomsRecipientTin property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."CustomsRecipientTin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsRecipientTin { get; }
        /// <summary> The CustomsTaxIdType property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."CustomsTaxIdType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> CustomsTaxIdType { get; }
        /// <summary> The CustomsTinIssuingAuthority property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."CustomsTinIssuingAuthority"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsTinIssuingAuthority { get; }
        /// <summary> The ScanFormBatchID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ScanFormBatchID { get; }
        /// <summary> The PackagingType property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PackagingType { get; }
        /// <summary> The DimsProfileID property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The Reference1 property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Reference1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Reference1 { get; }
        /// <summary> The Insurance property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity DhlEcommerceShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceShipment"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> InsurancePennyOne { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        public IScanFormBatchEntity ScanFormBatch { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlEcommerceShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlEcommerceShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDhlEcommerceShipmentData(IDhlEcommerceShipmentEntity source);
    }
}
