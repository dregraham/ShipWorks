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
    /// Entity interface which represents the entity 'AmazonStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonStoreEntity: IStoreEntity
    {
        
        /// <summary> The AmazonApi property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AmazonApi"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AmazonApi { get; }
        /// <summary> The AmazonApiRegion property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AmazonApiRegion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonApiRegion { get; }
        /// <summary> The SellerCentralUsername property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."SellerCentralUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SellerCentralUsername { get; }
        /// <summary> The SellerCentralPassword property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."SellerCentralPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SellerCentralPassword { get; }
        /// <summary> The MerchantName property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MerchantName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MerchantName { get; }
        /// <summary> The MerchantToken property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MerchantToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MerchantToken { get; }
        /// <summary> The AccessKeyID property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AccessKeyID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AccessKeyID { get; }
        /// <summary> The AuthToken property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AuthToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AuthToken { get; }
        /// <summary> The Cookie property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."Cookie"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Cookie { get; }
        /// <summary> The CookieExpires property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."CookieExpires"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CookieExpires { get; }
        /// <summary> The CookieWaitUntil property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."CookieWaitUntil"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CookieWaitUntil { get; }
        /// <summary> The Certificate property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."Certificate"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.Byte[] Certificate { get; }
        /// <summary> The WeightDownloads property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."WeightDownloads"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String WeightDownloads { get; }
        /// <summary> The MerchantID property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MerchantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MerchantID { get; }
        /// <summary> The MarketplaceID property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."MarketplaceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceID { get; }
        /// <summary> The ExcludeFBA property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."ExcludeFBA"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ExcludeFBA { get; }
        /// <summary> The DomainName property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."DomainName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DomainName { get; }
        /// <summary> The AmazonShippingToken property of the Entity AmazonStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonStore"."AmazonShippingToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonShippingToken { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmazonStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmazonStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonStoreEntity : IAmazonStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmazonStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IAmazonStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonStoreEntity(this, objectMap);
        }
    }
}
