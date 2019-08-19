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
    /// Read-only representation of the entity 'ShopifyStore'. <br/><br/>
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
            ApiKey = source.ApiKey;
            Password = source.Password;
            ShopifyNotifyCustomer = source.ShopifyNotifyCustomer;
            ShopifyFulfillmentLocation = source.ShopifyFulfillmentLocation;
            
            
            

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
        /// <summary> The ApiKey property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ApiKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiKey { get; }
        /// <summary> The Password property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The ShopifyNotifyCustomer property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyNotifyCustomer"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ShopifyNotifyCustomer { get; }
        /// <summary> The ShopifyFulfillmentLocation property of the Entity ShopifyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopifyStore"."ShopifyFulfillmentLocation"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShopifyFulfillmentLocation { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IShopifyStoreEntity AsReadOnlyShopifyStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IShopifyStoreEntity AsReadOnlyShopifyStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShopifyStoreData(IShopifyStoreEntity source);
    }
}
