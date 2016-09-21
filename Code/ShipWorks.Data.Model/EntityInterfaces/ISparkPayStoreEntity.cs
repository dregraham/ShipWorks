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
    /// Entity interface which represents the entity 'SparkPayStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ISparkPayStoreEntity: IStoreEntity
    {
        
        /// <summary> The Token property of the Entity SparkPayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SparkPayStore"."Token"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Token { get; }
        /// <summary> The StoreUrl property of the Entity SparkPayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SparkPayStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StoreUrl { get; }
        /// <summary> The StatusCodes property of the Entity SparkPayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SparkPayStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String StatusCodes { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ISparkPayStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ISparkPayStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'SparkPayStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class SparkPayStoreEntity : ISparkPayStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISparkPayStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new ISparkPayStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ISparkPayStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlySparkPayStoreEntity(this, objectMap);
        }
    }
}
