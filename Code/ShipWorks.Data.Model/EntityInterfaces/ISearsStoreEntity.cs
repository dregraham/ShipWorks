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
    /// Entity interface which represents the entity 'SearsStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ISearsStoreEntity: IStoreEntity
    {
        
        /// <summary> The SearsEmail property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."SearsEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SearsEmail { get; }
        /// <summary> The Password property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The SecretKey property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."SecretKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SecretKey { get; }
        /// <summary> The SellerID property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."SellerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SellerID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearsStoreEntity AsReadOnlySearsStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearsStoreEntity AsReadOnlySearsStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'SearsStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class SearsStoreEntity : ISearsStoreEntity
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
                return (ISearsStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlySearsStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public ISearsStoreEntity AsReadOnlySearsStore() =>
            (ISearsStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public ISearsStoreEntity AsReadOnlySearsStore(IDictionary<object, object> objectMap) =>
            (ISearsStoreEntity) AsReadOnly(objectMap);
        
    }
}
