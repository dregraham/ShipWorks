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
    /// Entity interface which represents the entity 'OrderItemAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderItemAttributeEntity
    {
        
        /// <summary> The OrderItemAttributeID property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."OrderItemAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OrderItemAttributeID { get; }
        /// <summary> The RowVersion property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The OrderItemID property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."OrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderItemID { get; }
        /// <summary> The Name property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The Description property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The UnitPrice property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."UnitPrice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal UnitPrice { get; }
        /// <summary> The IsManual property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."IsManual"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsManual { get; }
        
        
        IOrderItemEntity OrderItem { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderItemAttributeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderItemAttributeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OrderItemAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderItemAttributeEntity : IOrderItemAttributeEntity
    {
        
        IOrderItemEntity IOrderItemAttributeEntity.OrderItem => OrderItem;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderItemAttributeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOrderItemAttributeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOrderItemAttributeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderItemAttributeEntity(this, objectMap);
        }
    }
}
