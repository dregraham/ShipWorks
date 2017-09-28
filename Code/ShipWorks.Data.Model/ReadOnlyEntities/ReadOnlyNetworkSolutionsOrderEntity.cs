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
    /// Read-only representation of the entity 'NetworkSolutionsOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyNetworkSolutionsOrderEntity : ReadOnlyOrderEntity, INetworkSolutionsOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyNetworkSolutionsOrderEntity(INetworkSolutionsOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            NetworkSolutionsOrderID = source.NetworkSolutionsOrderID;
            
            
            
            NetworkSolutionsOrderSearch = source.NetworkSolutionsOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<INetworkSolutionsOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<INetworkSolutionsOrderSearchEntity>();

            CopyCustomNetworkSolutionsOrderData(source);
        }

        
        /// <summary> The NetworkSolutionsOrderID property of the Entity NetworkSolutionsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrder"."NetworkSolutionsOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 NetworkSolutionsOrderID { get; }
        
        
        
        public IEnumerable<INetworkSolutionsOrderSearchEntity> NetworkSolutionsOrderSearch { get; }
        
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
        public INetworkSolutionsOrderEntity AsReadOnlyNetworkSolutionsOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public INetworkSolutionsOrderEntity AsReadOnlyNetworkSolutionsOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomNetworkSolutionsOrderData(INetworkSolutionsOrderEntity source);
    }
}
