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
    /// Read-only representation of the entity 'RakutenOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyRakutenOrderSearchEntity : IRakutenOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyRakutenOrderSearchEntity(IRakutenOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            RakutenOrderSearchID = source.RakutenOrderSearchID;
            OrderID = source.OrderID;
            RakutenOrderID = source.RakutenOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            RakutenOrder = (IRakutenOrderEntity) source.RakutenOrder?.AsReadOnly(objectMap);
            

            CopyCustomRakutenOrderSearchData(source);
        }

        
        /// <summary> The RakutenOrderSearchID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."RakutenOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 RakutenOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The RakutenOrderID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."RakutenOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String RakutenOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IRakutenOrderEntity RakutenOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IRakutenOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IRakutenOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomRakutenOrderSearchData(IRakutenOrderSearchEntity source);
    }
}
