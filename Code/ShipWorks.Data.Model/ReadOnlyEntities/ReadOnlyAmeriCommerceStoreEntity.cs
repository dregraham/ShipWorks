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
    /// Read-only representation of the entity 'AmeriCommerceStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmeriCommerceStoreEntity : ReadOnlyStoreEntity, IAmeriCommerceStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmeriCommerceStoreEntity(IAmeriCommerceStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Username = source.Username;
            Password = source.Password;
            StoreUrl = source.StoreUrl;
            StoreCode = source.StoreCode;
            StatusCodes = source.StatusCodes;
            
            
            

            CopyCustomAmeriCommerceStoreData(source);
        }

        
        /// <summary> The Username property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Username { get; }
        /// <summary> The Password property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The StoreUrl property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StoreUrl { get; }
        /// <summary> The StoreCode property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."StoreCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 StoreCode { get; }
        /// <summary> The StatusCodes property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StatusCodes { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmeriCommerceStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmeriCommerceStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmeriCommerceStoreData(IAmeriCommerceStoreEntity source);
    }
}
