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
    /// Read-only representation of the entity 'OrderMotionOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderMotionOrderSearchEntity : IOrderMotionOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderMotionOrderSearchEntity(IOrderMotionOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderMotionOrderSearchID = source.OrderMotionOrderSearchID;
            OrderID = source.OrderID;
            OrderMotionShipmentID = source.OrderMotionShipmentID;
            
            
            OrderMotionOrder = source.OrderMotionOrder?.AsReadOnly(objectMap);
            

            CopyCustomOrderMotionOrderSearchData(source);
        }

        
        /// <summary> The OrderMotionOrderSearchID property of the Entity OrderMotionOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrderSearch"."OrderMotionOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OrderMotionOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity OrderMotionOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The OrderMotionShipmentID property of the Entity OrderMotionOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrderSearch"."OrderMotionShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OrderMotionShipmentID { get; }
        
        
        public IOrderMotionOrderEntity OrderMotionOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderMotionOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderMotionOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderMotionOrderSearchData(IOrderMotionOrderSearchEntity source);
    }
}
