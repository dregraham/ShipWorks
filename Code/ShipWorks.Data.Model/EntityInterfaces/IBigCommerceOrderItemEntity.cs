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
    /// Entity interface which represents the entity 'BigCommerceOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IBigCommerceOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The OrderAddressID property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."OrderAddressID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderAddressID { get; }
        /// <summary> The OrderProductID property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."OrderProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderProductID { get; }
        /// <summary> The IsDigitalItem property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."IsDigitalItem"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsDigitalItem { get; }
        /// <summary> The EventDate property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."EventDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> EventDate { get; }
        /// <summary> The EventName property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."EventName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String EventName { get; }
        /// <summary> The OriginalOrderID property of the Entity BigCommerceOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceOrderItem"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IBigCommerceOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IBigCommerceOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'BigCommerceOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class BigCommerceOrderItemEntity : IBigCommerceOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBigCommerceOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IBigCommerceOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IBigCommerceOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyBigCommerceOrderItemEntity(this, objectMap);
        }
    }
}
