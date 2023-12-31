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
    /// Entity interface which represents the entity 'GrouponStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGrouponStoreEntity: IStoreEntity
    {
        
        /// <summary> The SupplierID property of the Entity GrouponStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponStore"."SupplierID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SupplierID { get; }
        /// <summary> The Token property of the Entity GrouponStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponStore"."Token"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Token { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGrouponStoreEntity AsReadOnlyGrouponStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGrouponStoreEntity AsReadOnlyGrouponStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GrouponStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class GrouponStoreEntity : IGrouponStoreEntity
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
                return (IGrouponStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGrouponStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IGrouponStoreEntity AsReadOnlyGrouponStore() =>
            (IGrouponStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGrouponStoreEntity AsReadOnlyGrouponStore(IDictionary<object, object> objectMap) =>
            (IGrouponStoreEntity) AsReadOnly(objectMap);
        
    }
}
