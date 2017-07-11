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
    /// Read-only representation of the entity 'GrouponOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGrouponOrderSearchEntity : IGrouponOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGrouponOrderSearchEntity(IGrouponOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            GrouponOrderSearchID = source.GrouponOrderSearchID;
            OrderID = source.OrderID;
            GrouponOrderID = source.GrouponOrderID;
            
            
            GrouponOrder = source.GrouponOrder?.AsReadOnly(objectMap);
            

            CopyCustomGrouponOrderSearchData(source);
        }

        
        /// <summary> The GrouponOrderSearchID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."GrouponOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 GrouponOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The GrouponOrderID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."GrouponOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GrouponOrderID { get; }
        
        
        public IGrouponOrderEntity GrouponOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGrouponOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGrouponOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGrouponOrderSearchData(IGrouponOrderSearchEntity source);
    }
}
