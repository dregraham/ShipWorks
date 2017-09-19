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
    /// Read-only representation of the entity 'ShipmentReturnItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShipmentReturnItemEntity : IShipmentReturnItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShipmentReturnItemEntity(IShipmentReturnItemEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentReturnItemID = source.ShipmentReturnItemID;
            RowVersion = source.RowVersion;
            ShipmentID = source.ShipmentID;
            Name = source.Name;
            Quantity = source.Quantity;
            Weight = source.Weight;
            Notes = source.Notes;
            SKU = source.SKU;
            Code = source.Code;
            
            
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            

            CopyCustomShipmentReturnItemData(source);
        }

        
        /// <summary> The ShipmentReturnItemID property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."ShipmentReturnItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShipmentReturnItemID { get; }
        /// <summary> The RowVersion property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentID property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The Name property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The Quantity property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Quantity { get; }
        /// <summary> The Weight property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The Notes property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Notes"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Notes { get; }
        /// <summary> The SKU property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."SKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SKU { get; }
        /// <summary> The Code property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Code"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Code { get; }
        
        
        public IShipmentEntity Shipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentReturnItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentReturnItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShipmentReturnItemData(IShipmentReturnItemEntity source);
    }
}
