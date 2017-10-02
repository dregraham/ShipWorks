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
    /// Read-only representation of the entity 'ThreeDCartOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyThreeDCartOrderSearchEntity : IThreeDCartOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyThreeDCartOrderSearchEntity(IThreeDCartOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ThreeDCartOrderSearchID = source.ThreeDCartOrderSearchID;
            OrderID = source.OrderID;
            ThreeDCartOrderID = source.ThreeDCartOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            ThreeDCartOrder = (IThreeDCartOrderEntity) source.ThreeDCartOrder?.AsReadOnly(objectMap);
            

            CopyCustomThreeDCartOrderSearchData(source);
        }

        
        /// <summary> The ThreeDCartOrderSearchID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."ThreeDCartOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ThreeDCartOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The ThreeDCartOrderID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."ThreeDCartOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ThreeDCartOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IThreeDCartOrderEntity ThreeDCartOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IThreeDCartOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IThreeDCartOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomThreeDCartOrderSearchData(IThreeDCartOrderSearchEntity source);
    }
}
