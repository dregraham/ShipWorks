﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'ShopSiteStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShopSiteStoreEntity: IStoreEntity
    {
        
        /// <summary> The Username property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The Password property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The CgiUrl property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."CgiUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CgiUrl { get; }
        /// <summary> The RequireSSL property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."RequireSSL"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean RequireSSL { get; }
        /// <summary> The DownloadPageSize property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."DownloadPageSize"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DownloadPageSize { get; }
        /// <summary> The RequestTimeout property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."RequestTimeout"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestTimeout { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IShopSiteStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IShopSiteStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShopSiteStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShopSiteStoreEntity : IShopSiteStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IShopSiteStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IShopSiteStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShopSiteStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShopSiteStoreEntity(this, objectMap);
        }
    }
}