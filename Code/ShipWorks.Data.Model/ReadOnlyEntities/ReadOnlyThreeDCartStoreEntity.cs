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
    /// Entity interface which represents the entity 'ThreeDCartStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyThreeDCartStoreEntity : ReadOnlyStoreEntity, IThreeDCartStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyThreeDCartStoreEntity(IThreeDCartStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            StoreUrl = source.StoreUrl;
            ApiUserKey = source.ApiUserKey;
            TimeZoneID = source.TimeZoneID;
            StatusCodes = source.StatusCodes;
            DownloadModifiedNumberOfDaysBack = source.DownloadModifiedNumberOfDaysBack;
            RestUser = source.RestUser;
            
            
            

            CopyCustomThreeDCartStoreData(source);
        }

        
        /// <summary> The StoreUrl property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 110<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StoreUrl { get; }
        /// <summary> The ApiUserKey property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."ApiUserKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 65<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiUserKey { get; }
        /// <summary> The TimeZoneID property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."TimeZoneID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String TimeZoneID { get; }
        /// <summary> The StatusCodes property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String StatusCodes { get; }
        /// <summary> The DownloadModifiedNumberOfDaysBack property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."DownloadModifiedNumberOfDaysBack"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DownloadModifiedNumberOfDaysBack { get; }
        /// <summary> The RestUser property of the Entity ThreeDCartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartStore"."RestUser"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean RestUser { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IThreeDCartStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IThreeDCartStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomThreeDCartStoreData(IThreeDCartStoreEntity source);
    }
}
