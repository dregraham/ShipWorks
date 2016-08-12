///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'ShopifyStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShopifyStoreEntity: IStoreEntity
    {
        
        /// <summary> The ShopifyShopUrlName property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyShopUrlName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShopifyShopUrlName { get; }
        /// <summary> The ShopifyShopDisplayName property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyShopDisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShopifyShopDisplayName { get; }
        /// <summary> The ShopifyAccessToken property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyAccessToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShopifyAccessToken { get; }
        /// <summary> The ShopifyRequestedShippingOption property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyRequestedShippingOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShopifyRequestedShippingOption { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IShopifyStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IShopifyStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShopifyStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShopifyStoreEntity : IShopifyStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IShopifyStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IShopifyStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShopifyStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShopifyStoreEntity(this, objectMap);
        }
    }
}
