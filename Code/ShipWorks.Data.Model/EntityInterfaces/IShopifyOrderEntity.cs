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
    /// Entity interface which represents the entity 'ShopifyOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShopifyOrderEntity: IOrderEntity
    {
        
        /// <summary> The ShopifyOrderID property of the Entity ShopifyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrder"."ShopifyOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShopifyOrderID { get; }
        /// <summary> The FulfillmentStatusCode property of the Entity ShopifyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrder"."FulfillmentStatusCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FulfillmentStatusCode { get; }
        /// <summary> The PaymentStatusCode property of the Entity ShopifyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyOrder"."PaymentStatusCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PaymentStatusCode { get; }
        
        
        
        IEnumerable<IShopifyOrderSearchEntity> ShopifyOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShopifyOrderEntity AsReadOnlyShopifyOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShopifyOrderEntity AsReadOnlyShopifyOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShopifyOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShopifyOrderEntity : IShopifyOrderEntity
    {
        
        
        IEnumerable<IShopifyOrderSearchEntity> IShopifyOrderEntity.ShopifyOrderSearch => ShopifyOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShopifyOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShopifyOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IShopifyOrderEntity AsReadOnlyShopifyOrder() =>
            (IShopifyOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IShopifyOrderEntity AsReadOnlyShopifyOrder(IDictionary<object, object> objectMap) =>
            (IShopifyOrderEntity) AsReadOnly(objectMap);
        
    }
}
