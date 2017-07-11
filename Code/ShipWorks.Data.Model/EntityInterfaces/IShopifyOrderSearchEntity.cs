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
    /// Entity interface which represents the entity 'ShopifyOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShopifyOrderSearchEntity
    {
        
        /// <summary> The ShopifyOrderSearchID property of the Entity ShopifyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderSearch"."ShopifyOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShopifyOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ShopifyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The ShopifyOrderID property of the Entity ShopifyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderSearch"."ShopifyOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShopifyOrderID { get; }
        
        
        IShopifyOrderEntity ShopifyOrder { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShopifyOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShopifyOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShopifyOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShopifyOrderSearchEntity : IShopifyOrderSearchEntity
    {
        
        IShopifyOrderEntity IShopifyOrderSearchEntity.ShopifyOrder => ShopifyOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShopifyOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShopifyOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShopifyOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShopifyOrderSearchEntity(this, objectMap);
        }
    }
}
