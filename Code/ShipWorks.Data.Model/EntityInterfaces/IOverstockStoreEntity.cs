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
    /// Entity interface which represents the entity 'OverstockStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOverstockStoreEntity: IStoreEntity
    {
        
        /// <summary> The Username property of the Entity OverstockStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The Password property of the Entity OverstockStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOverstockStoreEntity AsReadOnlyOverstockStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOverstockStoreEntity AsReadOnlyOverstockStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OverstockStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class OverstockStoreEntity : IOverstockStoreEntity
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
                return (IOverstockStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOverstockStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IOverstockStoreEntity AsReadOnlyOverstockStore() =>
            (IOverstockStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IOverstockStoreEntity AsReadOnlyOverstockStore(IDictionary<object, object> objectMap) =>
            (IOverstockStoreEntity) AsReadOnly(objectMap);
        
    }
}
