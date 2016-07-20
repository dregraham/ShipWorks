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
    /// Read-only representation of the entity 'BuyDotComStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyBuyDotComStoreEntity : ReadOnlyStoreEntity, IBuyDotComStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyBuyDotComStoreEntity(IBuyDotComStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FtpUsername = source.FtpUsername;
            FtpPassword = source.FtpPassword;
            
            
            

            CopyCustomBuyDotComStoreData(source);
        }

        
        /// <summary> The FtpUsername property of the Entity BuyDotComStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComStore"."FtpUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FtpUsername { get; }
        /// <summary> The FtpPassword property of the Entity BuyDotComStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComStore"."FtpPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FtpPassword { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBuyDotComStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBuyDotComStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomBuyDotComStoreData(IBuyDotComStoreEntity source);
    }
}
