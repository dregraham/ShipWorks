﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'PlatformStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPlatformStoreEntity: IStoreEntity
    {
        
        /// <summary> The OrderSourceID property of the Entity PlatformStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PlatformStore"."OrderSourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderSourceID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPlatformStoreEntity AsReadOnlyPlatformStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPlatformStoreEntity AsReadOnlyPlatformStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PlatformStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class PlatformStoreEntity : IPlatformStoreEntity
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
                return (IPlatformStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPlatformStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IPlatformStoreEntity AsReadOnlyPlatformStore() =>
            (IPlatformStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IPlatformStoreEntity AsReadOnlyPlatformStore(IDictionary<object, object> objectMap) =>
            (IPlatformStoreEntity) AsReadOnly(objectMap);
        
    }
}
