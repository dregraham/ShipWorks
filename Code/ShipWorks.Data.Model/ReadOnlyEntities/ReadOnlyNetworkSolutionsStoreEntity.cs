///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'NetworkSolutionsStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyNetworkSolutionsStoreEntity : ReadOnlyStoreEntity, INetworkSolutionsStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyNetworkSolutionsStoreEntity(INetworkSolutionsStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UserToken = source.UserToken;
            DownloadOrderStatuses = source.DownloadOrderStatuses;
            StatusCodes = source.StatusCodes;
            StoreUrl = source.StoreUrl;
            
            
            

            CopyCustomNetworkSolutionsStoreData(source);
        }

        
        /// <summary> The UserToken property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."UserToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UserToken { get; }
        /// <summary> The DownloadOrderStatuses property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."DownloadOrderStatuses"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DownloadOrderStatuses { get; }
        /// <summary> The StatusCodes property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StatusCodes { get; }
        /// <summary> The StoreUrl property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StoreUrl { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INetworkSolutionsStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INetworkSolutionsStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomNetworkSolutionsStoreData(INetworkSolutionsStoreEntity source);
    }
}
