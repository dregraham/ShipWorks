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
    /// Entity interface which represents the entity 'RakutenStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IRakutenStoreEntity: IStoreEntity
    {
        
        /// <summary> The AuthKey property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."AuthKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AuthKey { get; }
        /// <summary> The MarketplaceID property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."MarketplaceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceID { get; }
        /// <summary> The ShopURL property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."ShopURL"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShopURL { get; }
        /// <summary> The DownloadStartDate property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."DownloadStartDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> DownloadStartDate { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenStoreEntity AsReadOnlyRakutenStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenStoreEntity AsReadOnlyRakutenStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'RakutenStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class RakutenStoreEntity : IRakutenStoreEntity
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
                return (IRakutenStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyRakutenStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IRakutenStoreEntity AsReadOnlyRakutenStore() =>
            (IRakutenStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IRakutenStoreEntity AsReadOnlyRakutenStore(IDictionary<object, object> objectMap) =>
            (IRakutenStoreEntity) AsReadOnly(objectMap);
        
    }
}
