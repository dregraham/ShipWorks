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
    /// Entity interface which represents the entity 'BigCommerceStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IBigCommerceStoreEntity: IStoreEntity
    {
        
        /// <summary> The ApiUrl property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."ApiUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 110<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiUrl { get; }
        /// <summary> The ApiUserName property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."ApiUserName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 65<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiUserName { get; }
        /// <summary> The ApiToken property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."ApiToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ApiToken { get; }
        /// <summary> The StatusCodes property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String StatusCodes { get; }
        /// <summary> The WeightUnitOfMeasure property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."WeightUnitOfMeasure"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 WeightUnitOfMeasure { get; }
        /// <summary> The DownloadModifiedNumberOfDaysBack property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."DownloadModifiedNumberOfDaysBack"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DownloadModifiedNumberOfDaysBack { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IBigCommerceStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IBigCommerceStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'BigCommerceStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class BigCommerceStoreEntity : IBigCommerceStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBigCommerceStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IBigCommerceStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IBigCommerceStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyBigCommerceStoreEntity(this, objectMap);
        }
    }
}
