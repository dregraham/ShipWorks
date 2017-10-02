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
    /// Entity interface which represents the entity 'OrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderSearchEntity
    {
        
        /// <summary> The OrderSearchID property of the Entity OrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderSearch"."OrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OrderSearchID { get; }
        /// <summary> The OrderID property of the Entity OrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity OrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity OrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity OrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumberComplete { get; }
        /// <summary> The IsManual property of the Entity OrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderSearch"."IsManual"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsManual { get; }
        /// <summary> The OriginalOrderID property of the Entity OrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IOrderEntity Order { get; }
        IStoreEntity Store { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderSearchEntity : IOrderSearchEntity
    {
        
        IOrderEntity IOrderSearchEntity.Order => Order;
        IStoreEntity IOrderSearchEntity.Store => Store;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderSearchEntity(this, objectMap);
        }

        
    }
}
