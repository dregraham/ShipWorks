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
    /// Read-only representation of the entity 'OrderItemAttribute'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderItemAttributeEntity : IOrderItemAttributeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderItemAttributeEntity(IOrderItemAttributeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderItemAttributeID = source.OrderItemAttributeID;
            RowVersion = source.RowVersion;
            OrderItemID = source.OrderItemID;
            Name = source.Name;
            Description = source.Description;
            UnitPrice = source.UnitPrice;
            IsManual = source.IsManual;
            
            
            OrderItem = source.OrderItem?.AsReadOnly(objectMap);
            

            CopyCustomOrderItemAttributeData(source);
        }

        
        /// <summary> The OrderItemAttributeID property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."OrderItemAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OrderItemAttributeID { get; }
        /// <summary> The RowVersion property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The OrderItemID property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."OrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderItemID { get; }
        /// <summary> The Name property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The Description property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The UnitPrice property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."UnitPrice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal UnitPrice { get; }
        /// <summary> The IsManual property of the Entity OrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItemAttribute"."IsManual"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsManual { get; }
        
        
        public IOrderItemEntity OrderItem { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderItemAttributeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderItemAttributeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderItemAttributeData(IOrderItemAttributeEntity source);
    }
}
