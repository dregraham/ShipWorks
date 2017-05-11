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
    /// Read-only representation of the entity 'ShopSiteStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShopSiteStoreEntity : ReadOnlyStoreEntity, IShopSiteStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShopSiteStoreEntity(IShopSiteStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Username = source.Username;
            Password = source.Password;
            ApiUrl = source.ApiUrl;
            RequireSSL = source.RequireSSL;
            DownloadPageSize = source.DownloadPageSize;
            RequestTimeout = source.RequestTimeout;
            ShopSiteAuthentication = source.ShopSiteAuthentication;
            OauthClientID = source.OauthClientID;
            OauthSecretKey = source.OauthSecretKey;
            Identifier = source.Identifier;
            OauthAuthorizationCode = source.OauthAuthorizationCode;
            
            
            

            CopyCustomShopSiteStoreData(source);
        }

        
        /// <summary> The Username property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Username { get; }
        /// <summary> The Password property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The ApiUrl property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."ApiUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiUrl { get; }
        /// <summary> The RequireSSL property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."RequireSSL"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean RequireSSL { get; }
        /// <summary> The DownloadPageSize property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."DownloadPageSize"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DownloadPageSize { get; }
        /// <summary> The RequestTimeout property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."RequestTimeout"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestTimeout { get; }
        /// <summary> The ShopSiteAuthentication property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."ShopSiteAuthentication"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public Interapptive.Shared.Enums.ShopSiteAuthenticationType ShopSiteAuthentication { get; }
        /// <summary> The OauthClientID property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."OauthClientID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OauthClientID { get; }
        /// <summary> The OauthSecretKey property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."OauthSecretKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OauthSecretKey { get; }
        /// <summary> The Identifier property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."Identifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Identifier { get; }
        /// <summary> The OauthAuthorizationCode property of the Entity ShopSiteStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShopSiteStore"."OauthAuthorizationCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OauthAuthorizationCode { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IShopSiteStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IShopSiteStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShopSiteStoreData(IShopSiteStoreEntity source);
    }
}
