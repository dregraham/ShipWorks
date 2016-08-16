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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'NetworkSolutionsStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface INetworkSolutionsStoreEntity: IStoreEntity
    {
        
        /// <summary> The UserToken property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."UserToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UserToken { get; }
        /// <summary> The DownloadOrderStatuses property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."DownloadOrderStatuses"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DownloadOrderStatuses { get; }
        /// <summary> The StatusCodes property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StatusCodes { get; }
        /// <summary> The StoreUrl property of the Entity NetworkSolutionsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StoreUrl { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INetworkSolutionsStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INetworkSolutionsStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'NetworkSolutionsStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class NetworkSolutionsStoreEntity : INetworkSolutionsStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INetworkSolutionsStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new INetworkSolutionsStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (INetworkSolutionsStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyNetworkSolutionsStoreEntity(this, objectMap);
        }
    }
}
