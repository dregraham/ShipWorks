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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'RakutenStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyRakutenStoreEntity : ReadOnlyStoreEntity, IRakutenStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyRakutenStoreEntity(IRakutenStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AuthKey = source.AuthKey;
            MarketplaceID = source.MarketplaceID;
            ShopURL = source.ShopURL;
            DownloadStartDate = source.DownloadStartDate;
            
            
            

            CopyCustomRakutenStoreData(source);
        }

        
        /// <summary> The AuthKey property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."AuthKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AuthKey { get; }
        /// <summary> The MarketplaceID property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."MarketplaceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceID { get; }
        /// <summary> The ShopURL property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."ShopURL"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShopURL { get; }
        /// <summary> The DownloadStartDate property of the Entity RakutenStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenStore"."DownloadStartDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> DownloadStartDate { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IRakutenStoreEntity AsReadOnlyRakutenStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IRakutenStoreEntity AsReadOnlyRakutenStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomRakutenStoreData(IRakutenStoreEntity source);
    }
}
