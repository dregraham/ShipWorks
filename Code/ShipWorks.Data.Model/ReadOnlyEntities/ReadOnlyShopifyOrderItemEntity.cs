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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'ShopifyOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShopifyOrderItemEntity : ReadOnlyOrderItemEntity, IShopifyOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShopifyOrderItemEntity(IShopifyOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShopifyOrderItemID = source.ShopifyOrderItemID;
            ShopifyProductID = source.ShopifyProductID;
            InventoryItemID = source.InventoryItemID;
            
            
            

            CopyCustomShopifyOrderItemData(source);
        }

        
        /// <summary> The ShopifyOrderItemID property of the Entity ShopifyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderItem"."ShopifyOrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShopifyOrderItemID { get; }
        /// <summary> The ShopifyProductID property of the Entity ShopifyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderItem"."ShopifyProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShopifyProductID { get; }
        /// <summary> The InventoryItemID property of the Entity ShopifyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderItem"."InventoryItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> InventoryItemID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IShopifyOrderItemEntity AsReadOnlyShopifyOrderItem() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IShopifyOrderItemEntity AsReadOnlyShopifyOrderItem(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShopifyOrderItemData(IShopifyOrderItemEntity source);
    }
}
