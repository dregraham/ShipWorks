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
    /// Read-only representation of the entity 'SparkPayStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlySparkPayStoreEntity : ReadOnlyStoreEntity, ISparkPayStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlySparkPayStoreEntity(ISparkPayStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Token = source.Token;
            StoreUrl = source.StoreUrl;
            StatusCodes = source.StatusCodes;
            
            
            

            CopyCustomSparkPayStoreData(source);
        }

        
        /// <summary> The Token property of the Entity SparkPayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SparkPayStore"."Token"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Token { get; }
        /// <summary> The StoreUrl property of the Entity SparkPayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SparkPayStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StoreUrl { get; }
        /// <summary> The StatusCodes property of the Entity SparkPayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SparkPayStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String StatusCodes { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISparkPayStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISparkPayStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomSparkPayStoreData(ISparkPayStoreEntity source);
    }
}
