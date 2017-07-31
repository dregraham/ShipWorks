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
    /// Entity interface which represents the entity 'JetStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IJetStoreEntity: IStoreEntity
    {
        
        /// <summary> The ApiUser property of the Entity JetStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetStore"."ApiUser"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiUser { get; }
        /// <summary> The Secret property of the Entity JetStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetStore"."Secret"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Secret { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IJetStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IJetStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'JetStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class JetStoreEntity : IJetStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IJetStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IJetStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IJetStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyJetStoreEntity(this, objectMap);
        }
    }
}
