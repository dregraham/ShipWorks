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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Entity interface which represents the entity 'ShopifyStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShopifyStoreEntity : ReadOnlyStoreEntity, IShopifyStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShopifyStoreEntity(IShopifyStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShopifyShopUrlName = source.ShopifyShopUrlName;
            ShopifyShopDisplayName = source.ShopifyShopDisplayName;
            ShopifyAccessToken = source.ShopifyAccessToken;
            ShopifyRequestedShippingOption = source.ShopifyRequestedShippingOption;
            
            
            

            CopyCustomShopifyStoreData(source);
        }

        
        /// <summary> The ShopifyShopUrlName property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyShopUrlName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShopifyShopUrlName { get; }
        /// <summary> The ShopifyShopDisplayName property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyShopDisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShopifyShopDisplayName { get; }
        /// <summary> The ShopifyAccessToken property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyAccessToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShopifyAccessToken { get; }
        /// <summary> The ShopifyRequestedShippingOption property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyRequestedShippingOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShopifyRequestedShippingOption { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IShopifyStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IShopifyStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShopifyStoreData(IShopifyStoreEntity source);
    }
}
