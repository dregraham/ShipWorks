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
    /// Entity interface which represents the entity 'WalmartOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWalmartOrderSearchEntity
    {
        
        /// <summary> The WalmartOrderSearchID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."WalmartOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 WalmartOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The PurchaseOrderID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."PurchaseOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PurchaseOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IWalmartOrderEntity WalmartOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWalmartOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWalmartOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WalmartOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class WalmartOrderSearchEntity : IWalmartOrderSearchEntity
    {
        
        IWalmartOrderEntity IWalmartOrderSearchEntity.WalmartOrder => WalmartOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWalmartOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IWalmartOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IWalmartOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWalmartOrderSearchEntity(this, objectMap);
        }

        
    }
}
