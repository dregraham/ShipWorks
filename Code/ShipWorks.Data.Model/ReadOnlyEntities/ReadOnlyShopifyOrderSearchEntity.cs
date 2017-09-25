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
    /// Read-only representation of the entity 'ShopifyOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShopifyOrderSearchEntity : IShopifyOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShopifyOrderSearchEntity(IShopifyOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShopifyOrderSearchID = source.ShopifyOrderSearchID;
            OrderID = source.OrderID;
            ShopifyOrderID = source.ShopifyOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            ShopifyOrder = source.ShopifyOrder?.AsReadOnly(objectMap);
            

            CopyCustomShopifyOrderSearchData(source);
        }

        
        /// <summary> The ShopifyOrderSearchID property of the Entity ShopifyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderSearch"."ShopifyOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShopifyOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ShopifyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The ShopifyOrderID property of the Entity ShopifyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderSearch"."ShopifyOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShopifyOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity ShopifyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IShopifyOrderEntity ShopifyOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShopifyOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShopifyOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShopifyOrderSearchData(IShopifyOrderSearchEntity source);
    }
}
