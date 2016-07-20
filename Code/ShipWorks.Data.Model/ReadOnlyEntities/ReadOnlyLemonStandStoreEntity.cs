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
    /// Entity interface which represents the entity 'LemonStandStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyLemonStandStoreEntity : ReadOnlyStoreEntity, ILemonStandStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyLemonStandStoreEntity(ILemonStandStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Token = source.Token;
            StoreURL = source.StoreURL;
            StatusCodes = source.StatusCodes;
            
            
            

            CopyCustomLemonStandStoreData(source);
        }

        
        /// <summary> The Token property of the Entity LemonStandStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandStore"."Token"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Token { get; }
        /// <summary> The StoreURL property of the Entity LemonStandStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandStore"."StoreURL"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StoreURL { get; }
        /// <summary> The StatusCodes property of the Entity LemonStandStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String StatusCodes { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ILemonStandStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ILemonStandStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomLemonStandStoreData(ILemonStandStoreEntity source);
    }
}
