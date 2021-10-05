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
    /// Read-only representation of the entity 'EtsyStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEtsyStoreEntity : ReadOnlyStoreEntity, IEtsyStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEtsyStoreEntity(IEtsyStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EtsyShopID = source.EtsyShopID;
            EtsyLoginName = source.EtsyLoginName;
            EtsyStoreName = source.EtsyStoreName;
            OAuthToken = source.OAuthToken;
            OAuthTokenSecret = source.OAuthTokenSecret;
            
            
            

            CopyCustomEtsyStoreData(source);
        }

        
        /// <summary> The EtsyShopID property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."EtsyShopID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EtsyShopID { get; }
        /// <summary> The EtsyLoginName property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."EtsyLogin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EtsyLoginName { get; }
        /// <summary> The EtsyStoreName property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."EtsyStoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EtsyStoreName { get; }
        /// <summary> The OAuthToken property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."OAuthToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OAuthToken { get; }
        /// <summary> The OAuthTokenSecret property of the Entity EtsyStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyStore"."OAuthTokenSecret"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OAuthTokenSecret { get; }
        
        
        
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
        public IEtsyStoreEntity AsReadOnlyEtsyStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IEtsyStoreEntity AsReadOnlyEtsyStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEtsyStoreData(IEtsyStoreEntity source);
    }
}
