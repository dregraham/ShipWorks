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
    /// Read-only representation of the entity 'LemonStandOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyLemonStandOrderSearchEntity : ILemonStandOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyLemonStandOrderSearchEntity(ILemonStandOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            LemonStandOrderSearchID = source.LemonStandOrderSearchID;
            OrderID = source.OrderID;
            LemonStandOrderID = source.LemonStandOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            LemonStandOrder = (ILemonStandOrderEntity) source.LemonStandOrder?.AsReadOnly(objectMap);
            

            CopyCustomLemonStandOrderSearchData(source);
        }

        
        /// <summary> The LemonStandOrderSearchID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."LemonStandOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 LemonStandOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The LemonStandOrderID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."LemonStandOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LemonStandOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public ILemonStandOrderEntity LemonStandOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ILemonStandOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ILemonStandOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomLemonStandOrderSearchData(ILemonStandOrderSearchEntity source);
    }
}
