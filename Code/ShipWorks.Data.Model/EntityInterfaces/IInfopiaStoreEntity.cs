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
    /// Entity interface which represents the entity 'InfopiaStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IInfopiaStoreEntity: IStoreEntity
    {
        
        /// <summary> The ApiToken property of the Entity InfopiaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaStore"."ApiToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiToken { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IInfopiaStoreEntity AsReadOnlyInfopiaStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IInfopiaStoreEntity AsReadOnlyInfopiaStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'InfopiaStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class InfopiaStoreEntity : IInfopiaStoreEntity
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
                return (IInfopiaStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyInfopiaStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IInfopiaStoreEntity AsReadOnlyInfopiaStore() =>
            (IInfopiaStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IInfopiaStoreEntity AsReadOnlyInfopiaStore(IDictionary<object, object> objectMap) =>
            (IInfopiaStoreEntity) AsReadOnly(objectMap);
        
    }
}
