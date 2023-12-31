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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'GrouponStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGrouponStoreEntity : ReadOnlyStoreEntity, IGrouponStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGrouponStoreEntity(IGrouponStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            SupplierID = source.SupplierID;
            Token = source.Token;
            
            
            

            CopyCustomGrouponStoreData(source);
        }

        
        /// <summary> The SupplierID property of the Entity GrouponStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponStore"."SupplierID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SupplierID { get; }
        /// <summary> The Token property of the Entity GrouponStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponStore"."Token"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Token { get; }
        
        
        
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
        public IGrouponStoreEntity AsReadOnlyGrouponStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGrouponStoreEntity AsReadOnlyGrouponStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGrouponStoreData(IGrouponStoreEntity source);
    }
}
