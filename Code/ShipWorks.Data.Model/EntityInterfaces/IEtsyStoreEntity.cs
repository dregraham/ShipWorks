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
    /// Entity interface which represents the entity 'EtsyStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEtsyStoreEntity: IStoreEntity
    {
        
        /// <summary> The EtsyShopID property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."EtsyShopID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EtsyShopID { get; }
        /// <summary> The EtsyLoginName property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."EtsyLogin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EtsyLoginName { get; }
        /// <summary> The EtsyStoreName property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."EtsyStoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EtsyStoreName { get; }
        /// <summary> The OAuthToken property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."OAuthToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OAuthToken { get; }
        /// <summary> The OAuthTokenSecret property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."OAuthTokenSecret"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OAuthTokenSecret { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IEtsyStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IEtsyStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EtsyStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class EtsyStoreEntity : IEtsyStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IEtsyStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IEtsyStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEtsyStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEtsyStoreEntity(this, objectMap);
        }
    }
}
