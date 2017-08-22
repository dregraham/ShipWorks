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
    /// Entity interface which represents the entity 'ShipmentReturnItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShipmentReturnItemEntity
    {
        
        /// <summary> The ShipmentReturnItemID property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."ShipmentReturnItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShipmentReturnItemID { get; }
        /// <summary> The RowVersion property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentID property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The Name property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The Quantity property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Quantity { get; }
        /// <summary> The Weight property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The Notes property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."Notes"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Notes { get; }
        /// <summary> The SKU property of the Entity ShipmentReturnItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentReturnItem"."SKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SKU { get; }
        
        
        IShipmentEntity Shipment { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipmentReturnItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipmentReturnItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShipmentReturnItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShipmentReturnItemEntity : IShipmentReturnItemEntity
    {
        
        IShipmentEntity IShipmentReturnItemEntity.Shipment => Shipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentReturnItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShipmentReturnItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShipmentReturnItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShipmentReturnItemEntity(this, objectMap);
        }
    }
}
