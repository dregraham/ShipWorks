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
    /// Read-only representation of the entity 'OrderCharge'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderChargeEntity : IOrderChargeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderChargeEntity(IOrderChargeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderChargeID = source.OrderChargeID;
            RowVersion = source.RowVersion;
            OrderID = source.OrderID;
            Type = source.Type;
            Description = source.Description;
            Amount = source.Amount;
            
            
            Order = source.Order?.AsReadOnly(objectMap);
            

            CopyCustomOrderChargeData(source);
        }

        
        /// <summary> The OrderChargeID property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."OrderChargeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OrderChargeID { get; }
        /// <summary> The RowVersion property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The Type property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."Type"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Type { get; }
        /// <summary> The Description property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The Amount property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."Amount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Amount { get; }
        
        
        public IOrderEntity Order { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderChargeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderChargeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderChargeData(IOrderChargeEntity source);
    }
}
