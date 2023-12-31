﻿///////////////////////////////////////////////////////////////
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
    /// Read-only representation of the entity 'DhlExpressShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDhlExpressShipmentEntity : IDhlExpressShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDhlExpressShipmentEntity(IDhlExpressShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            DhlExpressAccountID = source.DhlExpressAccountID;
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
            
            Shipment = (IShipmentEntity) source.Shipment?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).OfType<IDhlExpressPackageEntity>().ToReadOnly() ??
                Enumerable.Empty<IDhlExpressPackageEntity>();

            CopyCustomDhlExpressShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The DhlExpressAccountID property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."DhlExpressAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DhlExpressAccountID { get; }
        /// <summary> The Service property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The DeliveredDutyPaid property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."DeliveredDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DeliveredDutyPaid { get; }
        /// <summary> The NonMachinable property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NonMachinable { get; }
        /// <summary> The SaturdayDelivery property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SaturdayDelivery { get; }
        /// <summary> The RequestedLabelFormat property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The Contents property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Contents { get; }
        /// <summary> The NonDelivery property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 NonDelivery { get; }
        /// <summary> The ShipEngineLabelID property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."ShipEngineLabelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShipEngineLabelID { get; }
        /// <summary> The IntegratorTransactionID property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."IntegratorTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Guid> IntegratorTransactionID { get; }
        /// <summary> The StampsTransactionID property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."StampsTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Guid> StampsTransactionID { get; }
        /// <summary> The ResidentialDelivery property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."ResidentialDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ResidentialDelivery { get; }
        /// <summary> The CustomsRecipientTin property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."CustomsRecipientTin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsRecipientTin { get; }
        /// <summary> The CustomsTaxIdType property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."CustomsTaxIdType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> CustomsTaxIdType { get; }
        /// <summary> The CustomsTinIssuingAuthority property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."CustomsTinIssuingAuthority"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsTinIssuingAuthority { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        public IEnumerable<IDhlExpressPackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDhlExpressShipmentData(IDhlExpressShipmentEntity source);
    }
}
