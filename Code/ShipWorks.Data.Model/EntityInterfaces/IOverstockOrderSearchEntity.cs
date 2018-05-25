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
    /// Entity interface which represents the entity 'OverstockOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOverstockOrderSearchEntity
    {
        
        /// <summary> The OverstockOrderSearchID property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."OverstockOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OverstockOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        /// <summary> The SalesChannelName property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."SalesChannelName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SalesChannelName { get; }
        /// <summary> The WarehouseCode property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."WarehouseCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String WarehouseCode { get; }
        
        
        IOverstockOrderEntity OverstockOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOverstockOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOverstockOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OverstockOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class OverstockOrderSearchEntity : IOverstockOrderSearchEntity
    {
        
        IOverstockOrderEntity IOverstockOrderSearchEntity.OverstockOrder => OverstockOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOverstockOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOverstockOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOverstockOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOverstockOrderSearchEntity(this, objectMap);
        }

        
    }
}
