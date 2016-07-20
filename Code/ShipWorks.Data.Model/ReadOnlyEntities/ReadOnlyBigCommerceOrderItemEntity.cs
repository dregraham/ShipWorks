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
    /// Entity interface which represents the entity 'BigCommerceOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyBigCommerceOrderItemEntity : ReadOnlyOrderItemEntity, IBigCommerceOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyBigCommerceOrderItemEntity(IBigCommerceOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderAddressID = source.OrderAddressID;
            OrderProductID = source.OrderProductID;
            IsDigitalItem = source.IsDigitalItem;
            EventDate = source.EventDate;
            EventName = source.EventName;
            
            
            

            CopyCustomBigCommerceOrderItemData(source);
        }

        
        /// <summary> The OrderAddressID property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."OrderAddressID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderAddressID { get; }
        /// <summary> The OrderProductID property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."OrderProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderProductID { get; }
        /// <summary> The IsDigitalItem property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."IsDigitalItem"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsDigitalItem { get; }
        /// <summary> The EventDate property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."EventDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> EventDate { get; }
        /// <summary> The EventName property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."EventName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String EventName { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBigCommerceOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBigCommerceOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomBigCommerceOrderItemData(IBigCommerceOrderItemEntity source);
    }
}
