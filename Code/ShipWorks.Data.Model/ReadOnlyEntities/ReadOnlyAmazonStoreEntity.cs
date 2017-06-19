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
    /// Read-only representation of the entity 'AmazonStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonStoreEntity : ReadOnlyStoreEntity, IAmazonStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonStoreEntity(IAmazonStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonApi = source.AmazonApi;
            AmazonApiRegion = source.AmazonApiRegion;
            SellerCentralUsername = source.SellerCentralUsername;
            SellerCentralPassword = source.SellerCentralPassword;
            MerchantName = source.MerchantName;
            MerchantToken = source.MerchantToken;
            AccessKeyID = source.AccessKeyID;
            AuthToken = source.AuthToken;
            Cookie = source.Cookie;
            CookieExpires = source.CookieExpires;
            CookieWaitUntil = source.CookieWaitUntil;
            Certificate = source.Certificate;
            WeightDownloads = source.WeightDownloads;
            MerchantID = source.MerchantID;
            MarketplaceID = source.MarketplaceID;
            ExcludeFBA = source.ExcludeFBA;
            DomainName = source.DomainName;
            
            
            

            CopyCustomAmazonStoreData(source);
        }

        
        /// <summary> The AmazonApi property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AmazonApi"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AmazonApi { get; }
        /// <summary> The AmazonApiRegion property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AmazonApiRegion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonApiRegion { get; }
        /// <summary> The SellerCentralUsername property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."SellerCentralUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SellerCentralUsername { get; }
        /// <summary> The SellerCentralPassword property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."SellerCentralPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SellerCentralPassword { get; }
        /// <summary> The MerchantName property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MerchantName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MerchantName { get; }
        /// <summary> The MerchantToken property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MerchantToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MerchantToken { get; }
        /// <summary> The AccessKeyID property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AccessKeyID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AccessKeyID { get; }
        /// <summary> The AuthToken property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AuthToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AuthToken { get; }
        /// <summary> The Cookie property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."Cookie"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Cookie { get; }
        /// <summary> The CookieExpires property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."CookieExpires"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CookieExpires { get; }
        /// <summary> The CookieWaitUntil property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."CookieWaitUntil"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CookieWaitUntil { get; }
        /// <summary> The Certificate property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."Certificate"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.Byte[] Certificate { get; }
        /// <summary> The WeightDownloads property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."WeightDownloads"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String WeightDownloads { get; }
        /// <summary> The MerchantID property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MerchantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MerchantID { get; }
        /// <summary> The MarketplaceID property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MarketplaceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceID { get; }
        /// <summary> The ExcludeFBA property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."ExcludeFBA"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ExcludeFBA { get; }
        /// <summary> The DomainName property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."DomainName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DomainName { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmazonStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmazonStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonStoreData(IAmazonStoreEntity source);
    }
}
