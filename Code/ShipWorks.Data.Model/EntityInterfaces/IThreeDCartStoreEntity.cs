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
    /// Entity interface which represents the entity 'ThreeDCartStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IThreeDCartStoreEntity: IStoreEntity
    {
        
        /// <summary> The StoreUrl property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 110<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StoreUrl { get; }
        /// <summary> The ApiUserKey property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."ApiUserKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 65<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiUserKey { get; }
        /// <summary> The TimeZoneID property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."TimeZoneID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String TimeZoneID { get; }
        /// <summary> The StatusCodes property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String StatusCodes { get; }
        /// <summary> The DownloadModifiedNumberOfDaysBack property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."DownloadModifiedNumberOfDaysBack"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DownloadModifiedNumberOfDaysBack { get; }
        /// <summary> The RestUser property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."RestUser"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean RestUser { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartStoreEntity AsReadOnlyThreeDCartStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartStoreEntity AsReadOnlyThreeDCartStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ThreeDCartStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class ThreeDCartStoreEntity : IThreeDCartStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IThreeDCartStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyThreeDCartStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IThreeDCartStoreEntity AsReadOnlyThreeDCartStore() =>
            (IThreeDCartStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IThreeDCartStoreEntity AsReadOnlyThreeDCartStore(IDictionary<object, object> objectMap) =>
            (IThreeDCartStoreEntity) AsReadOnly(objectMap);
        
    }
}
