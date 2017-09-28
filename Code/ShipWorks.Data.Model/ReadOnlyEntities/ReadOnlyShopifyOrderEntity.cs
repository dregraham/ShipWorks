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
    /// Read-only representation of the entity 'ShopifyOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShopifyOrderEntity : ReadOnlyOrderEntity, IShopifyOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShopifyOrderEntity(IShopifyOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShopifyOrderID = source.ShopifyOrderID;
            FulfillmentStatusCode = source.FulfillmentStatusCode;
            PaymentStatusCode = source.PaymentStatusCode;
            
            
            
            ShopifyOrderSearch = source.ShopifyOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IShopifyOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IShopifyOrderSearchEntity>();

            CopyCustomShopifyOrderData(source);
        }

        
        /// <summary> The ShopifyOrderID property of the Entity ShopifyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrder"."ShopifyOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShopifyOrderID { get; }
        /// <summary> The FulfillmentStatusCode property of the Entity ShopifyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrder"."FulfillmentStatusCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FulfillmentStatusCode { get; }
        /// <summary> The PaymentStatusCode property of the Entity ShopifyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrder"."PaymentStatusCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PaymentStatusCode { get; }
        
        
        
        public IEnumerable<IShopifyOrderSearchEntity> ShopifyOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IShopifyOrderEntity AsReadOnlyShopifyOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IShopifyOrderEntity AsReadOnlyShopifyOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShopifyOrderData(IShopifyOrderEntity source);
    }
}
