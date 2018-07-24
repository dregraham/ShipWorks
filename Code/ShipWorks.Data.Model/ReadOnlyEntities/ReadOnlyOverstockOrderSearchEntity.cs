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
    /// Read-only representation of the entity 'OverstockOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOverstockOrderSearchEntity : IOverstockOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOverstockOrderSearchEntity(IOverstockOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OverstockOrderSearchID = source.OverstockOrderSearchID;
            OrderID = source.OrderID;
            OriginalOrderID = source.OriginalOrderID;
            SalesChannelName = source.SalesChannelName;
            WarehouseCode = source.WarehouseCode;
            
            
            OverstockOrder = (IOverstockOrderEntity) source.OverstockOrder?.AsReadOnly(objectMap);
            

            CopyCustomOverstockOrderSearchData(source);
        }

        
        /// <summary> The OverstockOrderSearchID property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."OverstockOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OverstockOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        /// <summary> The SalesChannelName property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."SalesChannelName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SalesChannelName { get; }
        /// <summary> The WarehouseCode property of the Entity OverstockOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrderSearch"."WarehouseCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String WarehouseCode { get; }
        
        
        public IOverstockOrderEntity OverstockOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOverstockOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOverstockOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOverstockOrderSearchData(IOverstockOrderSearchEntity source);
    }
}
