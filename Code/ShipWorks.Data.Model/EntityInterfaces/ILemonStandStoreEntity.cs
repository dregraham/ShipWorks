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
    /// Entity interface which represents the entity 'LemonStandStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ILemonStandStoreEntity: IStoreEntity
    {
        
        /// <summary> The Token property of the Entity LemonStandStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandStore"."Token"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Token { get; }
        /// <summary> The StoreURL property of the Entity LemonStandStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandStore"."StoreURL"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StoreURL { get; }
        /// <summary> The StatusCodes property of the Entity LemonStandStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String StatusCodes { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ILemonStandStoreEntity AsReadOnlyLemonStandStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ILemonStandStoreEntity AsReadOnlyLemonStandStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'LemonStandStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class LemonStandStoreEntity : ILemonStandStoreEntity
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
                return (ILemonStandStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyLemonStandStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public ILemonStandStoreEntity AsReadOnlyLemonStandStore() =>
            (ILemonStandStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public ILemonStandStoreEntity AsReadOnlyLemonStandStore(IDictionary<object, object> objectMap) =>
            (ILemonStandStoreEntity) AsReadOnly(objectMap);
        
    }
}
