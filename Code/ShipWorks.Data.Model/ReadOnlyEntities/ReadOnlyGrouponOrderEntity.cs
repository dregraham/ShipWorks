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
    /// Read-only representation of the entity 'GrouponOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGrouponOrderEntity : ReadOnlyOrderEntity, IGrouponOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGrouponOrderEntity(IGrouponOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            GrouponOrderID = source.GrouponOrderID;
            ParentOrderID = source.ParentOrderID;
            
            
            
            GrouponOrderSearch = source.GrouponOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IGrouponOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IGrouponOrderSearchEntity>();

            CopyCustomGrouponOrderData(source);
        }

        
        /// <summary> The GrouponOrderID property of the Entity GrouponOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrder"."GrouponOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GrouponOrderID { get; }
        /// <summary> The ParentOrderID property of the Entity GrouponOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrder"."ParentOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ParentOrderID { get; }
        
        
        
        public IEnumerable<IGrouponOrderSearchEntity> GrouponOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IGrouponOrderEntity AsReadOnlyGrouponOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGrouponOrderEntity AsReadOnlyGrouponOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGrouponOrderData(IGrouponOrderEntity source);
    }
}
