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
    /// Entity interface which represents the entity 'WalmartStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWalmartStoreEntity: IStoreEntity
    {
        
        /// <summary> The ConsumerID property of the Entity WalmartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartStore"."ConsumerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ConsumerID { get; }
        /// <summary> The PrivateKey property of the Entity WalmartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartStore"."PrivateKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2000<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PrivateKey { get; }
        /// <summary> The DownloadModifiedNumberOfDaysBack property of the Entity WalmartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartStore"."DownloadModifiedNumberOfDaysBack"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DownloadModifiedNumberOfDaysBack { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWalmartStoreEntity AsReadOnlyWalmartStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWalmartStoreEntity AsReadOnlyWalmartStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WalmartStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class WalmartStoreEntity : IWalmartStoreEntity
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
                return (IWalmartStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWalmartStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IWalmartStoreEntity AsReadOnlyWalmartStore() =>
            (IWalmartStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IWalmartStoreEntity AsReadOnlyWalmartStore(IDictionary<object, object> objectMap) =>
            (IWalmartStoreEntity) AsReadOnly(objectMap);
        
    }
}
