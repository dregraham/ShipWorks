﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'ShopifyOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShopifyOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The ShopifyOrderItemID property of the Entity ShopifyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderItem"."ShopifyOrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShopifyOrderItemID { get; }
        /// <summary> The ShopifyProductID property of the Entity ShopifyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderItem"."ShopifyProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShopifyProductID { get; }
        /// <summary> The InventoryItemID property of the Entity ShopifyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderItem"."InventoryItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> InventoryItemID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShopifyOrderItemEntity AsReadOnlyShopifyOrderItem();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShopifyOrderItemEntity AsReadOnlyShopifyOrderItem(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShopifyOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShopifyOrderItemEntity : IShopifyOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShopifyOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShopifyOrderItemEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IShopifyOrderItemEntity AsReadOnlyShopifyOrderItem() =>
            (IShopifyOrderItemEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IShopifyOrderItemEntity AsReadOnlyShopifyOrderItem(IDictionary<object, object> objectMap) =>
            (IShopifyOrderItemEntity) AsReadOnly(objectMap);
        
    }
}
