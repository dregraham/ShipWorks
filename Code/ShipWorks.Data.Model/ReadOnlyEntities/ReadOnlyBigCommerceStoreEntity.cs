﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'BigCommerceStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyBigCommerceStoreEntity : ReadOnlyStoreEntity, IBigCommerceStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyBigCommerceStoreEntity(IBigCommerceStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ApiUrl = source.ApiUrl;
            ApiUserName = source.ApiUserName;
            ApiToken = source.ApiToken;
            StatusCodes = source.StatusCodes;
            WeightUnitOfMeasure = source.WeightUnitOfMeasure;
            DownloadModifiedNumberOfDaysBack = source.DownloadModifiedNumberOfDaysBack;
            
            
            

            CopyCustomBigCommerceStoreData(source);
        }

        
        /// <summary> The ApiUrl property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."ApiUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 110<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiUrl { get; }
        /// <summary> The ApiUserName property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."ApiUserName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 65<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiUserName { get; }
        /// <summary> The ApiToken property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."ApiToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ApiToken { get; }
        /// <summary> The StatusCodes property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String StatusCodes { get; }
        /// <summary> The WeightUnitOfMeasure property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."WeightUnitOfMeasure"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 WeightUnitOfMeasure { get; }
        /// <summary> The DownloadModifiedNumberOfDaysBack property of the Entity BigCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BigCommerceStore"."DownloadModifiedNumberOfDaysBack"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DownloadModifiedNumberOfDaysBack { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBigCommerceStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBigCommerceStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomBigCommerceStoreData(IBigCommerceStoreEntity source);
    }
}
