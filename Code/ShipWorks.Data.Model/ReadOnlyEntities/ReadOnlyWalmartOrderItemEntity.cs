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
    /// Read-only representation of the entity 'WalmartOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWalmartOrderItemEntity : ReadOnlyOrderItemEntity, IWalmartOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWalmartOrderItemEntity(IWalmartOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            LineNumber = source.LineNumber;
            OnlineStatus = source.OnlineStatus;
            OriginalOrderID = source.OriginalOrderID;
            
            
            

            CopyCustomWalmartOrderItemData(source);
        }

        
        /// <summary> The LineNumber property of the Entity WalmartOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderItem"."LineNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LineNumber { get; }
        /// <summary> The OnlineStatus property of the Entity WalmartOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderItem"."OnlineStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OnlineStatus { get; }
        /// <summary> The OriginalOrderID property of the Entity WalmartOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderItem"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWalmartOrderItemData(IWalmartOrderItemEntity source);
    }
}
