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
    /// Read-only representation of the entity 'MagentoOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyMagentoOrderSearchEntity : IMagentoOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyMagentoOrderSearchEntity(IMagentoOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            MagentoOrderSearchID = source.MagentoOrderSearchID;
            OrderID = source.OrderID;
            MagentoOrderID = source.MagentoOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            MagentoOrder = source.MagentoOrder?.AsReadOnly(objectMap);
            

            CopyCustomMagentoOrderSearchData(source);
        }

        
        /// <summary> The MagentoOrderSearchID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."MagentoOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 MagentoOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The MagentoOrderID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."MagentoOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 MagentoOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IMagentoOrderEntity MagentoOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IMagentoOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IMagentoOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomMagentoOrderSearchData(IMagentoOrderSearchEntity source);
    }
}
